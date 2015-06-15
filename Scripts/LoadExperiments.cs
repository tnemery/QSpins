using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  
using UnityEngine.UI;
using System.Linq;

public class LoadExperiments : MonoBehaviour {
	public List<string> fileLines;
	private string[,] SpecificLine;
	//static private string loadedProg = "";
	private string Spin;
	public TextAsset check2;
	private GameObject onStage;
	public GameObject GunText;


	void Awake(){
		onStage = GameObject.Find ("OnStage").gameObject;
	}

	public void DebugFile(string[] myfile){
		for(int i = 0; i < myfile.Length; i++){
			print (myfile[i]);
		}
	}

	public void LoadExperiment(string fileName){
		this.GetComponent<MainGUI>().Clear();
		this.GetComponent<MainGUI>().DefaultSettings();
		this.GetComponent<Pathing>().SomethingChanged();
		Camera.main.GetComponent<MoveObject>().ResetData();
		TextAsset check = Resources.Load(fileName) as TextAsset;
		string[] breakup = check.text.Split('\n');
		//DebugFile(breakup);
		int i = breakup.Length-1;
		SpecificLine = new string[i,3];
		for(i = 0; i<breakup.Length-1;i++){
			if(fileName == "Default" || fileName == "McIntyreTE1" ||
			   fileName == "L1P1" || fileName == "L2P1"){
				string[] temp = breakup[i].Split(' ');
				SpecificLine[i,0] = temp[0];
				SpecificLine[i,1] = temp[1];
				SpecificLine[i,2] = temp[2].Substring(0,temp[2].Length-1);
			}else{
				string[] temp = breakup[i].Split(' ');
				SpecificLine[i,0] = temp[0];
				SpecificLine[i,1] = temp[1];
				SpecificLine[i,2] = temp[2].Substring(0,temp[2].Length);
			}
		}

		for(i = 0;i<breakup.Length-1;i++){
			if(SpecificLine[i,0] == "Initialize"){
				//print ("init "+SpecificLine[i,2]);
				this.GetComponent<AllMathData>().SetState(int.Parse(SpecificLine[i,2]));
				this.GetComponent<Pathing>().ChangeState(int.Parse(SpecificLine[i,2]));
				if(int.Parse(SpecificLine[i,2]) == 4){
					GunText.GetComponent<TextMesh>().text = "U";
				}else if(int.Parse(SpecificLine[i,2]) == 5){
					GunText.GetComponent<TextMesh>().text = "R";
				}else{
					GunText.GetComponent<TextMesh>().text = (int.Parse(SpecificLine[i,2])+1).ToString();
				}
			}
			if(SpecificLine[i,0] == "Spin"){
				Spin = SpecificLine[i,2];
				this.GetComponent<MainGUI>().SetSpin(Spin);
			}
			if(SpecificLine[i,0] == "Object"){
				//print ("Obj "+SpecificLine[i,2]);
				Objects (SpecificLine[i,2],ConvertToV3(SpecificLine[i+1,2]),SpecificLine[i+2,2]);
			}
		}
		for(i = 0;i<breakup.Length-1;i++){
			if(SpecificLine[i,0] == "Attach"){
				string[] getObjs = SpecificLine[i,2].Split(',');
				//print ("Attaching: "+getObjs[0]+" "+getObjs[2]+" "+getObjs[1]);
				Attach(getObjs[0],getObjs[2],getObjs[1]);
			}
		}
		Camera.main.GetComponent<MoveObject>().redrawLines();
	}

	public Vector3 ConvertToV3(string Obj){
		Vector3 newV3;
		float p1,p2,p3;
		string newStr = Obj.Substring(1,Obj.Length-2);
		string[] splStr = newStr.Split(',');
		p1 = float.Parse(splStr[0]);
		p2 = float.Parse(splStr[1]);
		p3 = float.Parse(splStr[2]);
		newV3 = new Vector3(p1,p2,p3);
		return newV3;
	}


	public void Objects(string Obj, Vector3 loc, string Axis){
		if(Obj.Substring(0,3) == "gun"){
			this.GetComponent<MainGUI>().AddGun(loc);
		}
		if(Obj.Substring(0,3) == "Ana"){
			this.GetComponent<MainGUI>().AddAnalyzer(Obj,loc,Axis);
			onStage.GetComponent<BuildPath>().UpdateDirection(Obj,GetAxisNum(Axis));
		}
		if(Obj.Substring(0,3) == "Mag"){
			this.GetComponent<MainGUI>().AddMagnet(Obj,loc,Axis);
			onStage.GetComponent<BuildPath>().UpdateDirection(Obj,GetAxisNum(Axis));
		}
		if(Obj.Substring(0,3) == "Cou"){
			this.GetComponent<MainGUI>().AddCounter(Obj,loc);
		}
	}

	public int GetAxisNum(string Ax){
		if(Ax == "Z"){
			return 0;
		}else if(Ax == "X"){
			return 2;
		}else if(Ax == "Y"){
			return 1;
		}else{
			return 3;
		}
	}


	public void Attach(string input, string output,string spot){
		Transform End   = GameObject.Find (output).transform;

		if(Spin == "Half"){
			if(input == "gun"){
				Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(0).transform,End,"top",input);
				onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
				onStage.GetComponent<BuildPath>().IncreaseInput(output);
			}
			if(input.Substring(0,3) == "Ana"){
				if(spot == "top"){
					Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(0).transform.GetChild (4).transform,End,spot,input);
					onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
					onStage.GetComponent<BuildPath>().IncreaseInput(output);
				}
				if(spot == "bot"){
					Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(0).transform.GetChild (5).transform,End,spot,input);
					onStage.GetComponent<BuildPath>().UpdateConBot(input,output);
					onStage.GetComponent<BuildPath>().IncreaseInput(output);
				}
			}
			if(input.Substring(0,3) == "Mag"){
				Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(5).transform,End,"top",input);
				onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
				onStage.GetComponent<BuildPath>().IncreaseInput(output);
			}
		}else if(Spin == "One"){
			if(input == "gun"){
				Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(0).transform,End,"top",input);
				onStage.GetComponent<BuildPath>().IncreaseInput(output);
				onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
				onStage.GetComponent<BuildPath>().IncreaseInput(output);
			}
			if(input.Substring(0,3) == "Ana"){
				if(spot == "top"){
					Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(1).transform.GetChild (4).transform,End,spot,input);
					onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
					onStage.GetComponent<BuildPath>().IncreaseInput(output);
				}
				if(spot == "bot"){
					Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(1).transform.GetChild (5).transform,End,spot,input);
					onStage.GetComponent<BuildPath>().UpdateConBot(input,output);
					onStage.GetComponent<BuildPath>().IncreaseInput(output);
				}
				if(spot == "mid"){
					Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(1).transform.GetChild (6).transform,End,spot,input);
					onStage.GetComponent<BuildPath>().UpdateConMid(input,output);
					onStage.GetComponent<BuildPath>().IncreaseInput(output);
				}
			}
			if(input.Substring(0,3) == "Mag"){
				Camera.main.GetComponent<LineTest>().SetSpecificLine(GameObject.Find (input).transform.GetChild(5).transform,End,"top",input);
				onStage.GetComponent<BuildPath>().UpdateConTop(input,output);
				onStage.GetComponent<BuildPath>().IncreaseInput(output);
			}
		}
	}
}
