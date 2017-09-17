using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiBehaviour
{
	Patrol,
	Agro,
	Chase,
	Neutralised
}

public class AiHandler : MonoBehaviour {

	//Publics
	public AiBehaviour behaviour;
	[HideInInspector]
	public SpriteRenderer sRenderer; //This should be referenced by all children instead of making their own call individually
	public GameObject graphics;

	//Privates
	public bool neutralised;
	public AiPatrolling patrollingScript;
	public AiGrumpy grumpyScript;
	public AiTurncoat turncoatScript;

	void Awake () 
	{
		neutralised = false;
		graphics = transform.GetChild(0).gameObject;
		sRenderer = graphics.GetComponent<SpriteRenderer>();

		if (gameObject.GetComponent<AiPatrolling>() == null) 
		{
		}
		else 
		{
			patrollingScript = gameObject.GetComponent<AiPatrolling>();
		}

		if (gameObject.GetComponent<AiTurncoat>() == null) 
		{
		}
		else 
		{
			turncoatScript = gameObject.GetComponent<AiTurncoat>();
		}

		if (gameObject.GetComponent<AiGrumpy>() == null) 
		{
		}
		else 
		{
			grumpyScript = gameObject.GetComponent<AiGrumpy>();
		}

	}
	

	void Update () 
	{

		switch(behaviour)
		{
		case AiBehaviour.Patrol:
			patrollingScript.isPatrolling = true;
			break;

		case AiBehaviour.Agro:
			patrollingScript.isPatrolling = false;
			break;

		case AiBehaviour.Chase:
			patrollingScript.isPatrolling = false;
			break;

		case AiBehaviour.Neutralised:
			if (!neutralised)
			{
				NeutraliseAI();	
				neutralised = true;
			}
			break;
		}
	}

	void NeutraliseAI()
	{
		patrollingScript.speed = 2;
		sRenderer.color = Color.white;
		graphics.tag = "Untagged";

		Debug.Log("Numnber of neutralised AIs");

		if (patrollingScript.mimic == true)
		{
			patrollingScript.mimic = false;
		}
		if (grumpyScript == null) 
		{
			Debug.Log("Number of AIs with no GrumpyScript");
		}
		else  if (grumpyScript != null)
		{
			Debug.Log("Number of AIs with GrumpyScript");
			grumpyScript.enabled = false;
		}

		if (turncoatScript == null) 
		{
		}
		else 
		{
			turncoatScript.enabled = false;
		}
	}
}
