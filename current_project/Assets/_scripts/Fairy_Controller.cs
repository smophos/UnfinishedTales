using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Fairy_Controller : MonoBehaviour {

	NavMeshAgent agent;
	public Transform goal;
	public Transform player;

	bool followPlayer;

	// Use this for initialization
	void Start () {
		followPlayer = false;
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (goal.position);
		ProgressTracker.ObjectiveChanged += UpdateGoal;
	}
	
	// Update is called once per frame
	void Update () {
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

	void UpdateGoal (Transform goal) {
		this.goal = goal;
		if (goal.position == player.position)
			followPlayer = true;
		else
			followPlayer = false;
		agent.SetDestination (goal.position);
	}
}
