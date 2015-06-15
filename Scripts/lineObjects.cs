using UnityEngine;
using System.Collections;

[System.Serializable]
public class lineObjects {

	public Transform Start;
	public Transform End;
	public int id;

	public lineObjects(Transform start,Transform end, int myID){
		Start = start;
		End   = end;
		id    = myID;
	}

	public lineObjects(){

	}
}
