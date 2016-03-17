using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;


/// <summary>
/// StoryManager is the class to use for updating, adding to, and showing story text
/// It is singleton, so use static GetStoryManager() to get instance for other classes 
/// </summary>
public class StoryManager : MonoBehaviour {

	public Text storyText;
	public Animator story_anim;

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

	// Return single StoryManager instance
	public static StoryManager GetStoryManager () {
		return storyManager;
	}

	// Update is called once per frame
	void Update () {
	
	}

	// Change story text to specified string
	public void ChangeText (string story) {
		storyText.text = story;
	}

	// Append specified text to end of current story text
	public void AddText (string story) {
		storyText.text += story;
	}

	// Show story text by playing fade in and out animation
	public void ShowText () {
		story_anim.SetTrigger ("Play");
	}

	/*public void TextVisible (bool cond) {
		story_anim.StopPlayback ();
		if (cond)
			storyText.color = new Color (storyText.color.r, storyText.color.g, storyText.color.b, 1f);
		else
			storyText.color = new Color (storyText.color.r, storyText.color.g, storyText.color.b, 0f);
	}*/
}
