using UnityEngine;
using System.Collections;
using org.opensourcephysics.orst.spins; // allows access to the Complex.cs functions
using UniMat;
using SpinVectorN;
using UnityEngine.EventSystems;


public class AllMathData : MonoBehaviour {
	static int State = 0;
	private int Spin = 0;
	private float up = 0f;
	private float mid = 0f;
	private float down = 0f;
	private float prevup = -1f;
	private float prevmid = -1f;
	private int prevDir = -2;
	private float prevdown = -1f;
	static private float mainPhi = 45f;
	static private float mainTheta = 90f;
	static private float mainPhi2 = 45f;
	static private float mainTheta2 = 90f;
	static private float UserA,UserB,UserC,UserD,UserE,UserF;	// input data from the user
	static private string UserDir = "Z"; //default				//input direction from user
	public bool CombinedBeam = false;
	public bool CombinedTM = false;
	public bool CombinedMB = false;
	public bool CombinedTB = false;
	private GameObject Onstage;
	private int AnaCount = 0;
	
	
	void Awake(){
		Onstage = GameObject.Find ("OnStage");
		SetTransformMatrix();
	}

	public int GetSpinState(){
		return Spin;
	}

	public void SetCombinedBeam(bool beam){
		CombinedBeam = beam;
	}

	public void SetCombined2(bool top,bool mid,bool bot){
		if(top == true && mid == true){
			CombinedTM = true;
			CombinedTB = false;
			CombinedMB = false;
		}
		if(top == true && bot == true){
			CombinedTB = true;
			CombinedTM = false;
			CombinedMB = false;
		}
		if(bot == true && mid == true){
			CombinedMB = true;
			CombinedTM = false;
			CombinedTB = false;
		}
	}

	public bool checkCombined(string s){
		if (Onstage.GetComponent<BuildPath>().GetConTop(s) == Onstage.GetComponent<BuildPath>().GetConBot(s) &&
		    Onstage.GetComponent<BuildPath>().GetConTop(s) == Onstage.GetComponent<BuildPath>().GetConMid(s)){
			SetCombinedBeam(true);
			return false;
		}
		if(Onstage.GetComponent<BuildPath>().GetConTop(s) == Onstage.GetComponent<BuildPath>().GetConBot(s)){
			if(Onstage.GetComponent<BuildPath>().GetConTop(s) == null ||
			   Onstage.GetComponent<BuildPath>().GetConBot(s) == null){
				
			}else{
				SetCombinedBeam(true);
			}
			return false;
		}
		if(Onstage.GetComponent<BuildPath>().GetConTop(s) == Onstage.GetComponent<BuildPath>().GetConMid(s)){
			if(Onstage.GetComponent<BuildPath>().GetConTop(s) == null ||
			   Onstage.GetComponent<BuildPath>().GetConMid(s) == null){
				
			}else{
				SetCombinedBeam(true);
				SetCombined2(true,true,false);
			}
			return false;
		}
		if(Onstage.GetComponent<BuildPath>().GetConBot(s) == Onstage.GetComponent<BuildPath>().GetConMid(s)){
			if(Onstage.GetComponent<BuildPath>().GetConBot(s) == null ||
			   Onstage.GetComponent<BuildPath>().GetConMid(s) == null){

			}else{
				SetCombinedBeam(true);
				SetCombined2(false,true,true);
			}
			return false;
		}
		return false;
	}

	//data from users for Phi and Theta
	public void SetDegrees(int phi, int theta){
		mainPhi2 = phi;
		mainTheta2 = theta;
	}

	//data from users for real1, img1, real2, img2 and Direction
	public void SetUserVals(float A, float B,float C,float D,string Dir){
		UserA = A;
		UserB = B;
		UserC = C;
		UserD = D;
		UserDir = Dir;
		GunMath(Spin);
	}

	//data from users for real1, img1, real2, img2, real3, img3 and direction
	public void SetUserVals(float A, float B,float C,float D,float E, float F,string Dir){
		UserA = A;
		UserB = B;
		UserC = C;
		UserD = D;
		UserE = E;
		UserF = F;
		UserDir = Dir;
		GunMath(Spin);
	}

