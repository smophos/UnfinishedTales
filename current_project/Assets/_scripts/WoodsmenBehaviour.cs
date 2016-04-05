using UnityEngine;
using System.Collections;

public class WoodsmenBehaviour : ActiveAgent {

    DialoguePanel panel;
    ProgressTracker tracker;
    public GameObject axe;
	DialogueManager conversation;

    // Use this for initialization
    void Start()
    {
		ProgressTracker.RegisterObjective (gameObject, 0);
        tracker = ProgressTracker.GetProgressTracker();
		conversation = DialogueManager.GetDialogueManager ();
        panel = DialoguePanel.Instance();
    }
		
    void OnTriggerEnter(Collider other)
    {
		conversation.CreateAConversation (this);

		if (!tracker.GetBool ("woodsman_item")) {
			if (!tracker.GetBool ("woodsman_met")) {
				tracker.setBool ("woodsman_met", true);
			}
			//panel.ShowDialogue ("I have lost something very precious to me! Please, help me find it.");
		}
		if (tracker.GetBool("woodsman_item"))
        {
            //panel.ShowDialogue("Thank you! You can have my axe!");
			if (!tracker.GetBool ("bridgeItem")) {
				tracker.setBool ("bridgeItem", true);
				axe.SetActive (false);
			}
        }
    }

	public override string GetName () {
		return name;
	}

	public override void Pause () {
		paused = !paused;
	}

	public override void TakeDamage (int dmg){}
	public override void DealDamage (ActiveAgent agent){}
	protected override void Die () {}

    /*void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!tracker.GetBool("woodsmanItem"))
                panel.ShowDialogue("Get me that thing!");
            if (tracker.GetBool("woodsmanItem"))
                panel.ShowDialogue("Thanks for the thing!");
        }
    }*/
}
