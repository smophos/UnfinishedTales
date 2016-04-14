using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : ActiveAgent {

	// for motion
	Rigidbody rb;
	public Animator playerAnim;
    public GameObject sprite;
	Vector3 startPos, endPos;
	bool switching;
	float startTime = 0f, distance;
	public float speed = 1.0f;
	int lane = 1;
	public Transform lane_restriction_check;
    
    
    // for sound 
    private AudioSource source;
    public AudioClip WalkingSound, swordswipe1;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    float delay = 0f;

    private StoryManager manager;
	DialogueManager conversation;

	// fight related code
	List<ActiveAgent> enemiesInRange = new List<ActiveAgent>();
	List<ActiveAgent> toRemove = new List<ActiveAgent> ();
	bool inBattle = false;
    float leftBound = 0f, rightBound = 0f;
	float attackDelay = 0.0f;




    // Use this for initialization
    void Start () {

        

        name = "Player";
		attackRadius = 1.0f;
        manager = StoryManager.GetStoryManager();
		conversation = DialogueManager.GetDialogueManager ();
		rb = GetComponent<Rigidbody> ();


        source = GetComponent<AudioSource>();
   

        distance = 3.0f;
	}

	public void CheckForFoes () {
		enemiesInRange.Clear ();
		foreach (ActiveAgent enemy in ActiveAgent.Enemies) {
			if (Vector3.Distance (transform.position, enemy.gameObject.transform.position) < 1.0f)
				enemiesInRange.Add (enemy);
		}
	}

    public bool CheckNearbyFoes()
    {
        CheckForFoes();

        if (enemiesInRange.Count > 0)
            return true;
        else
            return false;


    }
	// Update is called once per frame
	void Update () {

		if (attackDelay > 0f)
			attackDelay -= Time.deltaTime;


        if (!paused) {

			if (Input.GetKeyDown(KeyCode.L) && ProgressTracker.GetProgressTracker().GetBool("swordFound"))
			{

				source.Stop();
				source.clip = swordswipe1;
				source.PlayOneShot(swordswipe1, 0.75f);
			}

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

    

            
                    
            // Fight related logic and controls
			if (Input.GetKeyDown(KeyCode.L) && attackDelay <= 0  && ProgressTracker.GetProgressTracker().GetBool("swordFound")) {

                attackDelay = 0.5f;
                playerAnim.SetTrigger("Attack");

                

                CheckForFoes ();
				if (enemiesInRange.Count > 0) {

					if (!inBattle) {
						inBattle = true;

						leftBound = transform.position.x - 3.0f;
						rightBound = transform.position.x + 3.0f;

						manager.ChangeText ("Battle!");
						//manager.ShowText ();
					} else {
						foreach (ActiveAgent enemy in enemiesInRange) {
							DealDamage (enemy);
							manager.ChangeText ("Dealt " + damage + " damage!");
							//manager.ShowText ();
							if (enemy.GetHealth () <= 0) {

                                toRemove.Add (enemy);
								enemy.gameObject.SetActive (false);
                                
                                manager.ChangeText ("You killed the wolf! Go collect your prize :)");
                                Debug.Log("We killed things");
                                manager.fxSound.PlayOneShot(manager.successsound, 0.75f);
                                //manager.ShowText ();
                                inBattle = false;
							}
						}
						foreach (ActiveAgent agent in toRemove) {
							enemiesInRange.Remove (agent);
							Destroy (agent);
						}
					}
				}
			}

			// Get horizontal player motion if correct keys are pressed
			float horizontal = Input.GetAxisRaw ("Horizontal");

			// Set animation to walking for player if movement is not 0
			playerAnim.SetBool ("isWalking", horizontal != 0);

			// Flip player to face correct direction
			if (horizontal < 0 && transform.localScale.x > 0)
				Flip ();
			if (horizontal > 0 && transform.localScale.x < 0)
				Flip ();
		
			move (horizontal);
		}
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
				//rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			}
		}
	}

	int GetLayerRestriction (string tag) {
		if (tag == "r1")
			return 1;
		else if (tag == "r2f")
			return 2;
		else if (tag == "r2b")
			return 3;
		return 0;
	}

	// Move player to next layer
	void SwitchLayer (float startTime, float distance, int dir) {

		Collider ground = null;
		var low_colliders = Physics.OverlapSphere (lane_restriction_check.position, 0.05f);
		foreach (var collider in low_colliders) {
			if (collider.gameObject.layer == 12) {
				ground = collider;
				break;
			}
		}

		if (ground == null)
			return;
		
		int restriction = GetLayerRestriction (ground.gameObject.tag);

		if (restriction == 1) {
		} else if (restriction == 2) {
			if ((lane != 2 && dir == 1) || (lane != 1 && dir == -1)) {
				switching = true;
				this.startTime = startTime;
				startPos = transform.position;
				endPos = transform.position + new Vector3 (0f, 0f, dir * distance);
			}
		} else if (restriction == 3) {
			if ((lane != 3 && dir == 1) || (lane != 2 && dir == -1)) {
				switching = true;
				this.startTime = startTime;
				startPos = transform.position;
				endPos = transform.position + new Vector3 (0f, 0f, dir * distance);
			}
		}

		// Check which direction player wants and which layer he/she is in to prevent falling off map
		else if (restriction == 0) {
			if ((lane != 3 && dir == 1) || (lane != 1 && dir == -1)) {
				switching = true;
				this.startTime = startTime;
				startPos = transform.position;
				endPos = transform.position + new Vector3 (0f, 0f, dir * distance);
			}
		}

		if (switching) {
			if (transform.position.z + dir * distance > -4.5 && transform.position.z + dir * distance <= -1.5)
				lane = 1;
			else if (transform.position.z + dir * distance > -1.5 && transform.position.z + dir * distance < 1.5)
				lane = 2;
			else if (transform.position.z + dir * distance >= 1.5 && transform.position.z + dir * distance < 4.5)
				lane = 3;
		}
	}

	// Control player motion
    void move(float dir)
    {
        if (inBattle)
        {
            if (transform.position.x >= rightBound && dir > 0)
                rb.velocity = Vector3.zero;
            if (transform.position.x <= leftBound && dir < 0)
                rb.velocity = Vector3.zero;
            else
                   rb.velocity = new Vector3(dir * speed, rb.velocity.y, 0f);
        }
        else {
            rb.velocity = new Vector3(dir * speed, rb.velocity.y, 0f);
            if (Mathf.Abs(dir) > 0)
            {
                delay -= Time.deltaTime;
                if (delay <= 0f)
                {
                    source.Stop();
                    source.clip = WalkingSound;
                    float vol = Random.Range(volLowRange, volHighRange);
                    source.PlayOneShot(WalkingSound, vol);
                    delay = 1f;
                }
            }
            else {
                if (source.clip != swordswipe1)
                    source.Stop();
                delay = 0f;
            }
        }
    }

	// Return player's speed
	public Vector2 getSpeed () {
		return new Vector2(speed, 0.0f);
	}

	// Flip player to face opposite direction
    void Flip ()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public override void Pause () {
		paused = !paused;
		if (paused) {
			source.Stop ();
			playerAnim.SetBool ("isWalking", false);
		}
	}

	override public void DealDamage (ActiveAgent enemy) {
		enemy.TakeDamage (damage);
	}
		
	override public void TakeDamage (int dmg) {
		health -= dmg;
	}

	override protected void Die () {

	}

//	public void SubscribeToConversation (Conversation conversation) {
//		this.conversation = conversation;
//	}
		
	public override string GetName () {
		return name;
	}
}
