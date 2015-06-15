using UnityEngine;
using System.Collections;

public class MagnetAxisChange : MonoBehaviour {
	public GameObject[] MagnetAxis;
	private int curAxis = 0;
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	public Texture2D cursorTexture;
	private GameObject Onstage;
	
	
	void Awake(){
		Onstage = GameObject.Find ("OnStage");
	}


	void OnMouseEnter(){
		Cursor.SetCursor(cursorTexture,hotSpot, cursorMode);
	}
	
	void OnMouseExit(){
		Cursor.SetCursor(null,Vector2.zero, cursorMode);
	}
	
	void OnMouseDown(){
		MagnetAxis[curAxis].SetActive(false);
		curAxis++;
		if(curAxis == 4)
			curAxis = 0;

		if(curAxis == 1){
			this.SendMessageUpwards("ShowText",true);
		}else{
			this.SendMessageUpwards("ShowText",false);
		}
		MagnetAxis[curAxis].SetActive(true);
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
		GameObject.Find ("GUI").GetComponent<Pathing>().SomethingChanged();
	}


	public string GetAxis(){
		return MagnetAxis[curAxis].name;
	}

	public void ChangeAxis(string Axis){
		MagnetAxis[0].SetActive(false);
		MagnetAxis[1].SetActive(false);
		MagnetAxis[2].SetActive(false);
		MagnetAxis[3].SetActive(false);
		if(Axis == "N"){
			MagnetAxis[1].SetActive(true);
			curAxis = 1;
			this.SendMessageUpwards("ShowText",true);
		}
		if(Axis == "X"){
			MagnetAxis[2].SetActive(true);
			this.SendMessageUpwards("ShowText",false);
			curAxis = 2;
		}
		if(Axis == "Y"){
			MagnetAxis[3].SetActive(true);
			this.SendMessageUpwards("ShowText",false);
			curAxis = 3;
		}
		if(Axis == "Z"){
			MagnetAxis[0].SetActive(true);
			curAxis = 0;
			this.SendMessageUpwards("ShowText",false);
		}
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
	}

	public void Default(){
		MagnetAxis[1].SetActive(false);
		MagnetAxis[2].SetActive(false);
		MagnetAxis[3].SetActive(false);
		MagnetAxis[0].SetActive(true);
		curAxis = 0;
		this.SendMessageUpwards("ShowText",false);
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
	}

}
