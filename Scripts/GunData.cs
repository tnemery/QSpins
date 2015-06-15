using UnityEngine;
using System.Collections;

public class GunData : MonoBehaviour {

	public Transform nextComponent = null;
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	private Vector3 Offset = new Vector3(0,0,0);
	private Vector3 change = new Vector3(0,0,0);
	private Vector3 prevOffset = new Vector3(0,0,0);
	public Texture2D cursorTexture;
	private bool dragging = false;
	public GameObject move;
	
	
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
	
	void Update(){
		if(dragging){
			//print ("Gun is moving");
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
