using UnityEngine;
using System.Collections;

public class CounterData : MonoBehaviour {
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	private Vector3 Offset = new Vector3(0,0,0);
	private Vector3 change = new Vector3(0,0,0);
	private Vector3 prevOffset = new Vector3(0,0,0);
	public Texture2D cursorTexture;
	private int Count = 0;
	private bool dragging = false;
	public GameObject move;
	private float startCounterFillSize = 0.0f;
	private int counterCap = 100;
	
	
	void OnMouseEnter(){
		Cursor.SetCursor(cursorTexture,hotSpot, cursorMode);
	}
	
	void OnMouseExit(){
		Cursor.SetCursor(null,Vector2.zero, cursorMode);
	}

	public void AddCount(){
		Count++;
		this.transform.GetChild (0).transform.localScale = new Vector3((float)((float)Count/(float)counterCap),1,1);
		if(this.transform.GetChild (0).transform.localScale.x >= 1){
			CapUpdate();
		}
	}

	public int GetCount(){
		return Count; 
	}

	public void ResetCount(){
		Count = 0;
		this.transform.GetChild (0).transform.localScale = new Vector3(startCounterFillSize,1,1);
		counterCap = 100;
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

	void Awake(){
		this.transform.GetChild (0).transform.localScale = new Vector3(startCounterFillSize,1,1);
	}

	void Update(){
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

		if(Count >= counterCap){
			this.transform.GetChild (0).transform.localScale = new Vector3((float)((float)Count/(float)counterCap),1,1);
			if(this.transform.GetChild (0).transform.localScale.x >= 1){
				CapUpdate();
			}
		}


	}

	private void CapUpdate(){
		counterCap *= 2;
		this.transform.GetChild (0).transform.localScale = new Vector3((float)((float)Count/(float)counterCap),1,1);
	
		foreach(Transform myChild in GameObject.Find ("OnStage").transform){
			if(myChild.name.Substring(0,3) == "Cou"){
				myChild.GetComponent<CounterData>().UpdateAllCounters(counterCap);
			}
		}
	}

	public void UpdateAllCounters(int newCap){
		if(counterCap < newCap){
			this.counterCap = newCap;
			this.transform.GetChild (0).transform.localScale = new Vector3((float)((float)this.Count/(float)this.counterCap),1,1);
	
		}
	}

}
