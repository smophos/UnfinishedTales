using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;


/// <summary>
/// StoryManager is the class to use for updating, adding to, and showing story text
/// It is singleton, so use static GetStoryManager() to get instance for other classes 
/// </summary>
public class StoryManager : MonoBehaviour {

	public Canvas storyCanvas;
	public Text storyText, dialogue;
	public Animator story_anim, dialogue_anim;


	private static StoryManager storyManager;
    public AudioSource fxSound;
    public AudioClip successsound;

    // Make sure there is only one instance of this class
    void Awake () {

		if (storyManager == null) {
			DontDestroyOnLoad (gameObject);
			storyManager = this;
            fxSound = GetComponent<AudioSource>();
           
        }
		else if (storyManager != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		PauseMenuController.Pause += Pause;
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
	// This is for the fading animation --- not permanent
	public void ShowText () {
		story_anim.SetTrigger ("Play");
	}

	// Toggles visibility of text
	public void ToggleText () {
		storyText.gameObject.SetActive (!storyText.gameObject.activeSelf);
	}

	// Change dialogue text to specified string
	public void ChangeDialogueText (string story) {
		dialogue.text = story;
	}

	// Show dialogue text by playing fade in and out animation
	// This is for the fading animation --- not permanent
	public void ShowDialogueText () {
		dialogue_anim.SetTrigger ("Play");
	}

	public Animator GetDialogueAnimator() {
		return dialogue_anim;
	}

	public void StopAnimator() {
		dialogue_anim.StopPlayback ();
	}

	void Pause () {
		storyCanvas.enabled = !storyCanvas.enabled;
	}
}
