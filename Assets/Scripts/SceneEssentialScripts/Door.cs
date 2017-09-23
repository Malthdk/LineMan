using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	//Publics
	public bool midPoint;
	public bool endPoint;
	public string nextLevelName;
	public Color openColor, closedColor;

	//Privates
	private Door cDoor;
	public bool open, closed;
	private SpriteRenderer myRenderer;
	private BoxCollider2D myCollider;
	public ParticleSystem pSystem;

	void Start () 
	{
		myRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		pSystem = transform.GetComponentInChildren<ParticleSystem>();

		myCollider = GetComponent<BoxCollider2D>();
		cDoor = (GameObject.FindWithTag("cPoint") == null)?null:GameObject.FindWithTag("cPoint").GetComponent<Door>();

		if (cDoor != null && cDoor.gameObject != this.gameObject)
		{
			midPoint = true;
			open = true;
		}
		else if (cDoor != null && cDoor.gameObject == this.gameObject)
		{
			endPoint = true;
			closed = true;
		}
		else if (cDoor == null)
		{
			endPoint = true;
			open = true;
		}
	}

	void Update () 
	{
		if (open)
		{
			myRenderer.color = openColor;
		}
		else if (closed)
		{
			myRenderer.color = closedColor;
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			if (endPoint && open)
			{
				AkSoundEngine.PostEvent ("EndLevel", gameObject);
				CompletedLevel();
			}
			else if (midPoint && open)
			{
				myCollider.enabled = false;
				cDoor.open = true;
				pSystem.Play();
			}
		}
	}
		
	void CompletedLevel() 
	{
		StartCoroutine(LevelManager.instance.NextLevel(nextLevelName));	
	}
}
