using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCannon : MonoBehaviour {
	GameObject cratePrefab;
	float timeToCheckMaxCrateHeight;
	float timeToCheckMaxCrateHeightCounter;
	float timeToSpawnCrate;
	float timeToSpawnCrateCounter;
	bool crateSpawnTrigger, crateSpawnTrigger2;
	// Use this for initialization
	void Start () {
		cratePrefab = (GameObject)Resources.Load ("Prefabs/box");
		timeToCheckMaxCrateHeight = 0.5f;
		timeToCheckMaxCrateHeightCounter = 0f;
		timeToSpawnCrate = 1f;
		timeToSpawnCrateCounter = 0f;
		crateSpawnTrigger = false;
		crateSpawnTrigger2 = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (crateSpawnTrigger) {
			timeToCheckMaxCrateHeightCounter += Time.deltaTime;
			if (timeToCheckMaxCrateHeightCounter >= timeToCheckMaxCrateHeight) {
				timeToCheckMaxCrateHeightCounter = 0f;
				crateSpawnTrigger = false;
				float maxSoFar = transform.position.y;
				foreach (GameObject crate in GameObject.FindGameObjectsWithTag("Crate")) {
					if (crate.transform.localPosition.y > maxSoFar) {
						maxSoFar = crate.transform.localPosition.y;
					}
				}
				GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (transform.position.x, maxSoFar));
				//transform.position = new Vector3 (transform.position.x, maxSoFar, transform.position.z);
			}
		}

		if (crateSpawnTrigger2) {
			timeToSpawnCrateCounter += Time.deltaTime;
			if (timeToSpawnCrateCounter >= timeToSpawnCrate) {
				timeToSpawnCrateCounter = 0f;
				crateSpawnTrigger2 = false;
				GameObject.Instantiate (cratePrefab, gameObject.transform);
			}
		}
	}

	public void SpawnCrate(){
		timeToCheckMaxCrateHeightCounter = 0f;
		crateSpawnTrigger = true;
		crateSpawnTrigger2 = true;
	}
}
