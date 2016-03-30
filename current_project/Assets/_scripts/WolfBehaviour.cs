using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfBehaviour : ActiveAgent {
 
    public Transform player, patrol_left, patrol_right;
    public Animator anim;
    public Animation growlAnim;
    NavMeshAgent agent;
    bool growled = false, idling = true;
    private int goingForwards;
    private Vector3 wolfPos;
    float xPos;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        wolfPos = agent.transform.position;
		Enemies.Add (this);
    }
	
	// Update is called once per frame
	void Update () {

        agent.autoBraking = false;

        if (idling == true)
        {
            anim.SetBool("isWalking", true);
            if (goingForwards == 0 && xPos < patrol_right.position.x)
                xPos = transform.position.x + Time.deltaTime * 3;
            else
            {
                goingForwards = 1;
                if (transform.localScale.x == 1)
                    Flip();
            }
            if (goingForwards == 1 && xPos > patrol_left.position.x)
                xPos = transform.position.x - Time.deltaTime * 3;
            else
            {
                goingForwards = 0;
                if (transform.localScale.x == -1)
                    Flip();

            }

            wolfPos = new Vector3(Mathf.Clamp(xPos, patrol_left.position.x, patrol_right.position.x), wolfPos.y, wolfPos.z);
            transform.position = wolfPos;
        }

        if (Vector3.Distance(agent.transform.position, player.position) < 5.0f)
        {

            idling = false;


            if (!growled)
            {
                anim.SetTrigger("Growl");
                growled = true;
            }
            else
            {
                //anim.SetTrigger("Growl");

               // Vector3 direction = (player.position - transform.position).normalized;
                // Quaternion lookRotation = Quaternion.LookRotation(direction);
                // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

                anim.SetBool("isWalking", true);
                agent.SetDestination(player.position);
            }

        }else if (Vector3.Distance(agent.transform.position, player.position) < 1.0f)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("isWalking", false);
            growled = false;
        }
        else
        {

        }

        
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public override void Pause () {
		paused = !paused;
	}

	override public void DealDamage (ActiveAgent enemy) {
		enemy.TakeDamage (damage);
	}

	override public void TakeDamage(int dmg) {
		health -= dmg;
		if (health <= 0)
			Die ();
	}

	override protected void Die () {
		Enemies.Remove (this);
	}
}
