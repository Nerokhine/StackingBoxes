using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCannon : MonoBehaviour {
	GameObject cratePrefab;
	// Use this for initialization
	void Start () {
		cratePrefab = (GameObject)Resources.Load ("Prefabs/box");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnCrate(){
		GameObject.Instantiate(cratePrefab, gameObject.transform);
		Debug.Log ("Hello");
	}
}
