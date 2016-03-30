using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
		//transform.LookAt (Camera.main.transform, Vector3.up);
	}
}
