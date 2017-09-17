using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	SpriteRenderer sRenderer;
	BoxCollider2D bCollider;

	void Start () 
	{
		sRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		bCollider = transform.GetComponent<BoxCollider2D>();
	}
	

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.name == "LovedOne")
		{
			sRenderer.enabled = false;
			bCollider.enabled = false;
			ParticleSystem particleEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
			particleEffect.Play();
		}
	}
}
