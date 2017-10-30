using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	SpriteRenderer sRenderer;
	ParticleSystem pSystem;
	//ParticleSystem.LimitVelocityOverLifetimeModule limVelModule;
	//ParticleSystem.MainModule mainModule;
	PolygonCollider2D pCollider;
	GameObject textObject;
	Animator animator;

	void Start () 
	{
		sRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		pCollider = transform.GetComponent<PolygonCollider2D>();
		textObject = transform.GetChild(2).gameObject;
		pSystem = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
		animator = GetComponent<Animator>();
		//limVelModule = pSystem.limitVelocityOverLifetime;
		//mainModule = pSystem.main;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.name == "LovedOne")
		{
			sRenderer.enabled = false;
			animator.enabled = false;
			transform.localScale = new Vector3(1,1,1);
			//limVelModule.enabled = false;
			//mainModule.loop = false;

			pCollider.enabled = false;
			AkSoundEngine.PostEvent ("Collect", gameObject);
			pSystem.Clear();
			pSystem.time = 0f;
			var main = pSystem.main;
			main.startLifetime = 5f;
			main.duration = 5f;
			pSystem.Play();
			/*ParticleSystem particleEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
			particleEffect.Play();*/

			BGParticles.instance.hasCollected = true; //Setting hascollected in Background ParticleSystem for effect.
			textObject.SetActive(true);
		}
	}
}