	//gets the initial state from the gui (unknown1,2,3,4,rand,user)
	public void SetState(int st){
		State = st;
		//print ("State: "+State);
		GunMath(Spin);
	}

	//used when looking at directions on the Analyzer components
	private int GetDirectionAna(string obj, int spin){ // child 2 is the Axis object from the analyzer component
		return Onstage.GetComponent<BuildPath>().GetDirection(obj);
	}
	
	//used to find the direction on a magnet component
	private int GetDirectionMag(string obj){ //child 6 is the Axis onject from the magnet component
		return Onstage.GetComponent<BuildPath>().GetDirection(obj);
	}
	
	//called whenever the initial gun data needs to be updates
	public void SetGunStates(int curSpin){
		Spin = curSpin;
		GunMath(curSpin); //sets up the unknowns for the gun
	}

	//this function will use whatever data it is given and calculate which path the particle will take
	//up,down,mid, or die these are the only states possible - this function is called from another script to start the math
	public int DoMath(string input, string output, int curSpin){
		//go up: 0, down: 1; mid: 2
		int curDir = -1;
		int curDir2 = -1;
		int retVal = 0;
		up = 0f;
		mid = 0f;
		down = 0f;
		retVal = 0;
		//.print ("math output: "+output);
		if(output == "" || output == null){
			//do nothing
		}
		else if(output.Substring(0,3) == "Ana"){ // if the output is an analyzer then gets the direction and sets up the inits for the analyzer
			curDir = GetDirectionAna(output,curSpin); //get direction from the analyzer
			AnalyzerMath(curSpin,curDir); 
		}
		else if(output.Substring(0,3) == "Mag"){
			curDir2 = GetDirectionMag(output); //get direction from the magnet
			MagnetMath(curSpin,curDir2); 
		}
		if(input.Substring(0,3) == "Ana"){
			prevDir = GetDirectionAna(input,curSpin); //get direction from the analyzer 
		}
		if(output != "" && output != null && output != "Null"){
			if(input != output && curSpin == 0){
				//print (output);
				retVal = SpinHalfMath(input, output,curSpin,curDir);
			}
			
			//spin 1
			if(input != output && curSpin == 1){
				
				retVal = SpinOneMath(input, output,curSpin,curDir);
				
			}
		}
		return retVal; //return which path the partical is going (0 - up, 1 - down, 2 - middle )
	}

