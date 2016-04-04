using UnityEngine;
using System.Collections;


/// <summary>
/// Camera Trigger is the script for CameraTrigger prefabs. These are scalable box colliders that
/// allow certain areas to affect camera movement by setting an x-axis point constraint on the
/// main camera (cameraController). Place one at beginning of zone and one at end to work properly
/// </summary>
public class CameraTrigger : MonoBehaviour {

	public float xStopPosition;
	public static bool[] enter;              // Is the player in the zone specified by setNumber?
	public CameraControl cameraController;   // Camera to constrain
	public int setNumber = 0;                // Which of twenty available sets of CameraTriggers is this a part of?

	// Default enter variables to false; player has not entered any zones at spawn
	void Awake () {
		enter = new bool[20];
		for (int i = 0; i < 20; i++) {
			enter [i] = false;
		}
	}

	// Use this for initialization
	void Start () {
		//setNumber = 0;
	}

	// If player enters or exits a zone (either way they enter a trigger zone first), 
	// change enter bool for this CameraTrigger's setNumber and set camera constraint -
	// or unlock if player left.
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			enter[setNumber] = !enter[setNumber];
			if (enter[setNumber])
				cameraController.SetCameraConstraint (xStopPosition);
			else
				cameraController.UnlockCamera();
		}
	}
}
