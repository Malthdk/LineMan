using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPower : MonoBehaviour {

	//Publics
	public List<AiHandler> aiHandlers;

	//Privates
	SpriteRenderer sRenderer;
	BoxCollider2D bCollider;

	void Start () 
	{
		FillList();
		sRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		bCollider = transform.GetComponent<BoxCollider2D>();
	}
	

	void Update () 
	{
		
	}

	void FillList()
	{
		foreach(GameObject aiObject in GameObject.FindGameObjectsWithTag("ai"))
		{
			AiHandler aiPatrol = aiObject.GetComponent<AiHandler>();
			aiHandlers.Add(aiPatrol);
		}
	}

	void NeutraliseAI(List<AiHandler> theList)
	{
		if (theList.Count == 0)
		{
			return;
		}
		else 
		{
			for (int i = 0; i < theList.Count; i++)
			{
				theList[i].behaviour = AiBehaviour.Neutralised;
			}	
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.name == "LovedOne")	
		{
			NeutraliseAI(aiHandlers);
			sRenderer.enabled = false;
			bCollider.enabled = false;
			ParticleSystem particleEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
			particleEffect.Play();
		}
	}
}
