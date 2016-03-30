using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour {

	private static DialogueManager dialogueManager;
	public Text dialogueText;
	public ActiveAgent player;
	float textDelay = 0.05f;
	float nextDelay = 2f;
	bool writing = false;

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
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDialogue (ActiveAgent talkitiveOne, Vector3 pos, string intitialText) {
		talkitiveOne.Pause ();
		player.Pause ();
		Vector3 screenPos = Camera.main.WorldToScreenPoint (pos);
		Debug.Log (screenPos);
		dialogueText.rectTransform.anchoredPosition = new Vector2 (screenPos.x, screenPos.y);
		dialogueText.text = intitialText;
	}

	public IEnumerator UpdateText (string text) {
		while (writing) {
			yield return new WaitForSeconds (textDelay);
		}
		writing = true;
		yield return new WaitForSeconds (nextDelay);
		dialogueText.text = "";
		foreach (char c in text) {
			dialogueText.text += c;
			yield return new WaitForSeconds (textDelay);
		}
		writing = false;
	}
}
