using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LovedOne : MonoBehaviour {

	public string nextLevelName;

	void Start () 
	{

	}

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			CompletedLevel();
		}
	}

	void CompletedLevel() 
	{
		StartCoroutine(LevelManager.instance.NextLevel(nextLevelName));	
	}

}
