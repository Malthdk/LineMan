﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHandler : MonoBehaviour {

	//Publics
	public AiBehaviour behaviour;


	//Privates
	AiPatrolling patrollingScript;
	AiGrumpy grumpyScript;
//	HeatSeeking heatSeekingScript;

	public enum AiBehaviour
	{
		Patrol,
		Agro,
		Chase,
		Neutralised
	}

	void Start () 
	{
		if (gameObject.GetComponent<AiPatrolling>() == null) //|| gameObject.GetComponent<HeatSeeking>() == null)
		{
			return;
		}
		else
		{
			patrollingScript = gameObject.GetComponent<AiPatrolling>();
//			heatSeekingScript = gameObject.GetComponent<HeatSeeking>();
		}

		if (transform.GetComponentInChildren<AiGrumpy>() == null)
		{
			return;
		}
		else
		{
			grumpyScript = transform.GetComponentInChildren<AiGrumpy>();
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
			Debug.Log("Agroed!");
			behaviour = AiBehaviour.Chase;
			break;

		case AiBehaviour.Chase:
			patrollingScript.isPatrolling = false;
//			heatSeekingScript.isSeeking = true;
			break;

		case AiBehaviour.Neutralised:
			patrollingScript.isPatrolling = false;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		if(heatSeekingScript.isSeeking == true)
//		{
			//If it collides with walls - Destroy it.
//		}
	}
}
