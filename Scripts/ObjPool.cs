using UnityEngine;
using System.Collections;

public class ObjPool : MonoBehaviour {
	private Transform curObj;

	public void Recieve(Transform Obj){
		curObj = Obj;
	}

	public Transform Send(){
		return curObj;
	}

	public void Clear(){
		curObj = null;
	}
}
