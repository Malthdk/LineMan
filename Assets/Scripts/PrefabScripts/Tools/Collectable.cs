using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	SpriteRenderer sRenderer;
	//ParticleSystem pSystem;
	//ParticleSystem.LimitVelocityOverLifetimeModule limVelModule;
	//ParticleSystem.MainModule mainModule;
	BoxCollider2D bCollider;
	GameObject textObject;

	void Start () 
	{
		sRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		bCollider = transform.GetComponent<BoxCollider2D>();
		textObject = transform.GetChild(2).gameObject;
		//pSystem = transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
		//limVelModule = pSystem.limitVelocityOverLifetime;
		//mainModule = pSystem.main;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.name == "LovedOne")
		{
			sRenderer.enabled = false;
			//limVelModule.enabled = false;
			//mainModule.loop = false;

			bCollider.enabled = false;
			AkSoundEngine.PostEvent ("Collect", gameObject);
			ParticleSystem particleEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
			particleEffect.Play();

			BGParticles.instance.hasCollected = true; //Setting hascollected in Background ParticleSystem for effect.
			textObject.SetActive(true);
		}
	}
}
