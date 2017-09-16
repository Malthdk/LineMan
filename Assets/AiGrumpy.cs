using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGrumpy : MonoBehaviour {

	//Publics
	public Grumpy grumpy;
	public Color deadly;
	public LayerMask collisionMask;
	public int stressedSpeed = 5;
	public int relaxSpeed = 2;
	public float stressedTime = 5f;
	public float spookedTime = 0.8f;

	//Privates
	private Transform player;
	private AiPatrolling patrollScript;
	private BoxCollider2D bCollider;
	private Bounds bounds;
	private Vector2 rayOrigin;
	private ParticleSystem.MainModule pSystem;
	private SpriteRenderer sRenderer;
	private GameObject parent;

	public enum Grumpy
	{
		Spooked,
		Stressed,
		Relaxed
	}

	void Start () 
	{
		parent = transform.parent.gameObject;
		player = GameObject.Find("Player").transform;
		patrollScript = parent.GetComponent<AiPatrolling>();
		bCollider = GetComponent<BoxCollider2D>();
		bounds = bCollider.bounds;
		sRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		pSystem = parent.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().main;
	}

	void Update () 
	{
		PlaceRayOrigin();

		switch(grumpy)
		{
		case Grumpy.Spooked:

			transform.GetChild(0).gameObject.tag = "Untagged";
			sRenderer.color = Color.white;
			pSystem.startColor = Color.white;

			/// NEW STUFF
			Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0);
			Vector2 dir = player.position - center;

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, Mathf.Infinity, collisionMask);
			Debug.DrawRay(rayOrigin, dir * 15f, Color.red);

			if (hit.collider != null) 
			{
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
				{
					Debug.Log("I SEE YOU!");
					patrollScript.isMoving = false;
					StartCoroutine("SpookToStress");
				}
			}
			break;

		case Grumpy.Stressed:
			sRenderer.color = deadly;
			pSystem.startColor = deadly;
			transform.GetChild(0).gameObject.tag = "killTag";
			patrollScript.speed = stressedSpeed;
			StartCoroutine("StressToRelax");

			break;

		case Grumpy.Relaxed:
			sRenderer.color = Color.white;
			pSystem.startColor = Color.white;
			transform.GetChild(0).gameObject.tag = "Untagged";
			patrollScript.speed = relaxSpeed;
			break;
		}
	}
		
	IEnumerator SpookToStress()
	{
		//play some spooked animation
		yield return new WaitForSeconds(spookedTime);
		patrollScript.isMoving = true;
		grumpy = Grumpy.Stressed;
	}

	IEnumerator StressToRelax()
	{
		yield return new WaitForSeconds(stressedTime);

		grumpy = Grumpy.Relaxed;
	}

	void PlaceRayOrigin()
	{
		bounds.Expand (0.015f * -2);
		rayOrigin = new Vector2(bounds.center.x, bounds.center.y);

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			grumpy = Grumpy.Spooked;
		}
	}
}
