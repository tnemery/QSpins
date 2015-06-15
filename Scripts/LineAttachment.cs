using UnityEngine;
using System.Collections;

public class LineAttachment : MonoBehaviour {

	void OnMouseOver(){
		GameObject.Find ("Pool").GetComponent<ObjPool>().Recieve(this.transform);
	}
	/*
	void OnMouseOut(){
		GameObject.Find ("Pool").GetComponent<ObjPool>().Recieve(null);
	}
*/
}
