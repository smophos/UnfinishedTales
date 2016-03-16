using UnityEngine;
using System.Collections;

public class WoodsmenBehaviour : MonoBehaviour {

    DialoguePanel panel;
    ProgressTracker tracker;
    public GameObject axe;

    // Use this for initialization
    void Start()
    {
        tracker = ProgressTracker.GetProgressTracker();
        panel = DialoguePanel.Instance();
    }
		
    void OnTriggerEnter(Collider other)
    {
		if (!tracker.GetBool ("woodsmanItem")) {
			if (!tracker.GetBool ("woodsmanChat")) {
				tracker.setBool ("woodsmanChat", true);
			}
			panel.ShowDialogue ("I have lost something very precious to me! Please, help me find it.");
		}
        if (tracker.GetBool("woodsmanItem"))
        {
            panel.ShowDialogue("Thank you! You can have my axe!");
			if (!tracker.GetBool ("bridgeItem")) {
				tracker.setBool ("bridgeItem", true);
				axe.SetActive (false);
			}
        }
    }

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
