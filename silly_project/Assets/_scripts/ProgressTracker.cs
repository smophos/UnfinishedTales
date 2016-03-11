using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour {

	private static ProgressTracker tracker;
	private bool bridgeItem;
    private bool woodsmanItem;

	// Make sure there is only one instance of this class
	void Awake () {
        woodsmanItem = false;
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
        if (condition.Equals("woodsmanItem"))
            return woodsmanItem;
        return false;
	}

	public void setBool (string condition, bool satisfied) {
		if (condition.Equals ("bridgeItem"))
			bridgeItem = satisfied;
        if (condition.Equals("woodsmanItem"))
            woodsmanItem = satisfied; ;
    }
}
