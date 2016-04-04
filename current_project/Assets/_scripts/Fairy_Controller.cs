using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Fairy_Controller : ActiveAgent {

	NavMeshAgent agent;
	public Transform goal;
	public Transform player;
	bool followPlayer;
	public Text chatText;
	public Transform fairy;
	float verticalDistanceToFairy;
	DialogueManager conversation;

	// Use this for initialization
	void Start () {
		name = "Book";
		verticalDistanceToFairy = Mathf.Abs (transform.position.y - fairy.position.y);
		followPlayer = false;
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (goal.position);
		ProgressTracker.ObjectiveChanged += UpdateGoal;
		conversation = DialogueManager.GetDialogueManager ();
		conversation.CreateAConversation (this);
		ProgressTracker.GetProgressTracker ().setBool ("fairy_met", true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			if (!followPlayer) {
				agent.stoppingDistance = 0.5f;
				if (Vector3.Distance (transform.position, player.position) > 5.0f)
					agent.Stop ();
				else if (Vector3.Distance (transform.position, player.position) <= 5.0f && agent.velocity == Vector3.zero)
					agent.Resume ();
			} else {
				if (agent.velocity == Vector3.zero)
					agent.SetDestination (goal.position);
				agent.stoppingDistance = 1.5f;
			}
		}
	}

	void UpdateGoal (Transform goal) {
		this.goal = goal;
		if (goal.position == player.position)
			followPlayer = true;
		else
			followPlayer = false;
		agent.SetDestination (goal.position);
	}

	public override void Pause () {
		paused = !paused;
		if (paused)
			agent.Stop ();
		else
			agent.Resume ();
	}

//	public void SubscribeToConversation (Conversation conversation) {
//
//	}

	public override string GetName () {
		return name;
	}

	public override void TakeDamage (int dmg){}
	public override void DealDamage (ActiveAgent agent){}
	protected override void Die () {}
}
