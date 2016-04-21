using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMapper : MonoBehaviour {

	public static string right { get; set;}
	public static string left { get; set;}
	public static string forward { get; set;}
	public static string backward { get; set;}

	static Dictionary<string, string> keymap = new Dictionary<string, string> {
		{"Left", "a"},
		{"Right", "d"},
		{"Forward", "w"},
		{"Backward", "s"},
		{"Use", "e"},
		{"Attack", "left mouse"}
	};

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static bool GetInput (string key) {
		if (keymap [key].Contains ("mouse"))
			return Input.GetMouseButton (0);
		return Input.GetKey (keymap [key]);
	}

	public static bool GetInputDown (string key) {
		if (keymap [key].Contains ("mouse")) {
			return Input.GetMouseButtonDown (0);
		}
		return Input.GetKeyDown (keymap [key]);
	}

	public static bool CheckKeyAvailable(string action, string key) {
		//Debug.Log (action + " ==> " + key);
		foreach (string s in keymap.Keys) {
			//Debug.Log (s + " ==> " + keymap [s]);
			if (s != action && keymap [s].Equals(key))
				return false;
		}
		return true;
	}

	public static void SetKey (string action, string key) {
		keymap [action] = key;
	}

	public static string GetKey (string action) {
		return keymap [action];
	}
}
