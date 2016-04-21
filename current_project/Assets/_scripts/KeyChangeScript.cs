using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class KeyChangeScript : MonoBehaviour, IPointerClickHandler {

	static bool ThereCanOnlyBeOne = false;

	InputField keyfield;
	bool waitForKey;
	string previous;
	string defaultKey;
	string lastConfirm;

	// Use this for initialization
	void Start () {
		keyfield = this.GetComponent<InputField> ();
		keyfield.interactable = false;
		waitForKey = false;


		switch (gameObject.name) {
		case "Right":
			defaultKey = "D";
			break;
		case "Left":
			defaultKey = "A";
			break;
		case "Forward":
			defaultKey = "W";
			break;
		case "Backward":
			defaultKey = "S";
			break;
		case "Use":
			defaultKey = "E";
			break;
		case "Attack":
			defaultKey = "Left Mouse";
			break;
		}

		lastConfirm = defaultKey;
		previous = defaultKey;

		keyfield.text = defaultKey;

		ControlPanelScript.ConfirmConfig += Confirm;
		ControlPanelScript.ResetConfig += Reset;
		ControlPanelScript.CancelConfig += Cancel;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForKey) {
			if (Input.anyKeyDown && Input.inputString.Length > 0) {
				foreach (var k in keys)
					if (Input.GetKeyDown (k)) {
						if (InputMapper.CheckKeyAvailable(gameObject.name, k))
							keyfield.text = k.ToUpper();
						else
							keyfield.text = previous;
						UpdateMapper ();
						waitForKey = false;
						KeyChangeScript.ThereCanOnlyBeOne = false;
					}
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (waitForKey) {
			if (InputMapper.CheckKeyAvailable(gameObject.name, eventData.button.ToString ().ToLower() + " mouse"))
				keyfield.text = eventData.button.ToString () + " Mouse";
			else
				keyfield.text = previous;
			UpdateMapper ();
			waitForKey = false;
			KeyChangeScript.ThereCanOnlyBeOne = false;
		}
		else if (!waitForKey && !ThereCanOnlyBeOne) {
			KeyChangeScript.ThereCanOnlyBeOne = true;
			previous = keyfield.text;
			keyfield.text = "Press any key...";
			waitForKey = true;
		}
	}
		
	void Reset () {
		keyfield.text = defaultKey;
		UpdateMapper ();
	}

	void Cancel () {
		keyfield.text = lastConfirm;
		UpdateMapper ();
	}

	void Confirm () {
		lastConfirm = keyfield.text;
		UpdateMapper ();
	}

	void UpdateMapper () {
//		switch (gameObject.name) {
//		case "Right":
//			InputMapper.SetKey(keyfield.text) = keyfield.text;
//			Debug.Log (InputMapper.right);
//			break;
//		case "Left":
//			InputMapper.left = keyfield.text;
//			Debug.Log (InputMapper.left);
//			break;
//		case "Forward":
//			InputMapper.forward = keyfield.text;
//			Debug.Log (InputMapper.forward);
//			break;
//		case "Backward":
//			InputMapper.backward = keyfield.text;
//			Debug.Log (InputMapper.backward);
//			break;
//		}

		InputMapper.SetKey (gameObject.name, keyfield.text.ToLower());
	}

	string[] keys = {
		"backspace",
		"delete",
		"tab",
		"clear",
		"return",
		"pause",
		"escape",
		"space",
		"up",
		"down",
		"right",
		"left",
		"insert",
		"home",
		"end",
		"page up",
		"page down",
		"f1",
		"f2",
		"f3",
		"f4",
		"f5",
		"f6",
		"f7",
		"f8",
		"f9",
		"f10",
		"f11",
		"f12",
		"f13",
		"f14",
		"f15",
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9",
		"!",
		"\"",
		"#",
		"$",
		"&",
		"'",
		"(",
		")",
		"*",
		"+",
		",",
		"-",
		".",
		"/",
		":",
		";",
		"<",
		"=",
		">",
		"?",
		"@",
		"[",
		"\\",
		"]",
		"^",
		"_",
		"`",
		"a",
		"b",
		"c",
		"d",
		"e",
		"f",
		"g",
		"h",
		"i",
		"j",
		"k",
		"l",
		"m",
		"n",
		"o",
		"p",
		"q",
		"r",
		"s",
		"t",
		"u",
		"v",
		"w",
		"x",
		"y",
		"z",
		"numlock",
		"caps lock",
		"scroll lock",
		"right shift",
		"left shift",
		"right ctrl",
		"left ctrl",
		"right alt",
		"left alt"
	};
}
