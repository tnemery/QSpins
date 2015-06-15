using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PathTree {
	public string[] Connections;
	public string Parent;
	public string PartName;
	public int outputs;
	public int inputs;
	public Type myObj;
	public int Direction;
	public int Strength;
	public float probUp;
	public float probMid;
	public float probDwn;
	public int ID;
	public int IDT;
	public int IDM;
	public int IDB;
	public enum Type{
		Gun,
		Analyzer,
		Magnet,
		Counter
	};

	public PathTree(string name,string parent, string[] Connectors, int outs, int ins, Type item, int dir, int str, float up, float mid, float dwn, int id, int idt, int idm, int idb){
		PartName = name;
		Parent = parent;
		Connections = Connectors;
		outputs = outs;
		inputs = ins;
		myObj = item;
		Direction = dir;
		Strength = str;
		probUp = up;
		probMid = mid;
		probDwn = dwn;
		ID = id;
		IDT = idt;
		IDM = idm;
		IDB = idb;
	}

	public PathTree(){

	}

}
