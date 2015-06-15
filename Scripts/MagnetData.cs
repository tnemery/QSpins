using UnityEngine;
using System.Collections;

public class MagnetData : MonoBehaviour {

	public int Counter = 0;
	public string Axis = "";
	public Transform NextComponent = null;
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	private Vector3 Offset = new Vector3(0,0,0);
	private Vector3 change = new Vector3(0,0,0);
	private Vector3 prevOffset = new Vector3(0,0,0);
	public Texture2D cursorTexture;
	private bool dragging = false;
	public GameObject move;
	private bool SetN = false;

	public void UpdateAngles(string Phi, string Theta){
		this.transform.GetChild(8).transform.GetComponent<TextMesh>().text = "φ: "+Phi; //Phi
		this.transform.GetChild(9).transform.GetComponent<TextMesh>().text = "θ: "+Theta; //Theta
	}


	void OnMouseEnter(){
		Cursor.SetCursor(cursorTexture,hotSpot, cursorMode);
	}
	
	void OnMouseExit(){
		Cursor.SetCursor(null,Vector2.zero, cursorMode);
	}

	void OnMouseDown(){
		Vector3 curMouse = Input.mousePosition;
		Offset = (Camera.main.ScreenToWorldPoint(curMouse));
		prevOffset = Offset;
		dragging = true;
	}
	
	void OnMouseUp(){
		dragging = false;
	}

	public void ShowText(bool n){
		if(n){
			SetN = true;
		}else{
			SetN = false;
		}
	}

	void Update(){
		if(SetN){
			this.transform.GetChild(8).gameObject.SetActive(true);
			this.transform.GetChild(9).gameObject.SetActive(true);
		}else{
			this.transform.GetChild(8).gameObject.SetActive(false);
			this.transform.GetChild(9).gameObject.SetActive(false);
		}


		if(dragging){
			Vector3 curMouse = Input.mousePosition;
			curMouse.z = 2.0f;
			Offset = (Camera.main.ScreenToWorldPoint(curMouse));
			change = Offset-prevOffset;
			if(change.x != 0.0 || change.y != 0.0){
				transform.position += change;
			}
			prevOffset = Offset;
			move.GetComponent<MoveObject>().redrawLines();
		}
	}
}
