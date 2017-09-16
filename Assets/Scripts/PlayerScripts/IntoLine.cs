using System.Collections;
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

	//Privates
	private Player player;
	private Controller2D controller;
	private Animator animator;
	private bool cannotTransform;
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
				else if (rightArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Rightwall));
				}
				else if (leftArrow  && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Leftwall));
				}
			}
			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			controller.playerOnground = true;
			controller.playerOnCieling = controller.playerOnRightWall = controller.playerOnLeftWall = player.inverseControlX = false;
			break;

		case Direction.Cieling:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (upArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Floor));
				}
				else if (rightArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Rightwall));
				}
				else if (leftArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Leftwall));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 180f);
			controller.playerOnCieling = player.inverseControlX = true;
			controller.playerOnground = controller.playerOnRightWall = controller.playerOnLeftWall = false;
			break;

		case Direction.Rightwall:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (rightArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Leftwall));
				}
				else if (upArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Cieling));
				}
				else if (downArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Floor));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			controller.playerOnRightWall = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnLeftWall = player.inverseControlX = false;
			break;

		case Direction.Leftwall:
			//INPUT
			if (inputLocked == false && cannotTransform == false)
			{
				if (leftArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Rightwall));
				}
				else if (downArrow && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Floor));
				}
				else if (upArrow && controller.collisions.left)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Cieling));
				}
			}

			//TRANSFORMATIONS
			transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			controller.playerOnLeftWall = player.inverseControlX = true;
			controller.playerOnground = controller.playerOnCieling = controller.playerOnRightWall = false;
			break;
		}
	}

	public void ResetDirection(Direction directionState)
	{
		direction = directionState;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "deadZone")
		{
			cannotTransform = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "deadZone")
		{
			cannotTransform = false;
		}
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

		yield return new WaitForSeconds(0.8f);
		particleEffect.Stop();
		transform.Translate(transformation);
		direction = directionState;

		yield return new WaitForSeconds(0.1f);
		animator.SetTrigger("goUp");
		particleEffect.Play();
		yield return new WaitForSeconds(0.8f);
		particleEffect.Stop();
		player.movementUnlocked = true;
		transforming = false;

		yield return new WaitForEndOfFrame();
		inputLocked = false;
	}
}
