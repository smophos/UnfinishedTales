using UnityEngine;
using System.Collections;

public class BridgeScript : MonoBehaviour {

	ProgressTracker tracker;

	// Use this for initialization
	void Start () {
		ProgressTracker.RegisterObjective (gameObject, 2);
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		tracker = ProgressTracker.GetProgressTracker ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.B))
			Activate ();
	}
		
	public void Activate() {
		if (!tracker.GetBool ("bridgeDown")) {
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			GetComponent<Rigidbody> ().useGravity = true;

			tracker.setBool ("bridgeDown", true);
		}
	}
}
