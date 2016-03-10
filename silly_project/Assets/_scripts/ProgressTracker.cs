using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour {

	private static ProgressTracker tracker;
	private bool bridgeItem;

	// Make sure there is only one instance of this class
	void Awake () {
		bridgeItem = false;
		if (tracker == null) {
			DontDestroyOnLoad (gameObject);
			tracker = this;
		}
		else if (tracker != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static ProgressTracker GetProgressTracker() {
		return tracker;
	}

	public bool GetBool (string condition) {
		if (condition.Equals ("bridgeItem"))
			return bridgeItem;
		return false;
	}

	public void setBool (string condition, bool satisfied) {
		if (condition.Equals ("bridgeItem"))
			bridgeItem = satisfied;
	}
}
