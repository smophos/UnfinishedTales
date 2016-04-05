using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProgressTracker : MonoBehaviour {

	// Single ProgressTracker instance and checkpoint bools
	private static ProgressTracker tracker;
	private bool fairyMet;
	private bool bridgeItem;
    private bool woodsmanItem;
	private bool woodsmanChat;
	private bool bridgeCheck;
	private bool bridgeDown;
	Dictionary<string, bool> conditionDictionary; 

	public Transform player;

	// Quest objects for instantiation when checkpoint is reached, etc.
	public GameObject woodsmanCube;
	private GameObject woodsmanObject;
	private static GameObject[] objectives = new GameObject[20];

	private StoryManager storyManager;

	public delegate void ObjectiveAction(Transform goal);
	public static event ObjectiveAction ObjectiveChanged;

	// Make sure there is only one instance of this class
	void Awake () {
		fairyMet = false;
		woodsmanChat = false;
        woodsmanItem = false;
        bridgeItem = false;
		bridgeCheck = false;
		bridgeDown = false;

		conditionDictionary = new Dictionary<string, bool> () {
			{"woodsman_met", woodsmanChat},
			{"woodsman_item", woodsmanItem},
			{"fairy_met", fairyMet}
		};

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

	public static void RegisterObjective (GameObject obj, int index) {
		objectives [index] = obj;
	}

	public Dictionary<string, bool> GetConditionDict () {
//		Debug.Log ("Conditions:");
//		foreach (var r in conditionDictionary)
//			Debug.Log ("In Class: " + r);
		return conditionDictionary;
	}

	public static ProgressTracker GetProgressTracker () {
		return tracker;
	}

	// Return bool for specified progress condition
	public bool GetBool (string condition) {
		if (condition.Equals("bridgeItem"))
			return bridgeItem;
		if (condition.Equals("woodsman_item"))
            return woodsmanItem;
		if (condition.Equals("woodsman_met"))
			return woodsmanChat;
		if (condition.Equals("bridgeCheck"))
			return bridgeCheck;
		if (condition.Equals("bridgeDown"))
			return bridgeDown;
		if (condition.Equals("woodsman_met_no_axe"))
			return (woodsmanChat && !bridgeItem);
		if (condition.Equals("fairy_met"))
			return fairyMet;
        return false;
	}

	// Set the specified condition bool to mark player progress through various checkpoints
	public void setBool (string condition, bool satisfied) {
		if (condition.Equals ("fairy_met")) {
			fairyMet = satisfied;
			conditionDictionary["fairy_met"] = satisfied;
		}
		if (condition.Equals ("bridgeItem")) {
			storyManager.ChangeText ("He met a woodsman searching for a CHEST. Maybe he could use the woodsman's AXE ...");
			storyManager.ShowText ();
			bridgeItem = satisfied;
			ObjectiveChanged (objectives [2].transform);
		}
		if (condition.Equals ("woodsman_item")) {
			woodsmanItem = satisfied;
			conditionDictionary["woodsman_item"] = satisfied;
			storyManager.ChangeText ("He met a woodsman searching for a CHEST. Maybe he could use the woodsman's ___ ...");
			storyManager.ShowText ();
			ObjectiveChanged (objectives [0].transform);
		}
		if (condition.Equals ("woodsman_met")) {
			storyManager.ChangeText ("He met a woodsman searching for a _____. Maybe he could use the woodsman's ___ ...");
			storyManager.ShowText ();
			Debug.Log ("Here");
			woodsmanChat = satisfied;
			conditionDictionary ["woodsman_met"] = satisfied;
			ObjectiveChanged (objectives [1].transform);
		}
		if (condition.Equals ("bridgeCheck")) {
			bridgeCheck = satisfied;

			if (!woodsmanChat) {
				storyManager.AddText ("... but the bridge was out. He would need to find another path.");
				storyManager.ShowText ();
			} else {
				storyManager.AddText (" The bridge was out, but the knight had a plan.");
				storyManager.ShowText ();
			}
		}
		if (condition.Equals ("bridgeDown")) {
			storyManager.ChangeText ("Having felled the tree, our hero coninued on his quest.");
			storyManager.ShowText ();
			bridgeDown = satisfied;
		}
    }
}
