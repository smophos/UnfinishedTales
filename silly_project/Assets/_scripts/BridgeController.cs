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
		if (other.gameObject.CompareTag ("Player") && tracker.GetBool ("bridgeItem")) {
			bridge.Activate ();
		} else {
			panel.ShowDialogue ("Need bridge item to pass!");
		}
	}
}
