using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInput : MonoBehaviour {

	private float UserA,UserB,UserC,UserD,UserE,UserF = 0.0f;
	private int Spin,Theta = 90,Phi = 45;
	private string Dir = "Z";
	private Transform StageComps;

	void Awake(){
		StageComps = GameObject.Find ("OnStage").transform;
	}


	public void changeTheta(string theta){
		Theta = int.Parse(theta);
		if(Theta > 180){
			Theta = 180;
			GameObject.Find ("Theta").GetComponent<InputField>().text = "180";
		}
		if(Theta < 0){
			Theta = 0;
			GameObject.Find ("Theta").GetComponent<InputField>().text = "0";
		}
	}

	public void changePhi(string phi){
		Phi = int.Parse (phi);
		if(Phi > 360){
			Phi = 360;
			GameObject.Find ("Phi").GetComponent<InputField>().text = "360";
		}
		if(Phi < 0){
			Phi = 0;
			GameObject.Find ("Phi").GetComponent<InputField>().text = "0";
		}
	}

	public void changeA(string A){
		UserA = float.Parse(A);
	}

	public void changeB(string B){
		UserB = float.Parse(B);
	}

	public void changeC(string C){
		UserC = float.Parse(C);
	}

	public void changeD(string D){
		UserD = float.Parse(D);
	}

	public void changeE(string E){
		UserE = float.Parse(E);
	}

	public void changeF(string F){
		UserF = float.Parse(F);
	}

	public void ZDir(bool val){
		if(val)
			Dir = "Z";
	}

	public void XDir(bool val){
		if(val)
			Dir = "X";
	}

	public void YDir(bool val){
		if(val)
			Dir = "Y";
	}


	public void SetSpin(){
		Spin = GameObject.Find ("GUI").transform.GetComponent<MainGUI>().getSpin();
	}

	public void sendAngles(){
		GameObject.Find ("GUI").transform.GetComponent<AllMathData>().SetDegrees(Phi,Theta);
		foreach(Transform go in StageComps){
			if(go.name.Substring(0,3) == "Mag"){
				go.transform.GetComponent<MagnetData>().UpdateAngles(Phi.ToString(),Theta.ToString());
			}else if(go.name.Substring(0,3) == "Ana"){
				go.transform.GetComponent<AnalyzerData>().UpdateAngles(Phi.ToString(),Theta.ToString());
			}
		}
	}

	public void sendData(){
		if(Spin == 0){
			GameObject.Find ("GUI").transform.GetComponent<AllMathData>().SetUserVals(UserA,UserB,UserC,UserD,Dir);
		}else if(Spin == 1){
			GameObject.Find ("GUI").transform.GetComponent<AllMathData>().SetUserVals(UserA,UserB,UserC,UserD,UserE,UserF,Dir);
		}
	}
}
