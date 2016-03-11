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

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!tracker.GetBool("woodsmanItem"))
            panel.ShowDialogue("Get me that thing!");
        if (tracker.GetBool("woodsmanItem"))
        {
            panel.ShowDialogue("Thanks for the thing! And my axe!");
            tracker.setBool("bridgeItem", true);
            axe.SetActive(false);
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