	public int SpinOneMath(string input, string output, int curSpin, int curDir){
		MyId = MyId.Identity();
		string preserver = output;
		switch(output.Substring(0,3)){
		case "Ana": //determine which path on the output analyzer the atom will take
			switch(input.Substring(0,3)){
			case "gun": //100% chance from gun to ana
				Complex myTest = testGun.dotProduct(AnaVec); //conjugation in the dotproduct
				up = (float)myTest.modSquared();
				Complex myTest2 = testGun.dotProduct(AnaVecMid); //conjugation in the dotproduct
				mid = (float)myTest2.modSquared();
				down = 1.0f-(up+mid);
				LastInput = testGun;
				break;
			case "Ana":
				AnaInit(curSpin,prevDir,1);
				checkCombined(input);
				if(prevup == 1){
					Complex myTest3;
					Complex myTest4;
					if(CombinedBeam == true){
						up = Onstage.GetComponent<BuildPath>().GetProbs(input,0);
						mid = Onstage.GetComponent<BuildPath>().GetProbs(input,1);
						down = Onstage.GetComponent<BuildPath>().GetProbs(input,2);
						if(CombinedTM == true){
							up = Onstage.GetComponent<BuildPath>().GetProbs(input,0)+(Onstage.GetComponent<BuildPath>().GetProbs(input,2)/2);
							mid = 1f-up;
							down = 0;
						}else if(CombinedMB == true){
							up = 0;
							mid = Onstage.GetComponent<BuildPath>().GetProbs(input,1)+(Onstage.GetComponent<BuildPath>().GetProbs(input,0)/2);
							down = 1f-mid;
						}
					}
					if(!CombinedBeam){
						myTest3 = testAna.dotProduct(AnaVec);
						up = (float)myTest3.modSquared();
						myTest4 = testAna.dotProduct(AnaVecMid);
						mid = (float)myTest4.modSquared();
						down = 1.0f-(up+mid);
						LastInput = testAna;
					}
				}else if(prevmid == 1){
					Complex myTest3;
					Complex myTest4;
					if(CombinedBeam == true){
						up = Onstage.GetComponent<BuildPath>().GetProbs(input,0);
						mid = Onstage.GetComponent<BuildPath>().GetProbs(input,1);
						down = Onstage.GetComponent<BuildPath>().GetProbs(input,2);
						if(CombinedTM == true){
							up = Onstage.GetComponent<BuildPath>().GetProbs(input,0)+(Onstage.GetComponent<BuildPath>().GetProbs(input,2)/2);
							mid = 1f-up;
							down = 0;
						}else if(CombinedMB == true){
							up = 0;
							mid = Onstage.GetComponent<BuildPath>().GetProbs(input,1)+(Onstage.GetComponent<BuildPath>().GetProbs(input,0)/2);
							down = 1f-mid;
						}
					}
					if(!CombinedBeam){
						myTest3 = testAnaMid.dotProduct(AnaVec);
						up = (float)myTest3.modSquared();
						myTest4 = testAnaMid.dotProduct(AnaVecMid);
						mid = (float)myTest4.modSquared();
						down = 1.0f-(up+mid);
						LastInput = testAnaMid;
					}
				}
				CombinedBeam = false;
				CombinedTM = false;
				CombinedTB = false;
				CombinedMB = false;
				break;
			case "Mag": //100% chance from mag to ana
				Complex testing;
				Complex myTestM1 = testMagN.dotProduct(AnaVec); //conjugation in the dotproduct
				up = (float)myTestM1.modSquared();
				testing = testMagN.dotProduct(AnaVecMid);
				mid = (float)testing.modSquared();
				down = 1.0f-(up+mid);
				LastInput = testMagN;

				break;
			case "Cou": // 0% chance from counter to ana
				prevup = -1;
				prevdown = -1;
				curDir = -1;
				prevDir = -1;
				AnaCount = 0;
				break;
			default:
				break;
			}
			break;
		case "Cou":
			prevup = -1;
			prevdown = -1;
			curDir = -1;
			prevDir = -1;
			AnaCount = 0;
			break;
		case "Mag": //100% chance from mag to ana
			AnaInit(curSpin,prevDir,1); //to get current ana input if it exists
			float Strength1 = 6.283185307179586f*GameObject.Find(output).transform.GetChild(7).GetComponent<MagnetCounter>().MagnetStrength() / 72.0f;
			SpinVector Last1 = new SpinVector();
			Complex part1 = new Complex(0,-Mathf.Sin(Strength1));
			Complex part2 = new Complex(Mathf.Cos(Strength1)-1,0);
			switch(input.Substring(0,3)){
			case "gun":
				Last1 = testGun;
				break;
			case "Ana":
				Last1 = testAna;
				break;
			case "Mag":
				Last1 = testMagN;
				break;
			default:
				break;
			}
			MagM = testMag.mMul (part1);
			MagM = MagM.mAdd (MyId);
			MagM2 = testMag.SquareMatrix().mMul (part2);
			MagM2 = MagM.mAdd(MagM2);
			testMagN = MagM2.mvMul(Last1);
			up = 1;
			mid = 0;
			down = 0;
			break;
		default:
			break;
		}
		float myR = Random.Range(0.0f,1.0f);
		Onstage.GetComponent<BuildPath>().UpdateProb(preserver,up,mid,down);


		if(myR <= up){
			prevup = 1;
			prevdown = 0;
			prevmid = 0;
			return 0;
		}else if((myR - up) < mid){
			prevup = 0;
			prevdown = 0;
			prevmid = 1;
			return 1;
		}else{
			prevup = 0;
			prevdown = 1;
			prevmid = 0;
			return 2;
		}
	}

	public int SpinOneCycle(string output){
		up = Onstage.GetComponent<BuildPath>().GetProbs(output,0);
		mid = Onstage.GetComponent<BuildPath>().GetProbs(output,1);
		down = Onstage.GetComponent<BuildPath>().GetProbs(output,2);
		
		float myR = Random.Range(0.0f,1.0f);
		if(myR <= up){
			prevup = 1;
			prevdown = 0;
			prevmid = 0;
			return 0;
		}else if((myR - up) < mid){
			prevup = 0;
			prevdown = 0;
			prevmid = 1;
			return 1;
		}else{
			prevup = 0;
			prevdown = 1;
			prevmid = 0;
			return 2;
		}
	}


