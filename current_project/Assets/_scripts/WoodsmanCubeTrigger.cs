using UnityEngine;
using System.Collections;

public class WoodsmanCubeTrigger : MonoBehaviour {

	ProgressTracker tracker;

	// Use this for initialization
	void Start () {
		ProgressTracker.RegisterObjective (gameObject, 1);
		tracker = ProgressTracker.GetProgressTracker ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			tracker.setBool ("woodsman_item", true);
			Destroy (this.gameObject);
		}
	}
}
