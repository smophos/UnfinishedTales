using UnityEngine;
using System.Collections;

public class SignTrigger : MonoBehaviour {

    DialoguePanel panel;

	// Use this for initialization
	void Start () {
        panel = DialoguePanel.Instance();
	}
	
	// Update is called once per frame
	void Update () {
	
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
            panel.ShowDialogue("Bridge Ahead!!!");
        }
    }
}