	Matrix MagM = new Matrix();
	Matrix MagM2 = new Matrix();
	Matrix MyId = new Matrix();
	SpinVector LastInput = new SpinVector();
	public int SpinHalfMath(string input, string output, int curSpin, int curDir){
		MyId = MyId.Identity();
		string preserver = output;
		//print ("Input: "+input+" output: "+output);
		switch(output.Substring(0,3)){
		case "Ana": //determine which path on the output analyzer the atom will take
			AnaCount++;
			switch(input.Substring(0,3)){
			case "gun":
				Complex myTest = testGun.dotProduct(AnaVec); //conjugation in the dotproduct
				up = (float)myTest.modSquared();
				down = 1.0f-up;
				Onstage.GetComponent<BuildPath>().UpdateProb(output,up,0,down);
				LastInput = testGun;
				break;
			case "Ana":
				AnaInit(curSpin,prevDir,1);
				checkCombined(input);
				Complex myTest2 = testAna.dotProduct(AnaVec);
				up = (float)myTest2.modSquared();
				down = 1.0f-up;
				if(CombinedBeam == true){
					myTest2 = LastInput.dotProduct(AnaVec);
					up = 1.0f;
					down = 0.0f;
				}
				if(!CombinedBeam){
					LastInput = testAna;
				}
				CombinedBeam = false;
				Onstage.GetComponent<BuildPath>().UpdateProb(output,up,0,down);
				break;
			case "Mag": //100% chance from mag to ana
				Complex myTestM = testMagN.dotProduct(AnaVec); //conjugation in the dotproduct
				up = (float)myTestM.modSquared();
				//print (up);
				down = 1.0f-up;
				LastInput = testMagN;
				Onstage.GetComponent<BuildPath>().UpdateProb(output,up,0,down);
				break;
			case "Cou": // 0% chance from counter to ana
				prevup = -1;
				prevdown = -1;
				curDir = -1;
				prevDir = -1;
				AnaCount = 0;
				break;
			default:
				break;
			}
			break;
		case "Cou":
			prevup = -1;
			prevdown = -1;
			curDir = -1;
			prevDir = -1;
			AnaCount = 0;
			break;
		case "Mag": //100% chance from mag to ana
			AnaInit(curSpin,prevDir,1);
			float Strength1 = 6.283185307179586f*GameObject.Find(output).transform.GetChild(7).GetComponent<MagnetCounter>().MagnetStrength() / 72.0f;
			SpinVector Last = new SpinVector();
			Complex part1 = new Complex(0,-Mathf.Sin(Strength1));
			Complex part2 = new Complex(Mathf.Cos(Strength1)-1,0);
			switch(input.Substring(0,3)){
			case "gun":
				Last = testGun;
				break;
			case "Ana":
				Last = testAna;
				break;
			case "Mag":
				Last = testMagN;
				break;
			default:
				break;
			}
			MagM = testMag.mMul (part1);
			MagM = MagM.mAdd (MyId);
			MagM2 = testMag.SquareMatrix().mMul (part2);
			MagM2 = MagM.mAdd(MagM2);
			testMagN = MagM2.mvMul(Last);
			up = 1.0f;
			down = 0.0f;
			Onstage.GetComponent<BuildPath>().UpdateProb(output,up,0,down);
			break;
		default:
			break;
		}
		float myR = Random.Range(0.0f,1.0f);





		if(myR < up){
			prevup = 1;
			prevdown = 0;
			return 0; //go up
		}else{
			prevup = 0;
			prevdown = 1;
			return 2; //go down
		}
	}

	public int SpinHalfCycle(string output){
		up = Onstage.GetComponent<BuildPath>().GetProbs(output,0);
		down = Onstage.GetComponent<BuildPath>().GetProbs(output,2);

		float myR = Random.Range(0.0f,1.0f);
		if(myR < up){
			prevup = 1;
			prevdown = 0;
			return 0; //go up
		}else{
			prevup = 0;
			prevdown = 1;
			return 2; //go down
		}
	}


