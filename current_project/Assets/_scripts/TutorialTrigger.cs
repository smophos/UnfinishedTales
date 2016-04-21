using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

	ProgressTracker tracker;
	ActiveAgent player;

	// Use this for initialization
	void Start () {
		tracker = ProgressTracker.GetProgressTracker ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActiveAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tracker.GetBool ("beginLane") && ! tracker.GetBool ("laneMovement") && !PauseMenuController.GetMenuController().gamePaused) {
			if (InputMapper.GetInputDown("Forward") || InputMapper.GetInputDown("Backward"))
				tracker.setBool ("laneMovement", true);
				//player.Pause ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (!tracker.GetBool ("beginLane"))
				tracker.setBool ("beginLane", true);
			//player.Pause ();
		}
	}
}
