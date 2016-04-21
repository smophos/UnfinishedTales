using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpeningScript : MonoBehaviour {

	Animator anim;
	AsyncOperation level;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		StartCoroutine ("LoadLevel");
	}

	IEnumerator LoadLevel () {

		level = SceneManager.LoadSceneAsync ("main_menu");
		level.allowSceneActivation = false;
		yield return level;

	}

	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("End"))
			//SceneManager.LoadScene ("main_menu");
			level.allowSceneActivation = true;
	}
}
