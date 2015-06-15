using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildPath : MonoBehaviour {
	public List<PathTree> myTree = new List<PathTree>();
	// Use this for initialization
	public void AddPart(string name, string parent, string[] Connectors, int outs, int ins, PathTree.Type item, int dir, int str, float up, float mid, float dwn,int id,int idt,int idm,int idb){
		myTree.Add (new PathTree(name,parent,Connectors,outs,ins,item,dir,str,up,mid,dwn,id,idt,idm,idb));
	}

	public void IncreaseInput(string s){
		for(int i = 0; i < myTree.Count; i++){
			if(myTree[i].PartName == s){
				myTree[i].inputs++;
			}
		}
	}

	public void DecreaseInput(string s){
		for(int i = 0; i< myTree.Count; i++){
			if(myTree[i].PartName == s){
				myTree[i].inputs--;
			}
		}
	}

	public int GetInputAmnt(string s){
		for(int i = 0; i< myTree.Count; i++){
			if(myTree[i].PartName == s){
				return myTree[i].inputs;
			}
		}
		return 0;
	}

	public void ResetLineIDs(){
		for(int i = 0 ; i < myTree.Count; i++){
			myTree[i].ID = 0;
			myTree[i].IDB = -1;
			myTree[i].IDM = -1;
			myTree[i].IDT = -1;
		}
	}

	public string GetPartByID(int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].ID == id){
				return myTree[i].PartName;
			}
		}
		return null;
	}

	public string GetStartByLineID(int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].IDT == id || myTree[i].IDM == id || myTree[i].IDB == id){
				return myTree[i].PartName;
			}
		}
		return "none";
	}

	public string GetEndLine(string s, int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == s){
				if(myTree[i].IDT == id){
					return myTree[i].Connections[0];
				}
				if(myTree[i].IDM == id){
					return myTree[i].Connections[1];
				}
				if(myTree[i].IDB == id){
					return myTree[i].Connections[2];
				}
			}
		}
		return "none";
	}

	public void SetID(string name, int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].ID = id;
				break;
			}
		}
	}

	public void SetLineID(string name,int spot, int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				if(spot == 0){
					myTree[i].IDT = id;
				}else if(spot == 1){
					myTree[i].IDM = id;
				}else{
					myTree[i].IDB = id;
				}
			}
		}
	}

	public int GetLineID(string name,int spot){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				if(spot == 0){
					return myTree[i].IDT;
				}else if(spot == 1){
					return myTree[i].IDM;
				}else{
					return myTree[i].IDB;
				}
			}
		}
		return -1;
	}

	public int GetLinePath(string name,int id){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				if(myTree[i].IDT == id){
					return 0;
				}else if(myTree[i].IDM == id){
					return 1;
				}else if(myTree[i].IDB == id){
					return 2;
				}
			}
		}
		return -1;
	}

	public int conPosition(string s,string e){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == s){
				if(myTree[i].Connections[0] == e){
					return 0;
				}else if(myTree[i].Connections[1] == e){
					return 1;
				}else if(myTree[i].Connections[2] == e){
					return 2;
				}
			}
		}
		return -1;
	}

	public void UpdateConnections(string name, string[] Connectors){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Connections = Connectors;
				break;
			}
		}
	}

	public void UpdateConTop(string name, string c){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Connections[0] = c;
				break;
			}
		}
	}

	public void UpdateConMid(string name, string c){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Connections[1] = c;
				break;
			}
		}
	}

	public void UpdateConBot(string name, string c){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Connections[2] = c;
				break;
			}
		}
	}

	public void SetParent(string name,string parent){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Parent = parent;
			}
		}
	}

	public string GetParent(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Parent;
			}
		}
		return null;
	}


	public string[] GetConnections(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Connections;
			}
		}
		return null;
	}

	public string GetConTop(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Connections[0];
			}
		}
		return null;
	}

	public string GetConMid(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Connections[1];
			}
		}
		return null;
	}

	public string GetConBot(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Connections[2];
			}
		}
		return null;
	}




	public string GetItem(string name, int loc){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Connections[loc];
			}
		}
		return null;
	}

	public float GetProbs(string name, int p){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				if(p == 0){
					return myTree[i].probUp;
				}else if(p == 1){
					return myTree[i].probMid;
				}else if(p == 2){
					return myTree[i].probDwn;
				}
			}
		}
		return 0;
	}


	public void UpdateSpinType(string name, int outs, int ins){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].inputs = ins;
				myTree[i].outputs = outs;
			}
		}
	}

	public void UpdateDirection(string name,int dir){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Direction = dir;
			}
		}
	}

	public int GetDirection(string name){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				return myTree[i].Direction;
			}
		}
		return 0;
	}

	public void UpdateStrength(string name, int str){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].Strength = str;
			}
		}
	}

	public void UpdateProb(string name,float up, float mid, float dwn){
		for(int i = 0 ; i < myTree.Count; i++){
			if(myTree[i].PartName == name){
				myTree[i].probUp = up;
				myTree[i].probMid = mid;
				myTree[i].probDwn = dwn;
			}
		}
	}

	public void ClearList(){
		myTree.Clear();
	}

	public void ClearConnectors(){
		for(int i = 0 ; i < myTree.Count; i++){
			myTree[i].Connections[0] = "";
			myTree[i].Connections[1] = "";
			myTree[i].Connections[2] = "";
		}
	}

}
