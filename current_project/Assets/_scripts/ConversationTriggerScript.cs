using UnityEngine;
using System.Collections;

public class ConversationTriggerScript : MonoBehaviour {

	public ActiveAgent fairy;

	// Use this for initialization
	void Start () {
	
	}
	
	 void OnTriggerEnter(Collider other)
     {
		DialogueManager.GetDialogueManager().CreateAConversation (fairy);
		ProgressTracker.GetProgressTracker ().setBool ("fairy_met", true);
     }
}
