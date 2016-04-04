using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ActiveAgent : MonoBehaviour {

	string name;
	public int health;
	public int damage;
	protected bool paused = false;
	protected float attackRadius;
	public static List<ActiveAgent> Enemies = new List<ActiveAgent>();

	abstract public string GetName ();
	abstract public void DealDamage (ActiveAgent enemy);
	abstract public void TakeDamage (int dmg);
	public int GetHealth () {return health;}
	abstract public void Pause ();
	abstract protected void Die (); 
}
