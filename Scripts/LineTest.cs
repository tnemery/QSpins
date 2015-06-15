using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineTest : MonoBehaviour {
	public bool isDrawing = false;
	
	private LineRenderer line;
	private List<Vector3> pointsList;
	private Vector3 mousePos;
	private MoveObject lineScript;
	private Transform other;
	public bool isConnected = false;
	
	private Transform sObj;
	private Transform eObj;
	private int retId;
	private GameObject myData;

	public void GetObj(Transform Obj){
		sObj = Obj;
	}
	
	public int makeLine(){
		eObj = GameObject.Find ("Pool").GetComponent<ObjPool>().Send();
		//GameObject.Find ("Pool").GetComponent<ObjPool>().Clear ();
		if(eObj.name != "Stage"  || sObj.name == eObj.name){
			retId = SetSpecificLine(sObj,eObj);
		}else{
			retId = SetSpecificLine(sObj,null);
		}

		return retId;
	}

	public void makeLine(string p,string n,int id){
		eObj = GameObject.Find ("Pool").GetComponent<ObjPool>().Send();
		//GameObject.Find ("Pool").GetComponent<ObjPool>().Clear ();
		if(n == "GunLine" || n == "MagLine"){
			myData.GetComponent<BuildPath>().UpdateConTop (p,eObj.name);
			myData.GetComponent<BuildPath>().SetLineID(p,0,id);
		}else if(n == "LineTop"){
			myData.GetComponent<BuildPath>().UpdateConTop (p,eObj.name);
			myData.GetComponent<BuildPath>().SetLineID(p,0,id);
		}else if(n == "LineMid"){
			myData.GetComponent<BuildPath>().UpdateConMid (p,eObj.name);
			myData.GetComponent<BuildPath>().SetLineID(p,1,id);
		}else if(n == "LineBot"){
			myData.GetComponent<BuildPath>().UpdateConBot (p,eObj.name);
			myData.GetComponent<BuildPath>().SetLineID(p,2,id);
		}



		if(eObj.name == "Stage" || sObj.name == eObj.name){
			lineScript.SetLine(sObj,null,id);
		}else{
			lineScript.SetLine(sObj,eObj,id);
		}

	}

	public int SetSpecificLine(Transform Start, Transform End){
		line.SetVertexCount(2);
		retId = lineScript.SetLine(Start,End);
		Start.transform.GetComponent<LineStart>().SetID(retId);
		return retId;
	}

	public void SetSpecificLine(Transform Start, Transform End,string type,string p){
		line.SetVertexCount(2);
		retId = lineScript.SetLine(Start,End);
		//print (p+ " hello "+retId);
		if(type == "top"){
			myData.GetComponent<BuildPath>().SetLineID(p,0,retId);
		}else if(type == "mid"){
			myData.GetComponent<BuildPath>().SetLineID(p,1,retId);
		}else{
			myData.GetComponent<BuildPath>().SetLineID(p,2,retId);
		}
		Start.transform.GetComponent<LineStart>().SetID(retId);
	}


	void Awake()
	{
		myData = GameObject.Find ("OnStage").gameObject;
		lineScript = GameObject.Find ("Main Camera").GetComponent<MoveObject>();
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer>();
		line.material =  new Material(Shader.Find("Particles/Multiply"));
		line.SetVertexCount(0);
		line.SetWidth(0.025f,0.025f);
		line.SetColors(Color.red, Color.red);
		line.useWorldSpace = true;
	}

	// Update is called once per frame
	void Update () {
		if(isDrawing){
			// If mouse button down, remove old line and set its color to green	
			//print ("moving");
			line.SetVertexCount(2);
			line.SetColors(Color.red, Color.red);
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z=0.0f;
			this.transform.GetComponent<LineRenderer>().SetPosition(0,sObj.position);
			this.transform.GetComponent<LineRenderer>().SetPosition(1,mousePos);

		}else if(!isDrawing){
			this.transform.GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
			this.transform.GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
		}
	}
}
