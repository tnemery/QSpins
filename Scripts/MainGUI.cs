using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MainGUI : MonoBehaviour {
	public GameObject OnStage;
	public GameObject Components;
	private int Spinner = 0;
	private MoveObject moveScript;
	private Transform current;
	//component counters
	private int ANACOUNTER = 1;
	private int MAGCOUNTER = 1;
	private int COCOUNTER = 1;
	private bool running = false;
	private int myCount = 0;
	private int counter = 0;

	private bool enableWatch = false;
	private float watchTime = 0.01f;
	private bool stop = false;

	private string[] cons = new string[3];
	void Awake(){
		moveScript = GameObject.Find ("Main Camera").transform.GetComponent<MoveObject>();
		this.GetComponent<AllMathData>().SetGunStates(0); //default spin half
	}

	void Start(){
		this.GetComponent<LoadExperiments>().LoadExperiment("Default");
	}

	public void stopping(){
		stop = true;
	}

	public int getSpin(){
		return Spinner;
	}

	public void changeSpin(int value){
		Spinner = value;
	}

	public void goStop(int value){
		if(value == 0){
			running = true;
			myCount = 0;
			stop = false;
		}else{
			running = false;
		}
	}

	public void AddWatchTime(string time){
		watchTime = (float)int.Parse (time)/1000f;
	}

	public void Watch(){
		enableWatch = !enableWatch;
	}

	public void Do(int value){
		myCount = value;
		running = true;
	}

	public void Clear(){
		GameObject.Find ("Main Camera").GetComponent<MoveObject>().clearAllLines();
		for(int a = 0; a < GameObject.Find("OnStage").transform.childCount;){
			string name = OnStage.transform.GetChild(a).name;
			GameObject.Find ("OnStage").transform.GetChild(a).parent = GameObject.Find ("Components").transform;
			GameObject.Find ("Components").transform.FindChild(name).transform.localPosition = new Vector3(0f,0f,0f);
		}
		OnStage.GetComponent<BuildPath>().ClearList();
	}

	public void watchUpdate(){
		if(stop == false){
			this.GetComponent<Pathing>().Run();
			StartCoroutine(Wait());
		}
	}

	
	void Update(){
		if(running){
			if(myCount == 0){
				if(enableWatch == true){
					running = false;
					watchUpdate ();
				}else{
					this.GetComponent<Pathing>().Run();
				}
			}
			else{
				do{
					this.GetComponent<Pathing>().Run();
					counter = this.GetComponent<Pathing>().moves;
				}while(counter < myCount);
				running = false;
				UpdateCounters();
			}
		}
	}

	private void UpdateCounters(){
		int comps = GameObject.Find ("OnStage").transform.childCount;
		for(int i = 0;i<comps;i++){
			if(GameObject.Find ("OnStage").transform.GetChild(i).name.Substring(0,3) == "Cou"){
				GameObject.Find ("OnStage").transform.GetChild(i).transform.GetChild(2).GetComponent<TextMesh>().text = GameObject.Find ("OnStage").transform.GetChild(i).GetComponent<CounterData>().GetCount ().ToString();
			}
		}
		this.GetComponent<Pathing>().TurnOffCounter(false);
		this.GetComponent<Pathing>().moves = 0;
		counter = 0;
	}
	
	public void MakeActiveOnStage(){
		for(int i = 0;i<OnStage.transform.childCount;i++){
			OnStage.transform.GetChild(i).transform.gameObject.SetActive(true);
			if(OnStage.transform.GetChild(i).transform.name.Substring(0,3) == "Ana"){
				if(Spinner == 1){
					OnStage.transform.GetChild(i).transform.GetChild (1).transform.gameObject.SetActive(true);
					OnStage.transform.GetChild(i).transform.GetChild (0).transform.gameObject.SetActive(false);
				}else{
					OnStage.transform.GetChild(i).transform.GetChild (0).transform.gameObject.SetActive(true);
					OnStage.transform.GetChild(i).transform.GetChild (1).transform.gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetSpin(string newSpin){
		if(newSpin == "Half"){
			Spinner = 0;
			MakeActiveOnStage(); //switch on stage if any active
			moveScript.clearAllLines(); // remove the lines for ease
			this.GetComponent<Pathing>().ChangeSpin(0);
			this.GetComponent<AllMathData>().SetGunStates(0);
		}else if(newSpin == "One"){
			Spinner = 1;
			MakeActiveOnStage(); //switch on stage if any active
			moveScript.clearAllLines(); // remove the lines for ease
			this.GetComponent<Pathing>().ChangeSpin(1);
			this.GetComponent<AllMathData>().SetGunStates(1);
		}
	}

	public void AddCounter(string name,Vector3 loc){
		if(Components.transform.FindChild (name) != null){
			cons = new string[3];
			current = Components.transform.FindChild (name);
			current.parent = OnStage.transform;
			current.position = loc;
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Counter,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddCounter(){
		int i = 1;
		while(i < 9){
			if(OnStage.transform.FindChild("Counter"+i.ToString()) == null){
				COCOUNTER = i;
				i = 10;
			}
			i++;
		}
		if(Components.transform.FindChild ("Counter"+COCOUNTER.ToString()) != null){
			cons = new string[3];
			current = Components.transform.FindChild ("Counter"+COCOUNTER.ToString());
			COCOUNTER++;
			if(COCOUNTER >=8){
				COCOUNTER = 1;
			}
			current.parent = OnStage.transform;
			current.position = new Vector3(Random.Range(4.0f,5.5f),Random.Range(1.0f,2.66f),0f);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Counter,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddAnalyzer(string name,Vector3 loc,string Axis){
		if(Components.transform.FindChild(name) != null){
			cons = new string[3];
			current = Components.transform.FindChild (name);
			current.parent = OnStage.transform;
			current.position = loc;
			current.transform.GetChild(2).transform.GetComponent<AnalyzerAxisChange>().ChangeAxis(Axis);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Analyzer,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddAnalyzer(){
		int i = 1;
		while(i < 8){
			if(OnStage.transform.FindChild("Analyzer"+i.ToString()) == null){
				ANACOUNTER = i;
				i = 9;
			}
			i++;
		}
		if(Components.transform.FindChild("Analyzer"+ANACOUNTER.ToString()) != null){
			cons = new string[3];
			current = Components.transform.FindChild ("Analyzer"+ANACOUNTER.ToString());
			ANACOUNTER++;
			if(ANACOUNTER >=7){
				ANACOUNTER = 1;
			}
			current.parent = OnStage.transform;
			current.position = new Vector3(Random.Range(4.0f,5.5f),Random.Range(1.0f,2.66f),0f);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Analyzer,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddMagnet(string name,Vector3 loc,string Axis){
		if(Components.transform.FindChild (name) != null){
			cons = new string[3];
			current = Components.transform.FindChild (name);
			current.parent = OnStage.transform;
			current.position = loc;
			current.transform.GetChild(6).transform.GetComponent<MagnetAxisChange>().ChangeAxis(Axis);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Magnet,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddMagnet(){
		int i = 1;
		while(i < 9){
			if(OnStage.transform.FindChild("Magnet"+i.ToString()) == null){
				MAGCOUNTER = i;
				i = 10;
			}
			i++;
		}
		if(Components.transform.FindChild ("Magnet"+MAGCOUNTER.ToString()) != null){
			cons = new string[3];
			current = Components.transform.FindChild ("Magnet"+MAGCOUNTER.ToString());
			MAGCOUNTER++;
			if(MAGCOUNTER >=8){
				MAGCOUNTER = 1;
			}
			current.parent = OnStage.transform;
			current.position = new Vector3(Random.Range(4.0f,5.5f),Random.Range(1.0f,2.66f),0f);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Magnet,0,0,0,0,0,1,-1,-1,-1);
		}
	}

	public void AddGun(Vector3 loc){
		if(OnStage.transform.FindChild("gun") == null && Components.transform.FindChild ("gun") != null){
			cons = new string[3];
			current = Components.transform.FindChild ("gun");
			current.parent = OnStage.transform;
			current.position = loc;//Vector3.zero-Vector3(0.5f,0.5f,0.5f);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Gun,0,0,0,0,0,0,-1,-1,-1);
		}
	}

	public void AddGun(){
		if(OnStage.transform.FindChild("gun") == null && Components.transform.FindChild ("gun") != null){
			cons = new string[3];
			current = Components.transform.FindChild ("gun");
			current.parent = OnStage.transform;
			current.position = new Vector3(Random.Range(4.0f,5.5f),Random.Range(1.0f,2.66f),0f);
			MakeActiveOnStage();
			OnStage.GetComponent<BuildPath>().AddPart(current.name,"",cons,1,0,PathTree.Type.Gun,0,0,0,0,0,0,-1,-1,-1);
		}
	}

	public void DefaultSettings(){
		for(int i = 1; i < 8; i++){
			GameObject.Find ("Magnet"+i.ToString()).transform.GetChild(6).GetComponent<MagnetAxisChange>().Default();
			GameObject.Find ("Magnet"+i.ToString()).transform.GetChild(7).GetComponent<MagnetCounter>().Default();
		}
		for(int i = 1; i < 7; i++){
			GameObject.Find ("Analyzer"+i.ToString()).transform.GetChild(2).GetComponent<AnalyzerAxisChange>().Default();
		}
	}


	public void ResetCounters(){
		for(int i = 1; i < 8; i++){
			GameObject.Find ("Counter"+i.ToString()).transform.GetChild(2).GetComponent<TextMesh>().text = "0";
			GameObject.Find ("Counter"+i.ToString()).GetComponent<CounterData>().ResetCount();
		}
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(watchTime);
		watchUpdate();
	}

}
