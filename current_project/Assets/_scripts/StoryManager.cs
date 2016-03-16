using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class StoryManager : MonoBehaviour {

	public Text storyText;

	private static StoryManager storyManager;

	// Make sure there is only one instance of this class
	void Awake () {
		if (storyManager == null) {
			DontDestroyOnLoad (gameObject);
			storyManager = this;
		}
		else if (storyManager != this) {
			Destroy (gameObject);
		}
	}

	public static StoryManager GetStoryManager() {
		return storyManager;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeText(string story) {
		storyText.text = story;
	}

	public void AddText(string story) {
		storyText.text += story;
	}
}
