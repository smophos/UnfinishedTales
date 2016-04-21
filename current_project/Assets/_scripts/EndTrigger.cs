using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour {
	public Canvas endCanvas;

	void OnTriggerEnter (Collider other) {
		//endCanvas.gameObject.SetActive (true);
		//Time.timeScale = 0f;
		SceneManager.LoadScene("ClosingCredits");
	}
}