	SpinVector testAna = new SpinVector();
	SpinVector testAnaMid = new SpinVector();
	SpinVector testAnaDown = new SpinVector();
	private void AnaInit(int Spin, int Dir,int updown){
		if(Dir == 2){
			mainTheta = Mathf.PI/2f;
			mainPhi = 0f;
		}
		if(Dir == 3){
			mainTheta = Mathf.PI/2f;
			mainPhi = Mathf.PI/2f;
		}
		if(Dir == 0){
			mainTheta = 0f;
			mainPhi = 0f;
		}
		if(Dir == 1){ //this grabs the data from the user (NHAT)
			mainTheta = mainTheta2*0.0174532925f;
			mainPhi = mainPhi2*0.0174532925f;
		}

		if(Spin == 0){
			testAna.data[0] = new Complex(Mathf.Cos (mainTheta/2f),0);
			testAna.data[1] = new Complex(Mathf.Cos (mainPhi)*Mathf.Sin (mainTheta/2f),Mathf.Sin (mainPhi)*Mathf.Sin(mainTheta/2f));
			testAna.data[2] = new Complex(0,0);

		}
		if(Spin == 1){

			testAna.data[0] = new Complex((1f/2f)*Mathf.Cos (mainPhi)+(1f/2f)*Mathf.Cos (mainTheta)*Mathf.Cos(mainPhi),-((1f/2f)*Mathf.Sin (mainPhi)+(1f/2f)*Mathf.Sin(mainPhi)*Mathf.Cos(mainTheta)));
			testAna.data[1] = new Complex((1f/Mathf.Sqrt (2f))*Mathf.Sin (mainTheta),0);
			testAna.data[2] = new Complex((1f/2f)*Mathf.Cos (mainTheta)-(1f/2f)*Mathf.Cos (mainTheta)*Mathf.Cos(mainPhi),(1f/2f)*Mathf.Sin(mainPhi)-(1f/2f)*Mathf.Sin(mainPhi)*Mathf.Cos(mainTheta));
			
			testAnaMid.data[0] = new Complex(-((Mathf.Sin (mainTheta)*Mathf.Cos(mainPhi))/Mathf.Sqrt (2f)),((Mathf.Sin(mainTheta)*Mathf.Sin(mainPhi))/Mathf.Sqrt(2f)));
			testAnaMid.data[1] = new Complex(Mathf.Cos (mainTheta),0);
			testAnaMid.data[2] = new Complex(((Mathf.Cos (mainPhi)*Mathf.Sin (mainTheta))/Mathf.Sqrt (2f)),((Mathf.Sin(mainPhi)*Mathf.Sin(mainTheta))/Mathf.Sqrt(2f)));
			testAnaDown.data[0] = new Complex((Mathf.Cos (mainPhi)/2f)-((Mathf.Cos (mainPhi)*Mathf.Cos(mainTheta))/2f),-(Mathf.Sin (mainPhi)/2f)+((Mathf.Sin (mainPhi)*Mathf.Cos (mainTheta))/2f));
			testAnaDown.data[1] = new Complex(-(Mathf.Sin (mainTheta)/Mathf.Sqrt(2f)),0);
			testAnaDown.data[2] = new Complex((Mathf.Cos (mainPhi)/2f)+((Mathf.Cos (mainPhi)*Mathf.Cos(mainTheta))/2f),(Mathf.Sin (mainPhi)/2f)+((Mathf.Sin (mainPhi)*Mathf.Cos (mainTheta))/2f));
		}
	}


	SpinVector testGun = new SpinVector();
	SpinVector UserGun = new SpinVector();
	SpinVector testMagN = new SpinVector();
	Matrix MHalfXZ = new Matrix();
	Matrix MHalfYZ = new Matrix();
	Matrix MOneXZ = new Matrix();
	Matrix MOneYZ = new Matrix();


