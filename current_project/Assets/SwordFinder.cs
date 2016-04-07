using UnityEngine;
using System.Collections;

public class SwordFinder : MonoBehaviour
{

    DialoguePanel panel;
    ProgressTracker tracker;

    // Use this for initialization
    void Start()
    {
        panel = DialoguePanel.Instance();
        tracker = ProgressTracker.GetProgressTracker();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* void OnTriggerEnter(Collider other)
     {
             panel.ShowDialogue("Bridge Ahead!!!");
     }*/

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            panel.ShowDialogue("found your sword!!!");
        }
    }
}
