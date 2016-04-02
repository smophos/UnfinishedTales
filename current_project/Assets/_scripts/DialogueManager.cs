using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

	private static DialogueManager dialogueManager;
	public Text dialogueText;
	public ActiveAgent player;
	private ActiveAgent speaker;
	float textDelay = 0.04f;
	float nextDelay = 1.5f;
	bool writing;
	bool finished;
	Vector3 currentPos;
	List<Vector3> positions = new List<Vector3>();

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
		finished = false;
		writing = false;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void UpdatePosition(Vector3 pos) {
		Vector3 screenPos = Camera.main.WorldToScreenPoint (pos);
		dialogueText.rectTransform.anchoredPosition = new Vector2 (screenPos.x, screenPos.y);
		Debug.Log (dialogueText.rectTransform.anchoredPosition);
	}

	public void StartDialogue (ActiveAgent talkitiveOne, Vector3 pos, string intitialText) {
		currentPos = pos;
		speaker = talkitiveOne;
		talkitiveOne.Pause ();
		player.Pause ();
		Vector3 screenPos = Camera.main.WorldToScreenPoint (pos);
		Debug.Log (screenPos);
		dialogueText.rectTransform.anchoredPosition = new Vector2 (screenPos.x, screenPos.y);
		dialogueText.text = intitialText;
	}

	public void EndDialogue(ActiveAgent talkitiveOne) {
		if (finished) {
			talkitiveOne.Pause ();
			player.Pause ();
			dialogueText.text = "";
			speaker = null;
		}
	}

	public void UpdateText (string text, bool isDone, Vector3 pos) {
		positions.Add (pos);
		StartCoroutine (UpdateText (text, isDone));
	}


	public IEnumerator UpdateText (string text, bool isDone) {
			while (writing) {
				yield return new WaitForSeconds (textDelay);
			}
			writing = true;
			yield return new WaitForSeconds (nextDelay);
			UpdatePosition (positions[0]);
			positions.RemoveAt (0);
			dialogueText.text = "";
			foreach (char c in text) {
				dialogueText.text += c;
				yield return new WaitForSeconds (textDelay);
			}
			writing = false;
			finished = isDone;

			if (finished)
				EndDialogue (speaker);
	}
}
