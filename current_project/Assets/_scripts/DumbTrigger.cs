using UnityEngine;
using System.Collections;

public class DumbTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log ("Enter");
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("Player");
			other.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3(25f,25f,0f));
		}
	}
}
