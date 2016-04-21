using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsButton : MonoBehaviour {
	Button button;

	// Use this for initialization
	void Start () {
		button = gameObject.GetComponent<Button> ();

		if (gameObject.name == "Confirm" || gameObject.name == "Cancel") {
			button.onClick.AddListener (delegate {
				PauseMenuController.GetMenuController ().Options ();
			});
		}

		if (gameObject.name == "Confirm") {
			button.onClick.AddListener (delegate {
				ControlPanelScript.Confirm ();
			});
		}

		if (gameObject.name == "Reset") {
			button.onClick.AddListener (delegate {
				ControlPanelScript.Reset ();
			});
		}

		if (gameObject.name == "Cancel") {
			button.onClick.AddListener (delegate {
				ControlPanelScript.Cancel ();
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
