using UnityEngine;
using System.Collections;

public class MagnetCounter : MonoBehaviour {
	private int Counter = 0;
	public TextMesh curText;
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
		Counter++;
		if(Counter == 100)
			Counter = 0;

		if(Counter < 10)
			curText.text = "0"+Counter.ToString();
		else
			curText.text = Counter.ToString();
		Onstage.GetComponent<BuildPath>().UpdateStrength(this.transform.parent.name,Counter);
		GameObject.Find ("GUI").GetComponent<Pathing>().SomethingChanged();
	}

	public int MagnetStrength(){
		return Counter;
	}

	public void Increment(){
		Counter++;
		if(Counter == 100)
			Counter = 0;
		
		if(Counter < 10)
			curText.text = "0"+Counter.ToString();
		else
			curText.text = Counter.ToString();
		Onstage.GetComponent<BuildPath>().UpdateStrength(this.transform.parent.name,Counter);
		GameObject.Find ("GUI").GetComponent<Pathing>().SomethingChanged();
	}

	public void Decrement(){
		Counter--;
		if(Counter == -1)
			Counter = 99;
		
		if(Counter < 10)
			curText.text = "0"+Counter.ToString();
		else
			curText.text = Counter.ToString();
		Onstage.GetComponent<BuildPath>().UpdateStrength(this.transform.parent.name,Counter);
		GameObject.Find ("GUI").GetComponent<Pathing>().SomethingChanged();
	}

	public void SetStrength(int Str){
		Counter = Str;
		if(Counter < 10)
			curText.text = "0"+Counter.ToString();
		else
			curText.text = Counter.ToString();
		Onstage.GetComponent<BuildPath>().UpdateStrength(this.transform.parent.name,Counter);
	}

	public void Default(){
		Counter = 0;
		curText.text = "00";
		Onstage.GetComponent<BuildPath>().UpdateStrength(this.transform.parent.name,Counter);
	}

}
