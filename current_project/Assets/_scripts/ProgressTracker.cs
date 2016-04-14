using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

// Class to handle changing of World Event and objective flags
// Updates story text based on events

public class ProgressTracker : MonoBehaviour {

	// Single ProgressTracker instance and checkpoint bools
	private static ProgressTracker tracker;

	// Dictionaries to hold cpondition, value pairs and condition, string response pairs
	Dictionary<string, bool> conditionDictionary;
	Dictionary<string, string> storyDictionary; 

	public Transform player;

	// Quest objects for instantiation when checkpoint is reached, waypoint for book, etc.
	public GameObject woodsmanCube;
	private GameObject woodsmanObject;
	private static GameObject[] objectives = new GameObject[20];

	// StoryManger reference and XElement for xml parsing
	private StoryManager storyManager;
	XElement element;

	// Delegates for notifying the book ActiveAgent of updated objectives
	public delegate void ObjectiveAction(Transform goal);
	public static event ObjectiveAction ObjectiveChanged;

	// Make sure there is only one instance of this class
	void Awake () {
		conditionDictionary = new Dictionary<string, bool> ();
		//		{
		//			{"woodsman_met", woodsmanChat},
		//			{"woodsman_item", woodsmanItem},
		//			{"fairy_met", fairyMet}
		//		};
		storyDictionary = new Dictionary<string, string> ();
		TextAsset progressData = Resources.Load ("ProgressData") as TextAsset;
		XDocument document = XDocument.Parse (progressData.text);
		element = document.Root;
		//element = XElement.Load (".\\Assets\\_scripts\\ProgressData.xml");
		ParseXML (element);

		//		Debug.Log ("Conditions:");
		//		foreach (var v in conditionDictionary)
		//			Debug.Log (v);
		//		Debug.Log ("Story Text:");
		//		foreach (var v in storyDictionary)
		//			Debug.Log (v);

		if (tracker == null) {
			DontDestroyOnLoad (gameObject);
			tracker = this;
		}
		else if (tracker != this) {
			Destroy (gameObject);
		}
	}

	// Get the singleton StoryManager object at start
	void Start () {
		storyManager = StoryManager.GetStoryManager ();
		setBool ("intro", true);
	}

	// Update is called once per frame
	void Update () {

	}

	// Interface for objectives to register themselves with the ProgressTracker to notify the book
	public static void RegisterObjective (GameObject obj, int index) {
		objectives [index] = obj;
	}

	// Return conditionDictionary
	public Dictionary<string, bool> GetConditionDict () {
		//		Debug.Log ("Conditions:");
		//		foreach (var r in conditionDictionary)
		//			Debug.Log ("In Class: " + r);
		return conditionDictionary;
	}

	// Get the single, static instance of ProgressTracker
	public static ProgressTracker GetProgressTracker () {
		return tracker;
	}

	// Return bool for specified progress condition
	public bool GetBool (string condition) {
		if (conditionDictionary.ContainsKey(condition) && conditionDictionary[condition] != null)
			return conditionDictionary[condition];
		return false;
	}

	// Set the specified condition bool to mark player progress through various checkpoints
	public void setBool (string condition, bool satisfied) {

		// Check if conditionDictionary contains gien condition, and if so
		// set and update story text if necessary
		if (conditionDictionary.ContainsKey (condition)) {
			conditionDictionary [condition] = satisfied;
			if (storyDictionary[condition] != "")
				storyManager.ChangeText (storyDictionary[condition]);
		}

	}

	// Helper method to convert parsed XML for condition and value
	// into conditionDictionary
	void ConvertToConditionDictionary (string condition, string value)
	{
		if (condition == "" && value == "") {
			conditionDictionary.Add("None", true);
			return;
		}

		if (value.ToLower().Equals("true"))
			conditionDictionary.Add(condition, true);
		else if (value.ToLower().Equals("false"))
			conditionDictionary.Add(condition, false);
	}

	// Helper method to convert parsed XML for condition and script
	// into storyDictionary
	void ConvertToStoryDictionary (string condition, string storyUpdate)
	{
		if (condition == "" && storyUpdate == "") {
			storyDictionary.Add("None", "");
			return;
		}
		storyDictionary.Add(condition, storyUpdate);
	}

	// Parse xml file for ProgressBool data (currently in ProgressData.xml)
	void ParseXML (XElement element)
	{
		foreach (var r in element.Elements("ProgressBool")) {
			ConvertToConditionDictionary (r.Element("Condition").Value, r.Element("Value").Value);
			ConvertToStoryDictionary (r.Element("Condition").Value, r.Element("script").Value);
		}
	}
}

