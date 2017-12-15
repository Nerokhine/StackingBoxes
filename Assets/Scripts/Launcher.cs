using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {
	GameObject powerBarMask;
	GameObject launchingStation;
	GameObject powerBar;
	float launchForceX, launchForceY;
	float initLaunchForceX, initLaunchForceY;
	float maxLaunchForceX, maxLaunchForceY;
	float rateLaunchForceX, rateLaunchForceY;
	bool increasingLaunchForce;
	bool hasLaunched;
	bool once;
	bool once2, once3;
	float delayBeforeCheckingCollision;
	float delayBeforeCheckingCollisionCounter;
	float motorSpeed;
	JointMotor2D jointMotor;

	// Use this for initialization
	void Start () {
		powerBarMask = GameObject.Find ("PowerBarMask");
		launchingStation = GameObject.Find ("LaunchingStation");
		powerBar = GameObject.Find ("PowerBar");
		initLaunchForceX = 800f;
		initLaunchForceY = 1600f;
		maxLaunchForceX = 2000f;
		maxLaunchForceY = 4000f;
		rateLaunchForceX = 20f;
		rateLaunchForceY = 40f;
		motorSpeed = -50f;
		delayBeforeCheckingCollision = 0.2f;
		delayBeforeCheckingCollisionCounter = 0f;
		increasingLaunchForce = true;
		hasLaunched = false;
		launchForceX = initLaunchForceX;
		launchForceY = initLaunchForceY;
		once = true;
		once2 = true;
		once3 = true;
		jointMotor = new JointMotor2D();
		jointMotor.motorSpeed = motorSpeed;
		jointMotor.maxMotorTorque = 10000f;
		launchingStation.GetComponentInChildren<HingeJoint2D> ().motor = jointMotor;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Input.GetKey (KeyCode.Space)) {
			if (powerBar.transform.localEulerAngles.z > 350f) {
				if (once2) {
					motorSpeed *= -1;
					jointMotor.motorSpeed = motorSpeed;
					launchingStation.GetComponentInChildren<HingeJoint2D> ().motor = jointMotor;
					once3 = true;
					once2 = false;
				}
			} else if (powerBar.transform.localEulerAngles.z < 280f) {
				if (once3) {
					motorSpeed *= -1;
					jointMotor.motorSpeed = motorSpeed;
					launchingStation.GetComponentInChildren<HingeJoint2D> ().motor = jointMotor;
					once3 = false;
					once2 = true;
				}
			}
		} else {
			jointMotor.motorSpeed = 0f;
			launchingStation.GetComponentInChildren<HingeJoint2D> ().motor = jointMotor;
		}

		if (!hasLaunched) {
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

			powerBarMask.transform.localPosition = new Vector3 (
				powerBarMask.transform.localPosition.x,
				-3.5f * (1f - ((launchForceY - initLaunchForceY) / (maxLaunchForceY - initLaunchForceY))),
				powerBarMask.transform.localPosition.z);

			Debug.Log ("X: " + launchForceX + " Y: " + launchForceY);


			if (Input.GetKeyUp (KeyCode.Space)) {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (launchForceX, launchForceY));
				launchForceX = initLaunchForceX;
				launchForceY = initLaunchForceY;
				increasingLaunchForce = true;
				powerBarMask.transform.localPosition = new Vector3 (
					powerBarMask.transform.localPosition.x,
					-3.5f,
					powerBarMask.transform.localPosition.z);
				hasLaunched = true;
				//GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
				transform.parent = GameObject.Find ("Boxes").transform;
			}
		}

		if (hasLaunched && delayBeforeCheckingCollisionCounter < delayBeforeCheckingCollision) {
			delayBeforeCheckingCollisionCounter += Time.deltaTime;
		}

		if (delayBeforeCheckingCollisionCounter >= delayBeforeCheckingCollision && once) {
			/*foreach (Collider2D collider in GameObject.FindObjectsOfType<Collider2D>()) {
				if(GetComponent<BoxCollider2D>().IsTouching(collider)){
					launchingStation.GetComponent<CrateCannon>().SpawnCrate();
					once = false;
					break;
				}
			}*/
			if (GetComponent<Rigidbody2D> ().velocity.x == 0f &&
				GetComponent<Rigidbody2D> ().velocity.y == 0f &&
				GetComponent<Rigidbody2D> ().angularVelocity == 0f) {
				launchingStation.GetComponent<CrateCannon>().SpawnCrate();
				once = false;
			}
		}
	}
}
