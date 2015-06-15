using UnityEngine;
using System.Collections;

public class LineStart : MonoBehaviour {
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	public Texture2D cursorTexture;
	public Transform start;

	private int myID = 1;
	private int LineID1 = 1;
	private int LineID2 = 1;
	private int LineID3 = 1;
	private bool used = false; 
	private GameObject AllObjs;
	private GameObject AllLines;

	void Start(){
		AllObjs = GameObject.Find ("OnStage").gameObject;
		AllLines = GameObject.Find ("Lines").gameObject;
	}


	void OnMouseEnter(){
		Cursor.SetCursor(cursorTexture,hotSpot, cursorMode);
	}
	
	void OnMouseExit(){
		Cursor.SetCursor(null,Vector2.zero, cursorMode);
	}

	void OnMouseDown(){
		string prev = "";
		if(start.name.Substring(0,3) == "gun" || start.name.Substring(0,3) == "Mag"){
			prev = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
			AllObjs.GetComponent<BuildPath>().UpdateConTop(start.name,"");
			if(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0) != -1){
				if(prev != ""){
					AllObjs.GetComponent<BuildPath>().DecreaseInput(prev);
					if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(prev) == 0){
						AllObjs.GetComponent<BuildPath>().SetParent(prev,"");
					}
				}
				Camera.main.GetComponent<MoveObject>().ClearLine(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0));
			}
		}else if(start.name.Substring(0,3) == "Ana"){
			if(this.name == "LineTop"){
				prev = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
				AllObjs.GetComponent<BuildPath>().UpdateConTop(start.name,"");
				if(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0) != -1){
					if(prev != ""){
						AllObjs.GetComponent<BuildPath>().DecreaseInput(prev);
						if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(prev) == 0){
							AllObjs.GetComponent<BuildPath>().SetParent(prev,"");
						}
					}
					Camera.main.GetComponent<MoveObject>().ClearLine(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0));
				}
			}else if(this.name == "LineMid"){
				prev = AllObjs.GetComponent<BuildPath>().GetConMid(start.name);
				AllObjs.GetComponent<BuildPath>().UpdateConMid(start.name,"");
				if(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,1) != -1){
					if(prev != ""){
						AllObjs.GetComponent<BuildPath>().DecreaseInput(prev);
						if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(prev) == 0){
							AllObjs.GetComponent<BuildPath>().SetParent(prev,"");
						}
					}
					Camera.main.GetComponent<MoveObject>().ClearLine(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,1));
				}
			}else if(this.name == "LineBot"){
				prev = AllObjs.GetComponent<BuildPath>().GetConBot(start.name);
				AllObjs.GetComponent<BuildPath>().UpdateConBot(start.name,"");
				if(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,2) != -1){
					if(prev != ""){
						AllObjs.GetComponent<BuildPath>().DecreaseInput(prev);
						if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(prev) == 0){
							AllObjs.GetComponent<BuildPath>().SetParent(prev,"");
						}
					}
					Camera.main.GetComponent<MoveObject>().ClearLine(AllObjs.GetComponent<BuildPath>().GetLineID(start.name,2));
				}
			}
		}
		Camera.main.GetComponent<LineTest>().GetObj(this.transform);
		Camera.main.GetComponent<LineTest>().isDrawing = true;

	}

	void OnMouseUp(){
		Camera.main.GetComponent<LineTest>().isDrawing = false;
		int tempID = -1;
		string newCom = "";
		if(GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name == "Stage"){
			//do nothing
		}else if(start.name.Substring(0,3) == "gun" || start.name.Substring(0,3) == "Mag"){
			tempID = AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0);
			if(tempID != -1){
				if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
					Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
					newCom = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
					if(newCom != ""){
						AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
						if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
							AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
						}
					}
				}
			}else{
				if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
					tempID = Camera.main.GetComponent<LineTest>().makeLine();
					Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
					AllObjs.GetComponent<BuildPath>().SetLineID(start.name,0,tempID);
					newCom = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
					if(newCom != ""){
						AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
						if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
							AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
						}
					}
				}
			}
		}else if(start.name.Substring(0,3) == "Ana"){
			if(this.name == "LineTop"){
				tempID = AllObjs.GetComponent<BuildPath>().GetLineID(start.name,0);
				if(tempID != -1){
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}else{
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						tempID = Camera.main.GetComponent<LineTest>().makeLine();
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						AllObjs.GetComponent<BuildPath>().SetLineID(start.name,0,tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConTop(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}
			}else if(this.name == "LineMid"){
				tempID = AllObjs.GetComponent<BuildPath>().GetLineID(start.name,1);
				if(tempID != -1){
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConMid(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}else{
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						tempID = Camera.main.GetComponent<LineTest>().makeLine();
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						AllObjs.GetComponent<BuildPath>().SetLineID(start.name,1,tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConMid(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}
			}else if(this.name == "LineBot"){
				tempID = AllObjs.GetComponent<BuildPath>().GetLineID(start.name,2);
				if(tempID != -1){
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConBot(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}else{
					if(checkValidLine(start.name,GameObject.Find ("Pool").GetComponent<ObjPool>().Send().name)){
						tempID = Camera.main.GetComponent<LineTest>().makeLine();
						Camera.main.GetComponent<LineTest>().makeLine(start.name,this.name, tempID);
						AllObjs.GetComponent<BuildPath>().SetLineID(start.name,2,tempID);
						newCom = AllObjs.GetComponent<BuildPath>().GetConBot(start.name);
						if(newCom != ""){
							AllObjs.GetComponent<BuildPath>().IncreaseInput(newCom);
							if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(newCom) == 1){
								AllObjs.GetComponent<BuildPath>().SetParent(newCom,start.name);
							}
						}
					}
				}
			}
		}
	}

	public bool checkValidLine(string s, string e){
		print (s+" "+e);
		if(AllObjs.GetComponent<BuildPath>().GetInputAmnt(e) != 0){
			if(AllObjs.GetComponent<BuildPath>().GetParent(e) == s){
				return true;
			}else{
				return false;
			}
		}else{
			return true;
		}
		return false;
	}



	public void SetID(int num){
		myID = num;
		used = true;
	}

	public void changeID(){
		myID = 1;
	}

	public void setUsed(bool status){
		used = status;
	}

	/*
	public bool GetStatus(){
		return used;
	} */
}
