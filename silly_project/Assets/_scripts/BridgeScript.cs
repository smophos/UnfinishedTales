using UnityEngine;
using System.Collections;

public class BridgeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.B)) {
			ProgressTracker.GetProgressTracker ().setBool ("bridgeItem", true);
		}
	}

	public void Activate() {
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		GetComponent<Rigidbody> ().useGravity = true;
	}
}
