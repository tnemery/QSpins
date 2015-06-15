using UnityEngine;
using System.Collections;

public class UpdateMagnetStrength : MonoBehaviour {
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	private Vector3 Offset = new Vector3(0,0,0);
	private Vector3 change = new Vector3(0,0,0);
	private Vector3 prevOffset = new Vector3(0,0,0);
	public Texture2D cursorTexture;
	public GameObject Reference;

	void OnMouseEnter(){
		Cursor.SetCursor(cursorTexture,hotSpot, cursorMode);
	}
	
	void OnMouseExit(){
		Cursor.SetCursor(null,Vector2.zero, cursorMode);
	}

	void OnMouseDown(){
		if(this.name == "Plus"){
			Reference.GetComponent<MagnetCounter>().Increment();
		}else if(this.name == "Minus"){
			Reference.GetComponent<MagnetCounter>().Decrement();
		}
	}

}
