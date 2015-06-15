using UnityEngine;
using System.Collections;

public class activeMenu : MonoBehaviour {

	public void Activate(GameObject Obj){
		if(Obj.activeSelf)
			Obj.SetActive(false);
		else
			Obj.SetActive(true);
	}

	public void Deactivate(GameObject Obj){
		if(Obj.activeSelf)
			Obj.SetActive(false);
	}
}