	public void SetTransformMatrix(){
		MHalfXZ.data[0][0] = new Complex(1f/Mathf.Sqrt(2f),0);
		MHalfXZ.data[0][1] = new Complex(1f/Mathf.Sqrt(2f),0);
		MHalfXZ.data[1][0] = new Complex(1f/Mathf.Sqrt(2f),0);
		MHalfXZ.data[1][1] = new Complex(-1f/Mathf.Sqrt(2f),0);

		MHalfYZ.data[0][0] = new Complex(1f/Mathf.Sqrt(2f),0);
		MHalfYZ.data[0][1] = new Complex(1f/Mathf.Sqrt(2f),0);
		MHalfYZ.data[1][0] = new Complex(0,1f/Mathf.Sqrt(2f));
		MHalfYZ.data[1][1] = new Complex(0,-1f/Mathf.Sqrt(2f));

		MOneXZ.data[0][0] = new Complex(1f/2f,0);
		MOneXZ.data[0][1] = new Complex(1f/Mathf.Sqrt(2f),0);
		MOneXZ.data[0][2] = new Complex(1f/2f,0);
		MOneXZ.data[1][0] = new Complex(1f/Mathf.Sqrt(2f),0);
		MOneXZ.data[1][1] = new Complex(0,0);
		MOneXZ.data[1][2] = new Complex(-1f/Mathf.Sqrt(2f),0);
		MOneXZ.data[2][0] = new Complex(1f/2f,0);
		MOneXZ.data[2][1] = new Complex(-1f/Mathf.Sqrt(2f),0);
		MOneXZ.data[2][2] = new Complex(1f/2f,0);

		MOneYZ.data[0][0] = new Complex(1f/2f,0);
		MOneYZ.data[0][1] = new Complex(1f/Mathf.Sqrt(2f),0);
		MOneYZ.data[0][2] = new Complex(1f/2f,0);
		MOneYZ.data[1][0] = new Complex(0,1f/Mathf.Sqrt(2f));
		MOneYZ.data[1][1] = new Complex(0,0);
		MOneYZ.data[1][2] = new Complex(0,-1f/Mathf.Sqrt(2f));
		MOneYZ.data[2][0] = new Complex(-1f/2f,0);
		MOneYZ.data[2][1] = new Complex(1f/Mathf.Sqrt(2f),0);
		MOneYZ.data[2][2] = new Complex(-1f/2f,0);
	}

