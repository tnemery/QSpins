using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LineConnecter : MonoBehaviour {
	static public Transform startObj;
	static public Transform endObj;
	static public Transform prevObj;
	private GameObject Onstage;
	private string[] cons = new string[3];

	void Awake(){
		Onstage = GameObject.Find ("OnStage");
	}

	public void ChangeObjects(Transform s, Transform e){
		//startObj = s;
		//prevObj = endObj;
		//endObj = e;
		//cons = new string[3];
		//print (s+" "+e);
		/*
		if(s.parent.name.Substring(0,3) == "gun"){
			if(e != null){
				Onstage.GetComponent<BuildPath>().UpdateConTop("gun",e.name);
			}else{
				Onstage.GetComponent<BuildPath>().UpdateConTop("gun","");
			}
		}
		if(s.parent.name.Substring(0,3) == "Mag"){
			if(e != null){
				Onstage.GetComponent<BuildPath>().UpdateConTop(s.parent.name,e.name);
			}else{
				Onstage.GetComponent<BuildPath>().UpdateConTop(s.parent.name,"");
			}
		}

		if(s.parent.parent.name.Substring(0,3) == "Ana"){
			cons = Onstage.GetComponent<BuildPath>().GetConnections(s.parent.parent.name);
			if(e == null){
				if(prevObj == null){
					if(cons[0] == null){
						cons[0] = "";
					}else if(cons[1] == null){
						cons[1] = "";
					}else{
						cons[2] = "";
					}
				}else if(cons[0] == prevObj.name){
					cons[0] = "";
				}else if(cons[1] == prevObj.name){
					cons[1] = "";
				}else{
					cons[2] = "";
				}
			}else{
				if(cons[0] == "" || cons[0] == null){
					cons[0] = e.name;
				}else if(cons[1] == "" || cons[1] == null){
					cons[1] = e.name;
				}else{
					cons[2] = e.name;
				}
			}
			//Onstage.GetComponent<BuildPath>().UpdateConnections(s.parent.parent.name,cons);
		} */
	}

	public Transform getStart(){
		return startObj;
	}

	public Transform getEnd(){
		return endObj;
	}
}
