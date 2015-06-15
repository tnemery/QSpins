using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildLine : MonoBehaviour {
	public List<lineObjects> myLines = new List<lineObjects>();

	public void AddLine(Transform s,Transform e, int ID){
		myLines.Add (new lineObjects(s,e,ID));
	}

	public int GetLineID(string s,string e){
		return 1;
	}

	public string GetLineEnd(string s, int ID){
		return "";
	}


	public void UpdateLineStart(Transform s, int ID){
		for(int i=0; i<myLines.Count; i++){
			if(myLines[i].id == ID){
				myLines[i].Start = s;
				myLines[i].End = null;
			}
		}
	}

	public void UpdateLineEnd(Transform e, int ID){
		for(int i=0; i<myLines.Count; i++){
			if(myLines[i].id == ID){
				myLines[i].End = e;
			}
		}
	}
}
