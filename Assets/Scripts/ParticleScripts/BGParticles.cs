using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParticles : MonoBehaviour {


	private ParticleSystem pSystem;
	private ParticleSystem.NoiseModule noiseModule;
	private ParticleSystem.MainModule mainModule;

	void Start () 
	{
		pSystem = GetComponent<ParticleSystem>();
		noiseModule = pSystem.noise;
		mainModule = pSystem.main;
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
	}
}
