using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
	public Animator playerAnim;
	public GameObject sprite;
	Vector3 startPos, endPos;
	bool switching;
	float startTime = 0f, distance;
	public float speed = 1.0f;
    
    
    //for sound 
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
		if (Input.GetKeyDown (KeyCode.W) && !switching) {
			SwitchLayer (Time.time, distance, 1);
		}

		if (Input.GetKeyDown (KeyCode.S) && !switching) {
			SwitchLayer (Time.time, distance, -1);
		}

		float horizontal = Input.GetAxisRaw ("Horizontal");
		playerAnim.SetBool ("isWalking", horizontal != 0);
        if (horizontal < 0 && transform.localScale.x > 0)
            Flip();
        if (horizontal > 0 && transform.localScale.x < 0)
            Flip();

        move (horizontal);
	}

	void FixedUpdate () {
		if (switching) {
			float distanceCovered = (Time.time - startTime) * speed;
			float distFrac = distanceCovered / distance;
			transform.position = Vector3.Lerp (startPos, endPos, distFrac);

			if (distFrac >= 1.0f) {
				switching = false;
			}
		}
	}

	void SwitchLayer (float startTime, float distance, int dir) {
		if ((transform.position.z < 3f && dir == 1) || (transform.position.z > -3f && dir == -1)) {
			switching = true;
			this.startTime = startTime;
			startPos = transform.position;
			endPos = transform.position + new Vector3 (0f, 0f, dir * distance);
		}
	}

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

	public Vector2 getSpeed() {
		return new Vector2(speed, 0.0f);
	}

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
