using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{

    DialoguePanel panel;
    ProgressTracker tracker;
	ActiveAgent player;

    // Use this for initialization
    void Start()
    {
        panel = DialoguePanel.Instance();
        tracker = ProgressTracker.GetProgressTracker();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActiveAgent> ();
    }

    // Update is called once per frame
    void Update()
    {
		
    }

     void OnTriggerEnter(Collider other)
     {
		tracker.setBool ("atSign", true);
		player.Pause ();
     }

    void OnTriggerStay(Collider other)
    {
		if (Input.GetKeyDown(KeyCode.Space) && tracker.GetBool("atSign"))
        {
			if (!tracker.GetBool("signRead"))
				player.Pause ();
			tracker.setBool ("signRead", true);
        }
    }
}
