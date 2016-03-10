using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class DialoguePanel : MonoBehaviour {

	public Text dialogueText;
	public Button confirm;
	public GameObject dialoguePanelObject;

	private static DialoguePanel dialoguePanel;

	public static DialoguePanel Instance () {
		if (!dialoguePanel) {
			dialoguePanel = FindObjectOfType(typeof (DialoguePanel)) as DialoguePanel;
			if (!dialoguePanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}

		return dialoguePanel;
	}

	public void ShowDialogue (string message) {
		dialoguePanelObject.SetActive (true);

		confirm.onClick.RemoveAllListeners ();
		confirm.onClick.AddListener (ClosePanel);

		dialogueText.text = message;

		confirm.gameObject.SetActive (true);
	}

	void ClosePanel () {
		dialoguePanelObject.SetActive (false);
	}
}
