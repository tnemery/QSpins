using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class MoveObject : MonoBehaviour {
	private Transform[] pathObjs = new Transform[10]; // might be excessive but will start with 10 possible paths
	private string trail = "";
	static private int testID = 0;
	public GameObject allLines;
	public GameObject MathData;
	private LineStart[] allLineStartScripts;
	private GameObject Onstage;
	
	
	void Awake(){
		Onstage = GameObject.Find ("OnStage");
	}

	public void ResetData(){
		testID = 0;
		Onstage.GetComponent<BuildPath>().ResetLineIDs();
	}


	public int SetLine(Transform s,Transform e){
		if(e != null){
			if(Onstage.GetComponent<BuildPath>().GetParent(e.name) == "" ||
			   Onstage.GetComponent<BuildPath>().GetParent(e.name) == s.parent.name ||
			   Onstage.GetComponent<BuildPath>().GetParent(e.name) == s.parent.parent.name){
				if(s.parent.name.Substring(0,3) == "gun")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.name);
				else if(s.parent.name.Substring(0,3) == "Mag")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.name);
				else if(s.parent.parent.name.Substring(0,3) == "Ana")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.parent.name);
				s.position = new Vector3(s.position.x,s.position.y,0.029f);
				allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(0,s.position);
				allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(1,e.position);
				//allLines.transform.GetChild (testID).transform.GetComponent<LineConnecter>().ChangeObjects(s,e);
				Onstage.GetComponent<BuildPath>().SetID(e.name,testID);
				testID++;
				return (testID-1);
			}else{
				allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
				allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
				testID++;
				return (testID-1);
			}
		}else{
			allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
			allLines.transform.GetChild (testID).transform.GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
			testID++;
			return (testID-1);
		}
		return 1;
	}

	public void SetLine(Transform s,Transform e, int id){
		if(e != null){
			if(Onstage.GetComponent<BuildPath>().GetParent(e.name) == "" ||
			   Onstage.GetComponent<BuildPath>().GetParent(e.name) == s.parent.name ||
			   Onstage.GetComponent<BuildPath>().GetParent(e.name) == s.parent.parent.name){
				if(s.parent.name.Substring(0,3) == "gun")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.name);
				else if(s.parent.name.Substring(0,3) == "Mag")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.name);
				else if(s.parent.parent.name.Substring(0,3) == "Ana")
					Onstage.GetComponent<BuildPath>().SetParent(e.name,s.parent.parent.name);
				s.position = new Vector3(s.position.x,s.position.y,0.029f);
				allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(0,s.position);
				allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(1,e.position);
				//allLines.transform.GetChild (id).transform.GetComponent<LineConnecter>().ChangeObjects(s,e);
				//Onstage.GetComponent<BuildPath>().SetID(e.name,id);
			}else{
				allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
				allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
				Onstage.GetComponent<BuildPath>().SetParent(Onstage.GetComponent<BuildPath>().GetPartByID (id),"");
			}
		}else{
			allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
			allLines.transform.GetChild (id).transform.GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
			Onstage.GetComponent<BuildPath>().SetParent(Onstage.GetComponent<BuildPath>().GetPartByID (id),"");
		}
	}

	public void ClearLine(int id){
		if(id < allLines.transform.childCount){
			allLines.transform.GetChild(id).GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
			allLines.transform.GetChild(id).GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
			//Onstage.GetComponent<BuildPath>().SetParent(Onstage.GetComponent<BuildPath>().GetPartByID (id),"");
			//Onstage.GetComponent<BuildPath>().UpdateProb(Onstage.GetComponent<BuildPath>().GetPartByID (id),0,0,0); //reset prob of choosing this to 0
			//allLines.transform.GetChild(id).GetComponent<LineConnecter>().ChangeObjects(allLines.transform.GetChild(id).GetComponent<LineConnecter>().getStart(),null);
		}
	}


	public void clearAllLines(){
		allLineStartScripts = (LineStart[])GameObject.FindObjectsOfType<LineStart>();
		for(int i = 0;i<allLines.transform.childCount;i++){
			allLines.transform.GetChild(i).GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
			allLines.transform.GetChild(i).GetComponent<LineRenderer>().SetPosition(1,Vector3.zero);
			Onstage.GetComponent<BuildPath>().SetParent(Onstage.GetComponent<BuildPath>().GetPartByID (i),"");
			Onstage.GetComponent<BuildPath>().UpdateProb(Onstage.GetComponent<BuildPath>().GetPartByID (i),0,0,0); //reset prob of choosing this to 0
		}
	}


	public void redrawLines(){
		string myStart;
		string myEnd;
		for(int i = 0;i<allLines.transform.childCount;i++){
			if(i < testID){
				myStart = Onstage.GetComponent<BuildPath>().GetStartByLineID(i);
				myEnd = Onstage.GetComponent<BuildPath>().GetEndLine(myStart,i);
				//print (myStart+" "+myEnd);
				if(myEnd != null && myEnd != "" && myEnd != "none"){
					Transform s = GameObject.Find (myStart).transform;
					if(myStart.Substring(0,3) == "gun"){
						s = GameObject.Find (myStart).transform.GetChild(0).transform;
					}
					if(myStart.Substring(0,3) == "Mag"){
						s = GameObject.Find (myStart).transform.GetChild(5).transform;
					}
					if(myStart.Substring(0,3) == "Ana"){
						if(MathData.GetComponent<AllMathData>().GetSpinState() == 0){
							if(Onstage.GetComponent<BuildPath>().GetLinePath(myStart,i) == 0){
								s = GameObject.Find (myStart).transform.GetChild(0).transform.GetChild(4).transform;
							}
							if(Onstage.GetComponent<BuildPath>().GetLinePath(myStart,i) == 2){
								s = GameObject.Find (myStart).transform.GetChild(0).transform.GetChild(5).transform;
							}
						}
						if(MathData.GetComponent<AllMathData>().GetSpinState() == 1){
							if(Onstage.GetComponent<BuildPath>().GetLinePath(myStart,i) == 0){
								s = GameObject.Find (myStart).transform.GetChild(1).transform.GetChild(4).transform;
							}
							if(Onstage.GetComponent<BuildPath>().GetLinePath(myStart,i) == 1){
								s = GameObject.Find (myStart).transform.GetChild(1).transform.GetChild(6).transform;
							}
							if(Onstage.GetComponent<BuildPath>().GetLinePath(myStart,i) == 2){
								s = GameObject.Find (myStart).transform.GetChild(1).transform.GetChild(5).transform;
							}
						}
					}
					Transform e = GameObject.Find (myEnd).transform;
					allLines.transform.GetChild (i).transform.GetComponent<LineRenderer>().SetPosition(0,s.position);
					allLines.transform.GetChild (i).transform.GetComponent<LineRenderer>().SetPosition(1,e.position);
				}
			}
		}
	}
}
