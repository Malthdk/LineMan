using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParticles : MonoBehaviour {


	private ParticleSystem pSystem;
	private ParticleSystem.NoiseModule noiseModule;
	private ParticleSystem.MainModule mainModule;
	private ParticleSystem.LimitVelocityOverLifetimeModule limVelModule;
	private ParticleSystem.EmissionModule emissionModule;
	private ParticleSystem.VelocityOverLifetimeModule velLifeModule;
	private float startSpeedMax;
	private float startSpeedMin;
	private Color particleColor;

	void Start () 
	{
		pSystem = GetComponent<ParticleSystem>();
		noiseModule = pSystem.noise;
		mainModule = pSystem.main;
		startSpeedMax = mainModule.startSpeed.constantMax;
		limVelModule = pSystem.limitVelocityOverLifetime;
		velLifeModule = pSystem.velocityOverLifetime;
		emissionModule = pSystem.emission;
	}
	

	void Update () 
	{
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

		if ( LevelManager.instance.isRespawning)
		{
			limVelModule.enabled = false;
			velLifeModule.enabled = true;
			//mainModule.startSpeed = 10f;
			//emissionModule.rateOverTime = 200f;
			//mainModule.startLifetime = 1f;
		}
		else
		{
			mainModule.startSpeed = 0.1f;
			limVelModule.enabled = true;
			velLifeModule.enabled = false;
			emissionModule.rateOverTime = 20f;
			mainModule.startLifetime = 10f;
		}
	}


}
