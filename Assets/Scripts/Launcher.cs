using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {
	public static int cratesLaunched;
	GameObject powerBarMask;
	GameObject launchingStation;
	GameObject powerBar;
	float launchForce;
	float initLaunchForce;
	float maxLaunchForce;
	float rateLaunchForce;
	bool increasingLaunchForce;
	bool hasLaunched;
	bool once;
	static bool once2, once3;
	float delayBeforeCheckingCollision;
	float delayBeforeCheckingCollisionCounter;
	static float motorSpeed = -50f;
	JointMotor2D jointMotor;
	SpriteRenderer powerBarBack;

	// Use this for initialization
	void Start () {
		powerBarMask = GameObject.Find ("PowerBarMask");
		launchingStation = GameObject.Find ("LaunchingStation");
		powerBar = GameObject.Find ("PowerBar");
		initLaunchForce = 1000f;
		maxLaunchForce = 8000f;
		rateLaunchForce = 80f;
		delayBeforeCheckingCollision = 0.2f;
		delayBeforeCheckingCollisionCounter = 0f;
		increasingLaunchForce = true;
		hasLaunched = false;
		launchForce = initLaunchForce;
		once = true;
		once2 = true;
		once3 = true;
		jointMotor = new JointMotor2D();
		jointMotor.motorSpeed = motorSpeed;
		jointMotor.maxMotorTorque = 10000f;
		launchingStation.GetComponentInChildren<HingeJoint2D> ().motor = jointMotor;
		powerBarBack = GameObject.Find ("PowerBarBack").GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (powerBar.transform.localEulerAngles.z);
		if (!Input.GetMouseButton(0)) {
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
			if (Input.GetMouseButton(0)) {
				if (increasingLaunchForce) {
					launchForce += rateLaunchForce;
				} else {
					launchForce -= rateLaunchForce;
				}

				if (launchForce >= maxLaunchForce) {
					launchForce = maxLaunchForce;
					increasingLaunchForce = false;
				}

				if (launchForce <= initLaunchForce) {
					launchForce = initLaunchForce;
					increasingLaunchForce = true;
				}
			}

			powerBarMask.transform.localPosition = new Vector3 (
				powerBarMask.transform.localPosition.x,
				-3.5f * (1f - ((launchForce - initLaunchForce) / (maxLaunchForce - initLaunchForce))),
				powerBarMask.transform.localPosition.z);


			if (Input.GetMouseButtonUp(0)) {
				float launchForceY = ((powerBar.transform.localEulerAngles.z - 280f)/ (350f - 280f)) * launchForce;
				float launchForceX = (1f - ((powerBar.transform.localEulerAngles.z - 280f)/ (350f - 280f))) * launchForce;
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (launchForceX, launchForceY));
				Debug.Log ("X: " + launchForceX + " Y: " + launchForceY);
			
				launchForce = initLaunchForce;
				increasingLaunchForce = true;
				powerBarMask.transform.localPosition = new Vector3 (
					powerBarMask.transform.localPosition.x,
					-3.5f,
					powerBarMask.transform.localPosition.z);
				hasLaunched = true;
				//powerBarBack.enabled = false;
				GameObject.Find ("PowerBarBack").GetComponent<SpriteRenderer> ().enabled = false;
				GameObject.Find ("LaunchingStationFloor").GetComponent<BoxCollider2D> ().enabled = false;
				cratesLaunched += 1;
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
				if (GetComponent<SpriteRenderer> ().isVisible) {
					launchingStation.GetComponent<CrateCannon> ().SpawnCrate ();
				} else {
					launchingStation.GetComponent<CrateCannon> ().EndGame ();
				}
				once = false;
			}
		}
	}
}
