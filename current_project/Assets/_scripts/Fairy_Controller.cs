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
	Vector3 dialoguePosition, playerDialogue;

	// Use this for initialization
	void Start () {
		verticalDistanceToFairy = Mathf.Abs (transform.position.y - fairy.position.y);
		followPlayer = false;
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (goal.position);
		ProgressTracker.ObjectiveChanged += UpdateGoal;

		dialogue = DialogueManager.GetDialogueManager ();
		dialoguePosition = transform.position + new Vector3 (0f, verticalDistanceToFairy + 1f, 0f);
		playerDialogue = player.position + new Vector3 (0f, 1.5f, 0f);
		dialogue.StartDialogue (this, dialoguePosition, "Hello, my name is Unfinished Tales!");
		dialogue.UpdateText ("I need your assistance!", false, dialoguePosition);
		dialogue.UpdateText ("How may I be of service?", false, playerDialogue);
		dialogue.UpdateText ("I am working on chronicling...", false, dialoguePosition);
		dialogue.UpdateText ("your quest to confront the dragon.", false, dialoguePosition);
		dialogue.UpdateText ("You must help me find...", false, dialoguePosition);
		dialogue.UpdateText ("the missing details of your tale.", false, dialoguePosition);
		dialogue.UpdateText ("What missing details?", false, playerDialogue);
		dialogue.UpdateText ("I have yet to find the dragon!", false, playerDialogue);
		dialogue.UpdateText ("How is it you know so much about me?", false, playerDialogue);
		dialogue.UpdateText ("I will explain everything in time.", false, dialoguePosition);
		dialogue.UpdateText ("For now, there is work to do.", false, dialoguePosition);
		dialogue.UpdateText ("Let's go!", true, dialoguePosition);
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

	public override void TakeDamage (int dmg){}
	public override void DealDamage (ActiveAgent agent){}
	protected override void Die () {}
}