	public void GunMath(int Spin){
		if(Spin == 0){ //sets up the initial gun states based on user choice, [0,1] and [1,1] are ignored
			if(State == 0){
				testGun.data[0] = new Complex(1,0);
				testGun.data[1] = new Complex(0,0);
				testGun.data[2] = new Complex(0,0);
			}
			if(State == 1){
				testGun.data[0] = new Complex(1f/Mathf.Sqrt(2f),0);
				testGun.data[1] = new Complex(0,-1f/Mathf.Sqrt(2f));
				testGun.data[2] = new Complex(0,0);
			}
			if(State == 2){
				testGun.data[0] = new Complex(1f/Mathf.Sqrt(2f),0);
				testGun.data[1] = new Complex(-(1f/(2f*Mathf.Sqrt(2f))),-(Mathf.Sqrt(3f)/(2f*Mathf.Sqrt(2f))));
				testGun.data[2] = new Complex(0,0);
			}
			if(State == 3){
				testGun.data[0] = new Complex(1f/2f,0);
				testGun.data[1] = new Complex(3f/4f,-Mathf.Sqrt(3f)/4f);
				testGun.data[2] = new Complex(0,0);
			}
			if(State == 4){ //user States
				//degree *(2pi/360)
				testGun.data[0] = new Complex(UserA,UserB);
				testGun.data[1] = new Complex(UserC,UserD);
				testGun.data[2] = new Complex(0,0);
				testGun = testGun.normalize();
				if(UserDir == "X"){
					testGun = MHalfXZ.mvMul(testGun);
				}else if(UserDir == "Y"){
					testGun = MHalfYZ.mvMul(testGun);
				}
			}
			if(State == 5){
				int rand = Random.Range (0,2);
				if(rand == 0){
					testGun.data[0] = new Complex(1f,0);
					testGun.data[1] = new Complex(0,0);
					testGun.data[2] = new Complex(0,0);
				}else{
					testGun.data[0] = new Complex(0,0);
					testGun.data[1] = new Complex(1f,0);
					testGun.data[2] = new Complex(0,0);
				}
			}
		}

		if(Spin == 1){ //sets up the initial gun states based on user choice, [0,1] and [1,1] and [2,1] are ignored
			if(State == 0){ 
				testGun.data[0] = new Complex(1f/2f,0);
				testGun.data[1] = new Complex(0,1f/Mathf.Sqrt(2f));
				testGun.data[2] = new Complex(-(1f/2f),0);
			}
			if(State == 1){
				testGun.data[0] = new Complex(1f/2f,0);
				testGun.data[1] = new Complex(1f/(2f*Mathf.Sqrt(2f)),(Mathf.Sqrt(3f)/(2f*Mathf.Sqrt(2f))));
				testGun.data[2] = new Complex(-1f/4f,(Mathf.Sqrt(3f)/4f));
				
			}
			if(State == 2){
				testGun.data[0] = new Complex(1f/Mathf.Sqrt(3f),0);
				testGun.data[1] = new Complex(0,-1f/Mathf.Sqrt (3f));
				testGun.data[2] = new Complex(-1f/Mathf.Sqrt (3f),0);
			}
			if(State == 3){
				testGun.data[0] = new Complex(1f/Mathf.Sqrt(2f),0);
				testGun.data[1] = new Complex(0,0);
				testGun.data[2] = new Complex(0,-1f/Mathf.Sqrt(2f));
			}
			if(State == 4){ //user States
				testGun.data[0] = new Complex(UserA,UserB);
				testGun.data[1] = new Complex(UserC,UserD);
				testGun.data[2] = new Complex(UserE,UserF);
				testGun.normalize();
				if(UserDir == "X"){
					testGun = MOneXZ.mvMul(testGun);
				}else if(UserDir == "Y"){
					testGun = MOneYZ.mvMul(testGun);
				}
			}
			if(State == 5){
				int rand = Random.Range (0,3);
				if(rand == 0){
					testGun.data[0] = new Complex(1f,0);
					testGun.data[1] = new Complex(0,0);
					testGun.data[2] = new Complex(0,0);
				}else if(rand == 1){
					testGun.data[0] = new Complex(0,0);
					testGun.data[1] = new Complex(1f,0);
					testGun.data[2] = new Complex(0,0);
				}else{
					testGun.data[0] = new Complex(0,0);
					testGun.data[1] = new Complex(0,0);
					testGun.data[2] = new Complex(1f,0);
				}

			}
		}
	}

	/*
	 * Sets the data for the analyzer
	 * 
	 */

