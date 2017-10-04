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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.name == "LovedOne")
		{
			sRenderer.enabled = false;
			bCollider.enabled = false;
			AkSoundEngine.PostEvent ("Collect", gameObject);
			ParticleSystem particleEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
			particleEffect.Play();

			BGParticles.instance.hasCollected = true; //Setting hascollected in Background ParticleSystem for effect.
		}
	}
}
