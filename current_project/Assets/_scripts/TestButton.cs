using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestButton : MonoBehaviour {
	Button button;

	// Use this for initialization
	void Start () {
		GameObject optionsPanel = GameObject.Find ("Controls Panel");
		Debug.Log (optionsPanel);
		button = gameObject.GetComponent<Button> ();
		button.onClick.AddListener(delegate {
			PauseMenuController.GetMenuController().Options();
		});
	}
	
	// Update is called once per frame
	void Update () {
	}
}