	SpinVector AnaVec = new SpinVector();
	SpinVector AnaVecMid = new SpinVector();
	SpinVector AnaVecDown = new SpinVector();
	public void AnalyzerMath(int Spin, int Dir){
		if(Dir == 2){
			mainTheta = Mathf.PI/2f;
			mainPhi = 0;
		}
		if(Dir == 3){
			mainTheta = Mathf.PI/2f;
			mainPhi = Mathf.PI/2f;
		}
		if(Dir == 0){
			mainTheta = 0;
			mainPhi = 0;
		}
		if(Dir == 1){ //user defined
			mainTheta = mainTheta2*0.0174532925f;
			mainPhi = mainPhi2*0.0174532925f;
		}
		if(Spin == 0){
			AnaVec.data[0] = new Complex(Mathf.Cos (mainTheta/2f),0);
			AnaVec.data[1] = new Complex(Mathf.Cos (mainPhi)*Mathf.Sin (mainTheta/2f),Mathf.Sin (mainPhi)*Mathf.Sin(mainTheta/2f));
			AnaVec.data[2] = new Complex(0,0);
		}
		if(Spin == 1){
			AnaVec.data[0] = new Complex((1f/2f)*Mathf.Cos (mainPhi)+(1f/2f)*Mathf.Cos (mainTheta)*Mathf.Cos(mainPhi),-((1f/2f)*Mathf.Sin (mainPhi)+(1f/2f)*Mathf.Sin(mainPhi)*Mathf.Cos(mainTheta)));
			AnaVec.data[1] = new Complex((1f/Mathf.Sqrt (2f))*Mathf.Sin (mainTheta),0);
			AnaVec.data[2] = new Complex((1f/2f)*(Mathf.Cos(mainPhi)-Mathf.Cos (mainPhi)*Mathf.Cos (mainTheta)),(1f/2f)*(Mathf.Sin (mainPhi)-Mathf.Sin (mainPhi)*Mathf.Cos(mainTheta)));
			AnaVecMid.data[0] = new Complex(-((Mathf.Sin (mainTheta)*Mathf.Cos(mainPhi))/Mathf.Sqrt (2f)),((Mathf.Sin(mainTheta)*Mathf.Sin(mainPhi))/Mathf.Sqrt(2f)));
			AnaVecMid.data[1] = new Complex(Mathf.Cos (mainTheta),0);
			AnaVecMid.data[2] = new Complex(((Mathf.Cos (mainPhi)*Mathf.Sin (mainTheta))/Mathf.Sqrt (2f)),((Mathf.Sin(mainPhi)*Mathf.Sin(mainTheta))/Mathf.Sqrt(2f)));
			AnaVecDown.data[0] = new Complex((Mathf.Cos (mainPhi)/2f)-((Mathf.Cos (mainPhi)*Mathf.Cos(mainTheta))/2f),-(Mathf.Sin (mainPhi)/2f)+((Mathf.Sin (mainPhi)*Mathf.Cos (mainTheta))/2f));
			AnaVecDown.data[1] = new Complex(-(Mathf.Sin (mainTheta)/Mathf.Sqrt(2f)),0);
			AnaVecDown.data[2] = new Complex((Mathf.Cos (mainPhi)/2f)+((Mathf.Cos (mainPhi)*Mathf.Cos(mainTheta))/2f),(Mathf.Sin (mainPhi)/2f)+((Mathf.Sin (mainPhi)*Mathf.Cos (mainTheta))/2f));

		}
	}
	
	
	Matrix testMag = new Matrix();
	public void MagnetMath(int Spin, int Dir){
		float theta = mainTheta;
		float phi = mainPhi;
		//State 0 = X, State 1 = Y, State 2 = Z, Nhat unknown atm
		if(Dir == 2){
			theta = Mathf.PI/2f;
			phi = 0f;
		}
		if(Dir == 3){
			theta = Mathf.PI/2f;
			phi = Mathf.PI/2f;
		}
		if(Dir == 0){
			theta = 0f;
			phi = 0f;
		}
		if(Dir == 1){ //user defined
			theta = mainTheta*0.0174532925f;
			phi = mainPhi*0.0174532925f;
		}
		
		if(Spin == 0){
			testMag.data[0][0] = new Complex(Mathf.Cos (theta),0);
			testMag.data[0][1] = new Complex(Mathf.Cos (phi)*Mathf.Sin (theta),-Mathf.Sin (phi)*Mathf.Sin(theta));
			testMag.data[1][0] = new Complex(Mathf.Cos (phi)*Mathf.Sin (theta),Mathf.Sin (phi)*Mathf.Sin(theta));
			testMag.data[1][1] = new Complex(-Mathf.Cos (theta),0);
		}
		if(Spin == 1){
			testMag.data[0][0] = new Complex(Mathf.Cos (theta),0);
			testMag.data[0][1] = new Complex(1/Mathf.Sqrt (2)*Mathf.Cos (phi)*Mathf.Sin (theta),-(1/Mathf.Sqrt(2)*Mathf.Sin(phi)*Mathf.Sin(theta)));
			testMag.data[0][2] = new Complex(0,0);
			testMag.data[1][0] = new Complex(1/Mathf.Sqrt (2)*Mathf.Cos (phi)*Mathf.Sin (theta),(1/Mathf.Sqrt(2)*Mathf.Sin(phi)*Mathf.Sin(theta)));
			testMag.data[1][1] = new Complex(0,0);
			testMag.data[1][2] = new Complex(1/Mathf.Sqrt (2)*Mathf.Cos (phi)*Mathf.Sin (theta),-(1/Mathf.Sqrt(2)*Mathf.Sin(phi)*Mathf.Sin(theta)));
			testMag.data[2][0] = new Complex(0,0);
			testMag.data[2][1] = new Complex(1/Mathf.Sqrt (2)*Mathf.Cos (phi)*Mathf.Sin (theta),(1/Mathf.Sqrt(2)*Mathf.Sin(phi)*Mathf.Sin(theta)));
			testMag.data[2][2] = new Complex(-Mathf.Cos (theta),0);
		}
	}
	
}
