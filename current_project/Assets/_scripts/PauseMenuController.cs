using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

// Controls the in-game pause menu
// Place in every scene
// IMPORTANT: Need Pause Menu prefab in assets for this to work

public class PauseMenuController : MonoBehaviour {

	private static PauseMenuController menuController;
	public Canvas canvas;
	GameObject pausePanel, optionsPanel;
	string name1;
	public bool gamePaused = false;
	bool inOptions = false;

	public delegate void PauseAction();
	public static event PauseAction Pause;
	public static event PauseAction Cancel;
	//public AudioMixerSnapshot paused, unpaused;

	// Make sure there is only one instance of this class, initiate pause canvase, default canvase to disabled
	void Awake () {
		if (menuController == null) {
			DontDestroyOnLoad (gameObject);
			canvas = Instantiate (canvas);
			optionsPanel = GameObject.FindWithTag ("Options");
			name1 = optionsPanel.name;
			Debug.Log (optionsPanel.name);
			optionsPanel.SetActive (false);
			DontDestroyOnLoad (canvas);
			canvas.enabled = false;
			menuController = this;
		}
		else if (menuController != this) {
			Destroy (gameObject);
		}
	}

	void Start () {

	}

	// Check if escape key is pressed. If so, enable pause canvas and call pause game. Can also change cursor visibility as necessary
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && !(SceneManager.GetActiveScene ().name == "main_menu"))
		{
			canvas.enabled = !canvas.enabled;
			Debug.Log (optionsPanel);
			//Cursor.visible = !Cursor.visible;
			if (inOptions) {
				optionsPanel.SetActive (false);
				pausePanel.SetActive (true);
				inOptions = false;
				Cancel ();
			}

			PauseGame ();
		}
	}

	// Returns the single, static PauseMenuController object
	public static PauseMenuController GetMenuController () {
		return menuController;
	}

	// Sets timescale and cursor lock state as appropriate for pause/unpause
	// Also, can add LowPass filter for game music while in menu
	void PauseGame () {
		// Debug.Log (Cursor.lockState);
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		gamePaused = !gamePaused;
		Pause();

		/*if (Time.timeScale == 0) 
		{
			Cursor.lockState = CursorLockMode.Confined;
		} 

		else 
		{
			Cursor.lockState = CursorLockMode.Locked;
		}*/
		// LowPass ();
	}

	/*void LowPass () {
		if (Time.timeScale == 0) {
			paused.TransitionTo (0.1f);
		} 

		else {
			unpaused.TransitionTo (0.1f);
		}
	}*/

	// Save code here
	public void Save () {

	}

	// Load code here
	public void Load () {
		Debug.Log (optionsPanel);
	}

	// Options menu code here
	public void Options () {
		Debug.Log (name1);
		if (pausePanel == null) {
			pausePanel = GameObject.FindWithTag ("Pause");
		}

		if (inOptions) {
			optionsPanel.SetActive (false);
			pausePanel.SetActive (true);
			inOptions = false;
		} else {
			pausePanel.SetActive (false);
			optionsPanel.SetActive (true);
			inOptions = true;
		}
	}

	// Calls GameController.Quit() --- later will ask if want to quit to main menu or desktop
	public void Quit () {
		GameController.GetController ().Quit ();
	}
}