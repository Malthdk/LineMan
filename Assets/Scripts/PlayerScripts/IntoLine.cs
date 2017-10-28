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

	//This is for LovedOne mechanic
	public bool LovedOne;

	//Privates
	private Player player;
	private Controller2D controller;
	private Animator animator;
	public static bool cannotTransform;

	//For portal
	[HideInInspector]
	public Transform otherPortal;
	public Direction portalDirection;
	public Vector3 portalTransformation = new Vector3(0f, 0f, 0f);

	//To check if we can transform
	private bool transformBlocked;
	private BoxCollider2D myCollider;
	private float myWidth;
	public LayerMask hitMask;


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
		//direction = Direction.Floor;
		animator = transform.GetComponentInChildren<Animator>();
		myCollider = gameObject.GetComponent<BoxCollider2D>();
		myWidth = myCollider.bounds.extents.x;

		inputLocked = false;
	}

	void Update () 
	{
		//Check if on T-section - move to when transform is about to happen.
		CheckIfCanTransform();

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
			if (inputLocked == false && !transformBlocked)
			{
				Debug.Log("Face Dir: " + controller.collisions.faceDir);

				if (downArrow && controller.collisions.below)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetUpDown, 0f), Direction.Cieling));
				}
				else if (((LovedOne)?leftArrow:rightArrow) && controller.collisions.right)
				{
					StartCoroutine(TransformPlayer(new Vector3(0f, yOffsetRightLeft, 0f), Direction.Rightwall));
				}
				else if (((LovedOne)?rightArrow:leftArrow)  && (controller.collisions.left))
				{
					StartCoroutine(TransformPlayer(new Vector3(0, yOffsetRightLeft, 0f), Direction.Leftwall));
					Debug.Log("you have to transform now");
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
			if (inputLocked == false && !transformBlocked)
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
			if (inputLocked == false && !transformBlocked)
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
			if (inputLocked == false && !transformBlocked)
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
		AkSoundEngine.PostEvent ("Transition", gameObject);
		transforming = true;
		//inputLocked = true; 
		player.movementUnlocked = false; //Locks movement and stops raycasting
		player.velocity.x = 0;
		player.velocity.y = 0;
		ParticleSystem particleEffect = player.gameObject.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
		particleEffect.Play();
		animator.SetTrigger("goDown");
		controller.collisions.left = controller.collisions.right = false; //Quick fix for at sørge for at den ikke fortsat tror er er collisions. Dette gjorde at man kunne transforme lige efter en transform.

		yield return new WaitForSeconds(0.4f);
		particleEffect.Stop();

		if (Portal.playerOnPortal)
		{
			PortalTransform(otherPortal); //If is on portal
		}
		else
		{
			direction = directionState;
			transform.Translate(transformation);
		}

		yield return new WaitForSeconds(0.1f);
		animator.SetTrigger("goUp");
		particleEffect.Play();
		yield return new WaitForSeconds(0.4f);
		particleEffect.Stop();
		transforming = false;

		yield return new WaitForEndOfFrame();
		inputLocked = false;
		player.movementUnlocked = true;		//Unlocks movement and starts raycasting
		yield return new WaitForEndOfFrame();
	}

	void PortalTransform(Transform portal)
	{
		if (Portal.playerOnPortal)
		{
			transform.position = new Vector3(portal.position.x, portal.position.y, portal.position.z);
			direction = portalDirection;
			transform.Translate(portalTransformation);
		}
	}

	void CheckIfCanTransform()
	{
		if (controller.collisions.below)
		{
			Vector2 lineCastPos = transform.position.toVector2() - transform.right.toVector2() * (myWidth -0.05f) + -transform.up.toVector2() * 1.2f;//-Vector2.down * 1.2f;	

			transformBlocked = Physics2D.Linecast(lineCastPos, lineCastPos + transform.right.toVector2() * (myWidth -0.05f) * 2, hitMask); //new Vector2(lineCastPos.x + myWidth * 2, lineCastPos.y)

			Debug.DrawLine(lineCastPos, lineCastPos + transform.right.toVector2() * (myWidth -0.05f) * 2, Color.green);	
		}
		else
		{
			transformBlocked = false;
		}
	}
}
