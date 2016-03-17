using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressTracker : MonoBehaviour {

	// Single ProgressTracker instance and checkpoint bools
	private static ProgressTracker tracker;
	private bool bridgeItem;
    private bool woodsmanItem;
	private bool woodsmanChat;
	private bool bridgeCheck;
	private bool bridgeDown;

	// Quest objects for instantiation when checkpoint is reached, etc.
	public GameObject woodsmanCube;
	private GameObject woodsmanObject;

	private StoryManager storyManager;

	// Make sure there is only one instance of this class
	void Awake () {
		woodsmanChat = false;
        woodsmanItem = false;
        bridgeItem = false;
		bridgeCheck = false;
		bridgeDown = false;

		if (tracker == null) {
			DontDestroyOnLoad (gameObject);
			tracker = this;
		}
		else if (tracker != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		storyManager = StoryManager.GetStoryManager ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static ProgressTracker GetProgressTracker() {
		return tracker;
	}

	// Return bool for specified progress condition
	public bool GetBool (string condition) {
		if (condition.Equals ("bridgeItem"))
			return bridgeItem;
        if (condition.Equals("woodsmanItem"))
            return woodsmanItem;
		if (condition.Equals("woodsmanChat"))
			return woodsmanChat;
		if (condition.Equals("bridgeCheck"))
			return bridgeCheck;
		if (condition.Equals("bridgeDown"))
			return bridgeDown;
        return false;
	}

	// Set the specified condition bool to mark player progress through various checkpoints
	public void setBool (string condition, bool satisfied) {
		if (condition.Equals ("bridgeItem")) {
			storyManager.ChangeText ("He met a woodsman searching for a CHEST. Maybe he could use the woodsman's AXE ...");
			bridgeItem = satisfied;
		}
		if (condition.Equals ("woodsmanItem")) {
			woodsmanItem = satisfied;
			storyManager.ChangeText ("He met a woodsman searching for a CHEST. Maybe he could use the woodsman's ___ ...");
		}
		if (condition.Equals ("woodsmanChat")) {
			storyManager.ChangeText ("He met a woodsman searching for a _____. Maybe he could use the woodsman's ___ ...");
			Debug.Log ("Here");
			woodsmanChat = satisfied;

			woodsmanObject = GameObject.Instantiate (woodsmanCube);
			woodsmanObject.transform.position = new Vector3 (8f, 0.75f, -3f);
		}
		if (condition.Equals ("bridgeCheck")) {
			bridgeCheck = satisfied;

			if (!woodsmanChat) {
				storyManager.AddText ("... but the bridge was out. He would need to find another path.");
				storyManager.ShowText ();
			} else {
				storyManager.AddText (" The bridge was out, but the knight had a plan.");
			}
		}
		if (condition.Equals ("bridgeDown")) {
			storyManager.ChangeText ("Having felled the tree, our hero coninued on his quest.");
			bridgeDown = satisfied;
		}
    }
}
