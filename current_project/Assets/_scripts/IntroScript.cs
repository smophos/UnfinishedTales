using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroScript : MonoBehaviour {

	AsyncOperation level;

    public Text introText;
	string intro1 = "Once upon a time, the brave knight Sir Galifrey journeyed to a great castle that once belonged to the noble Tillerton family. Now decrepit and abandoned, the Tillerton castle housed criminals, vagabonds, and wild beasts. An ancient dragon now nested behind those walls, however, keeping close guard over his prisoner, Princess Helena of Lyonshire.";
	string intro2 = "The New Order of Camelot responded to the pleas King Alfred of Lyonshire, sending Gelifrey to find and slay the dragon that held his daughter captive.\n\nAnd so our knight finds himself in a forest, Tillerton castle looming on the distant hillside, a constant reminder of his quest...";
	float charDelay = 0.05f;

    AudioSource fxSound;

    // Use this for initialization
    void Start () {

        fxSound = GetComponent<AudioSource>();
        fxSound.playOnAwake = true;
        StartCoroutine ("LoadLevel");
		StartCoroutine ("PlayIntro");

	}

	IEnumerator LoadLevel () {

		level = SceneManager.LoadSceneAsync ("scene01");
		level.allowSceneActivation = false;
		yield return level;

	}

	// Update is called once per frame
	void Update () {
		if (level != null) {
			if (InputMapper.GetInputDown("Use") && level.progress >= .9f)
				level.allowSceneActivation = true;
		}

		if (InputMapper.GetInputDown("Use")) {
			charDelay = 0f;
		}
	}

	IEnumerator PlayIntro() {
		foreach (char c in intro1) {
			if (charDelay == 0f) {
				introText.text = intro1;
				break;
			}
			introText.text += c;
			yield return new WaitForSeconds (charDelay);
		}

		charDelay = 0.05f;
		yield return new WaitForSeconds (1f);
		introText.text = "";

		foreach (char c in intro2) {
			if (charDelay == 0f) {
				introText.text = intro2;
				break;
			}
			introText.text += c;
			yield return new WaitForSeconds (charDelay);
		}
	}
}
