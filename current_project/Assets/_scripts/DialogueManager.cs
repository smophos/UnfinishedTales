using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

	// Dialogue variables
	private static DialogueManager dialogueManager;
	ActiveAgent player, speaker; // Player and NPC speaker variables
	Conversation conversation;
	bool conversing = false;

	// Wake up and make sure there's still only one DialogueManager today
	void Awake () {
		if (dialogueManager == null) {
			DontDestroyOnLoad (gameObject);
			dialogueManager = this;
		}
		else if (dialogueManager != this) {
			Destroy (gameObject);
		}
	}

	// Get the single, static instance of DialogueManager
	public static DialogueManager GetDialogueManager () {
		return dialogueManager;
	}

	// Use this for initialization
	void Start () {
		// Get the Player object's ActiveAgent component at the start
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActiveAgent> ();
	}

	// Update is called once per frame
	void Update () {
		// If the player left clicks, end the conversation
		if (InputMapper.GetInputDown("Use") && conversing) {
			EndConversation ();
		}
	}

	public void CreateAConversation (ActiveAgent speaker, ActiveAgent listener) {
		conversation = new Conversation (speaker, listener);
		Begin ();
	}

	// Use this as the interface to create a conversation between speaker and Player
	public void CreateAConversation (ActiveAgent speaker) {
		conversation = new Conversation (speaker, player);
		this.speaker = speaker;
		Begin ();
	}

	// Private method to begin conversation and UpdateConversation coroutine
	void Begin() {
		conversing = true;
		speaker.Pause();
		player.Pause ();
		conversation.Begin ();
		StartCoroutine ("UpdateConversation");
	}

	// Coroutine for updating the conversation. Shows text and waits for animation to change to next text
	IEnumerator UpdateConversation () {
		//Debug.Log("Here");
		StoryManager.GetStoryManager().ChangeDialogueText(conversation.GetLastResponseText ());
		while (conversation.Next ()) {

			// Get the singleton story manager instance and show the dialogue text
			StoryManager.GetStoryManager().ShowDialogueText();

			// Wait for the dialogue animation to finish before changing the text
			yield return new WaitWhile (() => StoryManager.GetStoryManager().GetDialogueAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
			yield return new WaitWhile (() => StoryManager.GetStoryManager().GetDialogueAnimator().GetCurrentAnimatorStateInfo(0).IsName("dialogue_text_anim"));

			// Get the singleton story manager instance and change the dialogue text
			StoryManager.GetStoryManager().ChangeDialogueText(conversation.GetLastResponseText ());
		}
		EndConversation();
	}

	// Clean up to end conversation and upause characters
	void EndConversation () {
		conversing = false;
		conversation = null;
		StopAllCoroutines ();
		speaker.Pause();
		player.Pause ();
		Debug.Log ("Conversation over");
	}
}
