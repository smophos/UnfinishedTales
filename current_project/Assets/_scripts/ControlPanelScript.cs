using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlPanelScript : MonoBehaviour {

	public delegate void ControlPanelAction();
	public static event ControlPanelAction ConfirmConfig;
	public static event ControlPanelAction ResetConfig;
	public static event ControlPanelAction CancelConfig;

	// Use this for initialization
	void Start () {
		PauseMenuController.Cancel += Cancel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void Confirm () {
		ProgressTracker.GetProgressTracker ().ReParseStoryXML ();
		ConfirmConfig ();
	}

	public static void Reset () {
		ResetConfig ();
	}

	public static void Cancel () {
		CancelConfig ();
	}
}
