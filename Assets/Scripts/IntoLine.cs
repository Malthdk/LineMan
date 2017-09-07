using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoLine : MonoBehaviour {
	
	//Publics
	public Direction direction;

	//Privates
	private Player player;
	private Controller2D controller;
	private Animator animator;
	private bool special, downArrow, upArrow, rightArrow, leftArrow;
	private bool transformIn;
	private bool shiftUnlocked;

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

		shiftUnlocked = true;
	}

	void Update () 
	{
		//Input variables
		special = Input.GetKeyDown(KeyCode.LeftShift);
		downArrow = Input.GetKey(KeyCode.DownArrow);
		upArrow = Input.GetKey(KeyCode.UpArrow);
		rightArrow = Input.GetKey(KeyCode.RightArrow);
		leftArrow = Input.GetKey(KeyCode.LeftArrow);
			
		//Direction states for the player
		switch(direction)
		{
		case Direction.Floor:
			//INPUT
			if (special && shiftUnlocked)
			{
				if (downArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, -2.8f, 0f), Direction.Cieling));
					//direction = Direction.Cieling;
					//transform.Translate(0, -2.8f, 0);
				}
				else if (rightArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0.3f, 0f), Direction.Rightwall));
					//direction = Direction.Rightwall;
					//transform.Translate(0f, 0f, 0f);
				}
				else if (leftArrow  && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0.3f, 0f), Direction.Leftwall));
					//direction = Direction.Leftwall;
					//transform.Translate(0f, 0f, 0f);
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			ResetGravity();
			controller.playerOnground = true;
			controller.playerOnCieling = controller.playerOnRightWall = controller.playerOnLeftWall = false;
			break;

		case Direction.Cieling:
			//INPUT
			if (special && shiftUnlocked)
			{
				if (upArrow && controller.collisions.above)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 2.8f, 0f), Direction.Floor));
					//direction = Direction.Floor;
					//transform.Translate(0, 2.8f, 0);
				}
				else if (rightArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0f, 0f), Direction.Rightwall));
					//direction = Direction.Rightwall;
					//transform.Translate(0f, 0f, 0f);
				}
				else if (leftArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0f, 0f), Direction.Leftwall));
					//direction = Direction.Leftwall;
					//transform.Translate(0f, 0f, 0f);
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			ReverseGravity();
			controller.playerOnCieling = true;
			controller.playerOnground = controller.playerOnRightWall = controller.playerOnLeftWall = false;
			break;

		case Direction.Rightwall:
			//INPUT
			if (special && shiftUnlocked)
			{
				if (downArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, -1f, 0f), Direction.Leftwall));
					//direction = Direction.Leftwall;
					//transform.Translate(0, -1f, 0);
				}
				else if (rightArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(-1.5f, 0.3f, 0f), Direction.Cieling));
					//direction = Direction.Cieling;
					//transform.Translate(-1.5f, 0.5f, 0f);
				}
				else if (leftArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0.3f, 0f), Direction.Floor));
					//direction = Direction.Floor;
					//transform.Translate(0f, 0f, 0f);
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			ResetGravity();
			controller.playerOnRightWall = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnLeftWall = false;
			break;

		case Direction.Leftwall:
			//INPUT
			if (special && shiftUnlocked)
			{
				if (downArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, -1f, 0f), Direction.Rightwall));
					//direction = Direction.Rightwall;
					//transform.Translate(0, -1f, 0);
				}
				else if (rightArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, 0.3f, 0f), Direction.Floor));
					//direction = Direction.Floor;
					//transform.Translate(0f, 0f, 0f);
				}
				else if (leftArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(1.5f, 0.3f, 0f), Direction.Cieling));
					//direction = Direction.Cieling;
					//transform.Translate(1.5f, 0.5f, 0f);
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			ResetGravity();
			controller.playerOnLeftWall = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnRightWall = false;
			break;

		}
	}

	void ResetGravity()
	{
		player.maxJumpHeight = 3.5f;		
		player.timeToJumpApex = 0.65f;		
		player.minJumpHeight = 0.5f;
	}

	void ReverseGravity()
	{
		player.maxJumpHeight = -3.5f;
		player.timeToJumpApex = -0.65f;
		player.minJumpHeight = -0.5f;
	}

	public void ResetDirection(Direction directionState)
	{
		direction = directionState;
	}

	public IEnumerator TransformPlayer(Vector3 transformation, Direction directionState)
	{
		shiftUnlocked = false; 
		player.movementUnlocked = false;
		player.velocity.x = 0;
		player.velocity.y = 0;
		animator.SetTrigger("goDown");

		yield return new WaitForSeconds(0.4f);
		transform.Translate(transformation);
		direction = directionState;

		yield return new WaitForSeconds(0.1f);
		animator.SetTrigger("goUp");

		yield return new WaitForSeconds(0.3f);
		shiftUnlocked = true;
		player.movementUnlocked = true;
	}
}
