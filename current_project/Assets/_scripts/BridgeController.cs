using UnityEngine;
using System.Collections;

public class BridgeController : MonoBehaviour {

	ProgressTracker tracker;
	public BridgeScript bridge;
	DialoguePanel panel;

	// Use this for initialization
	void Start () {
		tracker = ProgressTracker.GetProgressTracker ();
		panel = DialoguePanel.Instance ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if(!tracker.GetBool("bridgeCheck"))
			tracker.setBool("bridgeCheck", true);

		if (other.gameObject.CompareTag ("Player") && tracker.GetBool ("bridgeItem")) {
			bridge.Activate ();
		} else {
			//panel.ShowDialogue ("Need to find a way to get across! Try another route.");
		}
	}
}
