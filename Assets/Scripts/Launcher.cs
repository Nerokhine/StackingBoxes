using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {
	GameObject powerBarMask;
	float launchForceX, launchForceY;
	float initLaunchForceX, initLaunchForceY;
	float maxLaunchForceX, maxLaunchForceY;
	float rateLaunchForceX, rateLaunchForceY;
	bool increasingLaunchForce;

	// Use this for initialization
	void Start () {
		powerBarMask = GameObject.Find ("PowerBarMask");
		initLaunchForceX = 50f;
		initLaunchForceY = 100f;
		maxLaunchForceX = 200f;
		maxLaunchForceY = 400f;
		rateLaunchForceX = 2f;
		rateLaunchForceY = 4f;
		increasingLaunchForce = true;
		launchForceX = initLaunchForceX;
		launchForceY = initLaunchForceY;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			if (increasingLaunchForce) {
				launchForceX += rateLaunchForceX;
				launchForceY += rateLaunchForceY;
			} else {
				launchForceX -= rateLaunchForceX;
				launchForceY -= rateLaunchForceY;
			}

			if (launchForceY >= maxLaunchForceY) {
				launchForceX = maxLaunchForceX;
				launchForceY = maxLaunchForceY;
				increasingLaunchForce = false;
			}

			if (launchForceY <= initLaunchForceY) {
				launchForceX = initLaunchForceX;
				launchForceY = initLaunchForceY;
				increasingLaunchForce = true;
			}
		}

		powerBarMask.transform.localPosition = new Vector3(
			powerBarMask.transform.localPosition.x,
			-3.5f * (1f - ((launchForceY - initLaunchForceY) / (maxLaunchForceY - initLaunchForceY))),
			powerBarMask.transform.localPosition.z);

		Debug.Log ("X: " + launchForceX + " Y: " + launchForceY);


		if(Input.GetKeyUp(KeyCode.Space)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(launchForceX, launchForceY));
			launchForceX = initLaunchForceX;
			launchForceY = initLaunchForceY;
			increasingLaunchForce = true;
		}
	}
}
