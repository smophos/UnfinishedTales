using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

	private static DialogueManager dialogueManager;
	ActiveAgent player, speaker;
	Conversation conversation;
	bool conversing = false;

	void Awake () {
		if (dialogueManager == null) {
			DontDestroyOnLoad (gameObject);
			dialogueManager = this;
		}
		else if (dialogueManager != this) {
			Destroy (gameObject);
		}
	}

	public static DialogueManager GetDialogueManager () {
		return dialogueManager;
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActiveAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && conversing) {
			EndConversation ();
		}
	}

	public void CreateAConversation (ActiveAgent speaker, ActiveAgent listener) {
		conversation = new Conversation (speaker, listener);
		Begin ();
	}

	public void CreateAConversation (ActiveAgent speaker) {
		conversation = new Conversation (speaker, player);
		this.speaker = speaker;
		Begin ();
	}

	void Begin() {
		conversing = true;
		speaker.Pause();
		player.Pause ();
		conversation.Begin ();
		StoryManager.GetStoryManager().ChangeText(conversation.GetLastResponseText ());
		StoryManager.GetStoryManager().ShowText();
		StartCoroutine ("UpdateConversation");
	}

	IEnumerator UpdateConversation () {
		//Debug.Log("Here");
		while (conversation.Next ()) {
			// The two story manager parts are quick examples of how you might
			// connect the dialogue logic to a UI.
			// Obviously, this way was just for show and not too great :)
			StoryManager.GetStoryManager().ShowText();
			yield return new WaitForSeconds (3f);
			StoryManager.GetStoryManager().ChangeText(conversation.GetLastResponseText ());
		}
		EndConversation();
	}

	void EndConversation () {
		conversing = false;
		conversation = null;
		StopAllCoroutines ();
		speaker.Pause();
		player.Pause ();
		Debug.Log ("Conversation over");
	}
}
