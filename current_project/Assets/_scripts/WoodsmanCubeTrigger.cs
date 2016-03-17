﻿using UnityEngine;
using System.Collections;

public class WoodsmanCubeTrigger : MonoBehaviour {

	ProgressTracker tracker;

	// Use this for initialization
	void Start () {
		tracker = ProgressTracker.GetProgressTracker ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			tracker.setBool ("woodsmanItem", true);
			Destroy (this.gameObject);
		}
	}
}