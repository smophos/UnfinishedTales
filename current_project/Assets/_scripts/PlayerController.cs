using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// for motion
	Rigidbody rb;
	public Animator playerAnim;
	public GameObject sprite;
	Vector3 startPos, endPos;
	bool switching;
	float startTime = 0f, distance;
	public float speed = 1.0f;
    
    
    // for sound 
    private AudioSource source;
    public AudioClip WalkingSound;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    float delay = 0f;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
        source = GetComponent<AudioSource>();
        distance = 3.0f;

	}
	
	// Update is called once per frame
	void Update () {

		// Player movement back one layer
		if (Input.GetKeyDown (KeyCode.W) && !switching) {
			SwitchLayer (Time.time, distance, 1);
		}

		// Player movement forward one layer
		if (Input.GetKeyDown (KeyCode.S) && !switching) {
			SwitchLayer (Time.time, distance, -1);
		}

		// Show the story text through its animation again
		if (Input.GetKeyDown (KeyCode.F)) {
			StoryManager.GetStoryManager ().ShowText ();
		}

		// Get horizontal player motion if correct keys are pressed
		float horizontal = Input.GetAxisRaw ("Horizontal");

		// Set animation to walking for player if movement is not 0
		playerAnim.SetBool ("isWalking", horizontal != 0);

		// Flip player to face correct direction
        if (horizontal < 0 && transform.localScale.x > 0)
            Flip();
        if (horizontal > 0 && transform.localScale.x < 0)
            Flip();
		
        move (horizontal);
	}

	void FixedUpdate () {

		// Perform vector lerp to new layer if switching is true, i.e. if player has
		// switched to an allowable layer
		if (switching) {
			float distanceCovered = (Time.time - startTime) * speed;
			float distFrac = distanceCovered / distance;
			transform.position = Vector3.Lerp (startPos, endPos, distFrac);

			if (distFrac >= 1.0f) {
				switching = false;
			}
		}
	}

	// Move player to next layer
	void SwitchLayer (float startTime, float distance, int dir) {

		// Check which direction player wants and which layer he/she is in to prevent falling off map
		if ((transform.position.z < 3f && dir == 1) || (transform.position.z > -3f && dir == -1)) {
			switching = true;
			this.startTime = startTime;
			startPos = transform.position;
			endPos = transform.position + new Vector3 (0f, 0f, dir * distance);
		}
	}

	// Control player motion
    void move(float dir)
    {
        rb.velocity = new Vector3(dir * speed, rb.velocity.y, 0f);
        if (Mathf.Abs(dir) > 0)
        {
            delay -= Time.deltaTime;
            if (delay <= 0f)
            {
                source.Stop();
                float vol = Random.Range(volLowRange, volHighRange);
                source.PlayOneShot(WalkingSound, vol);
                delay = 1f;
            }
        }
        else {
            source.Stop();
            delay = 0f;
        }
    }

	// Return player's speed
	public Vector2 getSpeed() {
		return new Vector2(speed, 0.0f);
	}

	// Flip player to face opposite direction
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
