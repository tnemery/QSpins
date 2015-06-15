using UnityEngine;
using System.Collections;

public class AnalyzerAxisChange : MonoBehaviour {
	public GameObject[] AxisSpin1;
	public GameObject[] AxisSpinH;
	public Texture2D cursorTexture;
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(16,16);
	private int curAxis = 0;
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
		AxisSpin1[curAxis].SetActive(false);
		AxisSpinH[curAxis].SetActive(false);
		curAxis++;
		if(curAxis == 4)
			curAxis = 0;

		if(curAxis == 1){
			this.SendMessageUpwards("ShowText",true);
		}else{
			this.SendMessageUpwards("ShowText",false);
		}
		AxisSpin1[curAxis].SetActive(true);
		AxisSpinH[curAxis].SetActive(true);
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
		GameObject.Find ("GUI").GetComponent<Pathing>().SomethingChanged();
	}

	public void UpdateDirection(){
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
	}

	public string GetAxis(int spin){
		if(spin == 0)
			return AxisSpinH[curAxis].name;
		else
			return AxisSpin1[curAxis].name;
	}

	public void ChangeAxis(string Axis){
		AxisSpin1[0].SetActive(false);
		AxisSpinH[0].SetActive(false);
		AxisSpin1[1].SetActive(false);
		AxisSpinH[1].SetActive(false);
		AxisSpin1[2].SetActive(false);
		AxisSpinH[2].SetActive(false);
		AxisSpin1[3].SetActive(false);
		AxisSpinH[3].SetActive(false);
		if(Axis == "N"){
			AxisSpin1[1].SetActive(true);
			AxisSpinH[1].SetActive(true);
			curAxis = 1;
			this.SendMessageUpwards("ShowText",true);
		}
		if(Axis == "X"){
			AxisSpin1[2].SetActive(true);
			AxisSpinH[2].SetActive(true);
			curAxis = 2;
			this.SendMessageUpwards("ShowText",false);
		}
		if(Axis == "Y"){
			AxisSpin1[3].SetActive(true);
			AxisSpinH[3].SetActive(true);
			curAxis = 3;
			this.SendMessageUpwards("ShowText",false);
		}
		if(Axis == "Z"){
			AxisSpin1[0].SetActive(true);
			AxisSpinH[0].SetActive(true);
			curAxis = 0;
			this.SendMessageUpwards("ShowText",false);
		}
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
	}

	public void Default(){
		AxisSpin1[1].SetActive(false);
		AxisSpinH[1].SetActive(false);
		AxisSpin1[2].SetActive(false);
		AxisSpinH[2].SetActive(false);
		AxisSpin1[3].SetActive(false);
		AxisSpinH[3].SetActive(false);
		AxisSpin1[0].SetActive(true);
		AxisSpinH[0].SetActive(true);
		this.SendMessageUpwards("ShowText",false);
		curAxis = 0;
		Onstage.GetComponent<BuildPath>().UpdateDirection(this.transform.parent.name,curAxis);
	}

}
