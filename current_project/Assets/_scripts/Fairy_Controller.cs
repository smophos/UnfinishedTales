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
	DialogueManager dialogue;
	public Transform fairy;
	float verticalDistanceToFairy;

	// Use this for initialization
	void Start () {
		verticalDistanceToFairy = Mathf.Abs (transform.position.y - fairy.position.y);
		followPlayer = false;
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (goal.position);
		ProgressTracker.ObjectiveChanged += UpdateGoal;

		dialogue = DialogueManager.GetDialogueManager ();
		dialogue.StartDialogue (this, transform.position + new Vector3 (0f, verticalDistanceToFairy + 1f, 0f), "Hello, my name is Unfinished Tales!");
		StartCoroutine(dialogue.UpdateText ("We have some work to do!"));
		StartCoroutine(dialogue.UpdateText ("Let's go!"));
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
		agent.Stop ();
	}

	public override void TakeDamage (int dmg){}
	public override void DealDamage (ActiveAgent agent){}
	protected override void Die () {}
}
