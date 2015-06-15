using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShortCuts : MonoBehaviour {

	private bool ActiveKeys = true;
	private bool watchCom = false;
	public GameObject watchme;
	public GameObject CtrlPan;
	public GameObject CtrlBar;
	public GameObject InputTime;
	public GameObject Ang;


	public void Active( bool act){
		ActiveKeys = act;
	}


	void Update () {

		if(ActiveKeys == true){
			if(Input.GetKeyUp (KeyCode.D)){
				GameObject.Find ("GUI").transform.GetComponent<LoadExperiments>().LoadExperiment("Default");
			}
			if(Input.GetKeyUp (KeyCode.N)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().AddAnalyzer();
			}
			if(Input.GetKeyUp (KeyCode.M)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().AddMagnet();
			}
			if(Input.GetKeyUp (KeyCode.K)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().AddCounter();
			}
			if(Input.GetKeyUp (KeyCode.U)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().AddGun();
			}
			if(Input.GetKeyUp (KeyCode.A)){
				Ang.SetActive(!Ang.activeSelf);
			}
			if(Input.GetKeyUp (KeyCode.G)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().goStop (0);
			}
			if(Input.GetKeyUp (KeyCode.S)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().goStop (1);
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().stopping();
			}
			if(Input.GetKeyUp (KeyCode.R)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().ResetCounters();
			}
			if(Input.GetKeyUp (KeyCode.Alpha1)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Do (1);
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().resetMoves();
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().TurnOffCounter(true);
			}
			if(Input.GetKeyUp (KeyCode.Alpha2)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Do (10);
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().resetMoves();
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().TurnOffCounter(true);
			}
			if(Input.GetKeyUp (KeyCode.Alpha3)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Do (100);
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().resetMoves();
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().TurnOffCounter(true);
			}
			if(Input.GetKeyUp (KeyCode.Alpha4)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Do (1000);
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().resetMoves();
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().TurnOffCounter(true);
			}
			if(Input.GetKeyUp (KeyCode.Alpha5)){
				GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Do (10000);
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().resetMoves();
				GameObject.Find ("GUI").transform.GetComponent<Pathing>().TurnOffCounter(true);
			}
			if(Input.GetKeyUp (KeyCode.W)){
				//GameObject.Find ("GUI").transform.GetComponent<MainGUI>().Watch();
				//GameObject.Find ("GUI").transform.GetComponent<Pathing>().Watch();
				watchCom = !watchCom;
				watchme.transform.GetComponent<Toggle>().isOn = watchCom;
				CtrlPan.GetComponent<activeMenu>().Activate(CtrlBar);
			}
			if(Input.GetKeyUp (KeyCode.T)){
				InputTime.SetActive(true);
			}
		}
	}
}
