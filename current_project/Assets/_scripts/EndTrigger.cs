using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTrigger : MonoBehaviour {
	public Canvas endCanvas;

	void OnTriggerEnter (Collider other) {
		endCanvas.gameObject.SetActive (true);
		Time.timeScale = 0f;
	}
}
