using UnityEngine;
using System.Collections;

// Controls camera tracking of player and other camera features, such as map bounds and CameraTrigger constraints

public class CameraControl : MonoBehaviour {

	public Vector2 offset;
	public PlayerController playerC; // Player
	public Transform mapBegin; // Beginning of map, don't move camera view to left of this point
	//public Transform mapTop; // Top of map, don't move camera view above this
	private Transform cameraT; // The camera's transform

	private Camera main; // variable for main camera instead of using Camera.main multiple times
	private float posX, posY;
	private float minx;
	private float speedX, speedY; // Speed to follow on x and y axes, respectively

	private float cameraXStop;
	private bool cameraConstrained = false;

	// Set camera initial position, speed, and other initialization
	void Awake () {
		cameraT = GetComponent<Transform> ();
		cameraT.position = new Vector3(playerC.transform.position.x+offset.x, playerC.transform.position.y+offset.y, cameraT.position.z);
		main = Camera.main;
		//minx = mapStart.position.x;
		speedX = 10.0f;
		speedY = 10.0f;
		cameraXStop = 0.0f;
	}
	
	// Move camera toward player
	void FixedUpdate () {

		// Set speed of camera tracking and update camera x and y coordinates
		speedX = Mathf.Abs (playerC.getSpeed ().x);
		speedY = Mathf.Abs (playerC.getSpeed ().y);
		posX = moveTowards(transform.position.x, playerC.transform.position.x, speedX);
		posY = moveTowards(transform.position.y, playerC.transform.position.y+offset.y, speedX);

		// If camera will have moved past the start of the map, reset to last x position to prevent this
		//if (main.WorldToScreenPoint (new Vector3 (posX, 0, 0)).x - main.pixelRect.width / 2f < main.WorldToScreenPoint (mapBegin.position).x) {
		if (posX <= mapBegin.position.x) {	
			posX = cameraT.position.x;
		}

		else if (cameraConstrained && Mathf.Abs(posX) >= Mathf.Abs(cameraXStop)) {	
			posX = cameraT.position.x;
		}

		// If camera will have moved above top of map, reset to last y position to prevent this
	//	if (main.WorldToScreenPoint (new Vector3 (0, posY, 0)).y + main.pixelRect.height / 2f > main.WorldToScreenPoint (mapTop.position).y) {
	//		posY = cameraT.position.y;
	//	}

		// Set camera to new position
		cameraT.position = new Vector3 (posX, posY, cameraT.position.z);
	}

	// Set x position that camera cannot move past
	public void SetCameraConstraint (float xStopPos) {
		cameraXStop = xStopPos;
		cameraConstrained = true;
	}

	// Set cameraConstrained to false to prevent extra constraints
	public void UnlockCamera () {
		cameraConstrained = false;
	}

	// Get the current camera constraint - default is 0.0f for simplicity
	public float GetCameraConstraint () {
		return cameraXStop;
	}

	// Return if camera is constrained
	public bool CameraConstrained () {
		return cameraConstrained;
	}

	// Found this code in a YouTube tutorial
	// f: x position of camera
	// t: x position of target
	// speed: speed to approach
	float moveTowards (float f, float t, float speed) {
		if (f == t) {
			return f;
		} else {
			float dir = Mathf.Sign (t - f); // direction to target
			f += speed * dir;
			return (dir == Mathf.Sign (t - f)) ? f : t; // Checks to see if we passed target on last calculation; if so, return target position
		}
	}
}
