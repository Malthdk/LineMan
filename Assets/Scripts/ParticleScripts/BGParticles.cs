using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParticles : MonoBehaviour {

	//Publics
	[HideInInspector]
	public bool hasCollected = false;

	//Privates
	private ParticleSystem pSystem;
	private ParticleSystem.NoiseModule noiseModule;
	private ParticleSystem.MainModule mainModule;
	private ParticleSystem.LimitVelocityOverLifetimeModule limVelModule;
	private ParticleSystem.EmissionModule emissionModule;
	private ParticleSystem.VelocityOverLifetimeModule velLifeModule;
	private ParticleSystem.ColorOverLifetimeModule colLifeModule;
	private ParticleSystem.RotationBySpeedModule rotSpeedModule;
	private float startSpeedMax;
	private float startSpeedMin;

	//For fading
	Color newColor = new Color(0.137f, 0.137f, 0.137f, 1f);
	Gradient grad = new Gradient();

	[HideInInspector]
	public static BGParticles bgParticles;
	public static BGParticles instance {	// Makes it possible to call script easily from other scripts
		get {
			if (bgParticles == null) {
				bgParticles = FindObjectOfType<BGParticles>();
			}
			return bgParticles;
		}
	}

	void Start () 
	{
		pSystem = GetComponent<ParticleSystem>();
		noiseModule = pSystem.noise;
		mainModule = pSystem.main;
		startSpeedMax = mainModule.startSpeed.constantMax;
		limVelModule = pSystem.limitVelocityOverLifetime;
		velLifeModule = pSystem.velocityOverLifetime;
		emissionModule = pSystem.emission;
		colLifeModule = pSystem.colorOverLifetime;
		rotSpeedModule = pSystem.rotationBySpeed;
	}
	

	void Update () 
	{
		//This is for when the player is transforming
		if (IntoLine.instance.transforming)
		{
			noiseModule.frequency = 5f;
			noiseModule.strength = 5f;
		}
		else
		{
			noiseModule.frequency = 0.5f;
			noiseModule.strength = 0.5f;
		}

		//This is for when the player is dead
		if ( LevelManager.instance.isRespawning)
		{
			limVelModule.enabled = false;
			velLifeModule.enabled = true;
		}
		else
		{
			mainModule.startSpeed = 0.1f;
			limVelModule.enabled = true;
			velLifeModule.enabled = false;
			emissionModule.rateOverTime = 20f;
			mainModule.startLifetime = 10f;
		}

		//This is for when the player has collected a collectable
		if (hasCollected)
		{
			StartCoroutine("Brighten");
			rotSpeedModule.enabled = true;
		}
		else
		{
			StartCoroutine("Darken");
			rotSpeedModule.enabled = false;
		}
	}

	//This brightens the particlesystem
	public IEnumerator Brighten()
	{
		while ( newColor.r < 1f && newColor.g < 1f && newColor.b < 1f)
		{
			//newColor = Color.Lerp(new Color(0.137f, 0.137f, 0.137f), Color.white, 1f); //lerping doesnt work - but the current method is quite primitive.
			newColor.r += 0.005f;
			newColor.g += 0.005f;
			newColor.b += 0.005f;
				
			grad.SetKeys(new GradientColorKey[] 
				{ 
					new GradientColorKey(newColor, 0.0f),
					new GradientColorKey(newColor, 1.0f) 
				}, 
				new GradientAlphaKey[] 
				{ 
					new GradientAlphaKey(1.0f, 0.0f),
					new GradientAlphaKey(1.0f, 0.58f),
					new GradientAlphaKey(0.0f, 1.0f) 
				});

			colLifeModule.color = grad;

			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(2f);
		hasCollected = false;
	}

	//This darkens the particlesystem
	public IEnumerator Darken()
	{
		while ( newColor.r > 0.392f && newColor.g > 0.392f && newColor.b > 0.392f)
		{
			newColor.r -= 0.005f;
			newColor.g -= 0.005f;
			newColor.b -= 0.005f;

			grad.SetKeys(new GradientColorKey[] 
				{ 
					new GradientColorKey(newColor, 0.0f),
					new GradientColorKey(newColor, 0.0f)
				}, 
				new GradientAlphaKey[] 
				{ 
					new GradientAlphaKey(1.0f, 0.0f),
					new GradientAlphaKey(1.0f, 0.58f),
					new GradientAlphaKey(0.0f, 1.0f) 
				});

			colLifeModule.color = grad;

			yield return new WaitForEndOfFrame();
		}
			
	}
}
