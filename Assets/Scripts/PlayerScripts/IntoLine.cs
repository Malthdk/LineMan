﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoLine : MonoBehaviour {
	
	//Publics
	public Direction direction;
	public float yOffsetRightLeft = 0.2f;
	public float yOffsetUpDown = -0.8f;
	[HideInInspector]
	public bool transforming;
	[HideInInspector]
	public bool inputLocked;
	[HideInInspector]
	public bool downArrow, upArrow, rightArrow, leftArrow;

	//This is for LovedOne mechanic
	public bool LovedOne;

	//Privates
	private Player player;
	private Controller2D controller;
	private Animator animator;
	public static bool cannotTransform;
	[HideInInspector]

	public static IntoLine _instance;

	public static IntoLine instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<IntoLine>();
			}
			return _instance;
		}
	}

	public enum Direction
	{
		Floor,
		Cieling,
		Rightwall,
		Leftwall
	}

	void Start () 
	{
		player = GetComponent<Player>();
		controller = GetComponent<Controller2D>();
		direction = Direction.Floor;
		animator = transform.GetComponentInChildren<Animator>();

		inputLocked = false;
	}

	void Update () 
	{
		//Input variables
		downArrow = Input.GetKey(KeyCode.DownArrow);
		upArrow = Input.GetKey(KeyCode.UpArrow);
		rightArrow = Input.GetKey(KeyCode.RightArrow);
		leftArrow = Input.GetKey(KeyCode.LeftArrow);
			
		//Direction states for the player
		switch(direction)
		{
		case Direction.Floor:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (downArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Cieling));
				}
				else if (((LovedOne)?leftArrow:rightArrow) && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Rightwall));
				}
				else if (((LovedOne)?rightArrow:leftArrow)  && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0, yOffsetRightLeft, 0f), Direction.Leftwall));
				}
			}
			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			controller.playerOnground = true;
			controller.playerOnCieling = controller.playerOnRightWall = controller.playerOnLeftWall = false;

			//This is for inverse control on LovedOne
			player.inverseControlX = (LovedOne)?true:false;

			break;

		case Direction.Cieling:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (upArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Floor));
				}
				else if (((LovedOne)?leftArrow:rightArrow) && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Rightwall));
				}
				else if (((LovedOne)?rightArrow:leftArrow) && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Leftwall));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 180f);
			controller.playerOnCieling = true;
			controller.playerOnground = controller.playerOnRightWall = controller.playerOnLeftWall = false;

			//This is for inverse control on LovedOne
			player.inverseControlX = (LovedOne)?false:true;
			break;

		case Direction.Rightwall:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (rightArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Leftwall));
				}
				else if (((LovedOne)?downArrow:upArrow) && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Cieling));
				}
				else if (((LovedOne)?upArrow:downArrow) && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Floor));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			controller.playerOnRightWall = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnLeftWall = false;

			//This is for inverse control on LovedOne
			player.inverseControlX = (LovedOne)?true:false;
			break;

		case Direction.Leftwall:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (leftArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Rightwall));
				}
				else if (((LovedOne)?upArrow:downArrow) && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Floor));
				}
				else if (((LovedOne)?downArrow:upArrow) && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Cieling));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			controller.playerOnLeftWall = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnRightWall = false;

			//This is for inverse control on LovedOne
			player.inverseControlX = (LovedOne)?false:true;

			break;
		}
	}

	public void ResetDirection(Direction directionState)
	{
		direction = directionState;
	}

	public IEnumerator TransformPlayer(Vector3 transformation, Direction directionState)
	{
		inputLocked = true;
		yield return new WaitForEndOfFrame();
		transforming = true;
		//inputLocked = true; 
		player.movementUnlocked = false;
		player.velocity.x = 0;
		player.velocity.y = 0;
		ParticleSystem particleEffect = player.gameObject.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
		particleEffect.Play();
		animator.SetTrigger("goDown");
		controller.collisions.left = controller.collisions.right = false; //Quick fix for at sørge for at den ikke fortsat tror er er collisions. Dette gjorde at man kunne transforme lige efter en transform.

		yield return new WaitForSeconds(0.8f);
		particleEffect.Stop();
		transform.Translate(transformation);
		direction = directionState;

		yield return new WaitForSeconds(0.1f);
		animator.SetTrigger("goUp");
		particleEffect.Play();
		yield return new WaitForSeconds(0.8f);
		particleEffect.Stop();
		transforming = false;

		yield return new WaitForEndOfFrame();
		inputLocked = false;
		player.movementUnlocked = true;
		yield return new WaitForEndOfFrame();
	}
}
