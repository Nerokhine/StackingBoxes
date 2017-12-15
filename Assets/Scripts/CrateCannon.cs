using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrateCannon : MonoBehaviour {
	GameObject cratePrefab;
	float timeToCheckMaxCrateHeight;
	float timeToCheckMaxCrateHeightCounter;
	float timeToSpawnCrate;
	float timeToSpawnCrateCounter;
	bool crateSpawnTrigger, crateSpawnTrigger2;
	int crateCeiling;
	float maxSoFar;
	bool startEndGame;
	float startRestartGameCounter;
	// Use this for initialization
	void Start () {
		cratePrefab = (GameObject)Resources.Load ("Prefabs/box");
		timeToCheckMaxCrateHeight = 0.5f;
		timeToCheckMaxCrateHeightCounter = 0f;
		timeToSpawnCrate = 1f;
		timeToSpawnCrateCounter = 0f;
		crateSpawnTrigger = false;
		crateSpawnTrigger2 = false;
		maxSoFar = 0f;
		startRestartGameCounter = 0f;
		startEndGame = false;
		crateCeiling = 5;
		Launcher.cratesLaunched = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (startEndGame) {
			startRestartGameCounter += Time.deltaTime;
		}

		if (startRestartGameCounter >= 5.0f) {
			Application.LoadLevel(Application.loadedLevel);
		}

		if (crateSpawnTrigger) {
			timeToCheckMaxCrateHeightCounter += Time.deltaTime;
			if (timeToCheckMaxCrateHeightCounter >= timeToCheckMaxCrateHeight) {
				timeToCheckMaxCrateHeightCounter = 0f;
				crateSpawnTrigger = false;
				foreach (GameObject crate in GameObject.FindGameObjectsWithTag("Crate")) {
					if (crate.transform.localPosition.y > maxSoFar) {
						maxSoFar = crate.transform.localPosition.y;
					}
				}
					

				//raise ceiling every 5 crates
				if (Launcher.cratesLaunched == crateCeiling) {
					GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (transform.position.x, transform.position.y + 1f));
					Camera.main.transform.position = new Vector3 (0, Camera.main.transform.position.y + 1f, -10f);
					crateCeiling += 5;
				}

				GameObject.Find ("ScoreText").GetComponent<Text>().text = "Score: " + maxSoFar.ToString("F2");
				GameObject.Find ("ElevateText").GetComponent<Text> ().text = "Crates Until Next Level: " + (crateCeiling - Launcher.cratesLaunched);
			}
		}

		if (crateSpawnTrigger2) {
			timeToSpawnCrateCounter += Time.deltaTime;
			if (timeToSpawnCrateCounter >= timeToSpawnCrate) {
				timeToSpawnCrateCounter = 0f;
				crateSpawnTrigger2 = false;
				GameObject.Find("PowerBarBack").GetComponent<SpriteRenderer>().enabled = true;
				GameObject.Find ("LaunchingStationFloor").GetComponent<BoxCollider2D> ().enabled = true;
				GameObject.Instantiate (cratePrefab, gameObject.transform);
			}
		}
	}

	public void SpawnCrate(){
		timeToCheckMaxCrateHeightCounter = 0f;
		crateSpawnTrigger = true;
		crateSpawnTrigger2 = true;
	}

	public void EndGame(){
		GameObject.Find ("WastedText").GetComponent<Text>().enabled = true;
		startEndGame = true;
	}
}
