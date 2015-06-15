using UnityEngine;
using System.Collections;

public class Pathing : MonoBehaviour {
	private MoveObject pathScript;
	private GameObject Onstage;
	private string curObj = "gun";
	private string prevObj = "";
	private int returnPath = 0; //0 is up, 1 is down, 2 is mid
	private int Spin = 0;
	public int moves = 0;
	private bool turnOffCount = false;
	public int State = 0;
	private bool enableWatch = false;
	private float watchTime = 0.01f;
	private bool changed = false;
	private int tempCount = 0;

	public void Watch(){
		enableWatch = !enableWatch;
	}

	public void AddWatchTime(string time){
		watchTime = (float)int.Parse(time)/1000f;
	}

	public void SomethingChanged(){
		changed = true;
	}

	// Use this for initialization
	void Awake () {
		pathScript = Camera.main.GetComponent<MoveObject>();
		Onstage = GameObject.Find ("OnStage").gameObject;
	}

	public void ChangeState(int st){
		State = st;
	}
	
	public void TurnOffCounter(bool onoff){
		turnOffCount = onoff;
	}

	public void Run () {
		if(curObj == "" || curObj == null){
			curObj = "gun";
		}
		prevObj = curObj;
		//print ("curObj = "+(curObj == null));
		switch(curObj.Substring(0,3)){
			case "Ana": //send direction up,down,mid(spin 1) instead of finding direction
				if(enableWatch == true){
					if(returnPath == 0){
						GameObject.Find (curObj).transform.GetChild(Spin).transform.GetChild (4).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
					}
					if(returnPath == 1){
						GameObject.Find (curObj).transform.GetChild(Spin).transform.GetChild (5).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
					}
					if(returnPath == 2){
						GameObject.Find (curObj).transform.GetChild(Spin).transform.GetChild (6).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
					}
					StartCoroutine(Wait(curObj,returnPath));
				}
				curObj = Onstage.GetComponent<BuildPath>().GetConnections(curObj)[returnPath];
			//print ("prev ana in path: "+curObj);
				break;
			case "Cou":
				if(turnOffCount == false){
					GameObject.Find(curObj).transform.GetChild(2).GetComponent<TextMesh>().text = (int.Parse(GameObject.Find(curObj).transform.GetChild(2).GetComponent<TextMesh>().text)+1).ToString ();
					GameObject.Find (curObj).GetComponent<CounterData>().AddCount ();
				}else if(turnOffCount == true)	
					GameObject.Find (curObj).GetComponent<CounterData>().AddCount ();
				Reset();
				break;
			case "nul":
				Reset();
				break;
			case "Sta":
				Reset();
				break;
		default: //goes here on the gun and Mag
				//curObj = pathScript.createPaths(curObj,returnPath);
				
				curObj = Onstage.GetComponent<BuildPath>().GetConnections(curObj)[0];

			//print ("in path: "+curObj);
				break;
		}
		//send this data to math
		if(curObj != "" && curObj != null && curObj != "Null"){
			if(moves < 5 || State == 5 || changed == true){
				if(changed){
					if(tempCount < 5){
						returnPath = this.GetComponent<AllMathData>().DoMath(prevObj,curObj,Spin);
						tempCount++;
					}else{
						changed = false;
						tempCount = 0;
					}
				}else{
					returnPath = this.GetComponent<AllMathData>().DoMath(prevObj,curObj,Spin);
				}
			}else{
				if(Spin == 0){
					returnPath = this.GetComponent<AllMathData>().SpinHalfCycle(curObj);
				}
				if(Spin == 1)
					returnPath = this.GetComponent<AllMathData>().SpinOneCycle(curObj);
			}
		}else{
			Reset();
		}
	}

	public void ChangeSpin(int curSpin){
		Spin = curSpin;
	}

	public void resetMoves(){
		moves = 0;
		curObj = "gun";
		returnPath = 0;
	}

	public void Reset(){
		curObj = "gun";
		returnPath = 0;
		moves++;
		if(State == 5){
			this.GetComponent<AllMathData>().GunMath(Spin);
		}
	}

	IEnumerator Wait(string obj, int spot){
		yield return new WaitForSeconds(watchTime);
		if(spot == 0){
			GameObject.Find (obj).transform.GetChild(Spin).transform.GetChild (4).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
		}
		if(spot == 1){
			GameObject.Find (obj).transform.GetChild(Spin).transform.GetChild (5).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
		}
		if(spot == 2){
			GameObject.Find (obj).transform.GetChild(Spin).transform.GetChild (6).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

}
