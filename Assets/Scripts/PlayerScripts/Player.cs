using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 3.5f;				//Max JumpHeight
	public float maxJumoHeightPainting;
	public float minJumpHeight = 1f;				//Minimum JumpHeight
	[HideInInspector]
	public float purpMinJumpVelocity = -9.3f;		//Minimum jump velocity when purple - have to be defined here cause of mathematical equation
	public float timeToJumpApex = .65f;				//Time to reach highest point (4875 was before)
	public float accelerationTimeAirborn = .2f;		//Acceleration while airborne
	public float accelerationTimeGrounded = .5f;	//Acceleration while grounded
	[HideInInspector]
	public float moveSpeed = 9;	
	public float airSpeed = 6.4f, groundSpeed = 9.4f;
	public float paintingSpeed;
	[HideInInspector]
	public Vector2 input;
	[HideInInspector]
	public int wallDirX;

	//FOR WALLSLIDING
	[HideInInspector]
	public float wallSlideSpeedMax = 0.01f;		//Maximum wall spide speed
	private float wallStickTime = 1f;		//Time before releasing from wall when input is away from wall
	float timeToWallUnstick;
	[HideInInspector]
	public bool wallSliding;
	[HideInInspector]
	public float wjXSmoothing, wjYSmoothing;
	private float wjAcceleration = 0.05f;

	public float gravity;					//gramaxJumpVelocity to player
	[HideInInspector]
	public float maxJumpVelocity, minJumpVelocity;			//Min jump velocity
	public Vector3 velocity;				//velocity
	[HideInInspector]
	public float velocityXSmoothing;		//smoothing on velocity

	Controller2D controller;					//calling controller class
	IntoLine intoLine;

	private Animator animator;		//ANIMATION
	[HideInInspector]
//	public bool doubleJumped, tripleJumped, hasTripleJumped = false, hasDoubleJumped = false;
//	private float doubleJumpVelocity;
//	public float doubleJumpReduction;
//	private float tripleJumpVelocity;
//	public float tripleJumpReduction;
	public float gravityModifierFall;
	public float gravityModifierJump;
//	public float gravityModifierPaintingJump;
//	public float gravityModifierPaintingFall;

	// Has player landed?
	private bool landed = true;

	public float ghostJumpingBuffer = 0.15f;
	private float timeSinceJump;

	// FOR SOUND
	public AudioClip jumpSoundTakeOff;
	public AudioClip jumpSoundLanding;
	public AudioClip paintingSound;
	public AudioClip paintingSplashSound;

	[HideInInspector]
	public AudioSource paintingSoundSource;
	[HideInInspector]
	public AudioSource jumpSoundSource;

	private float volLowRange = .6f;
	private float volHighRange = .8f;
	private float volLanding;

	//ParticleSystems for dJump and tJump
	private ParticleSystem doubleJumpParticle;
	private ParticleSystem tripleJumpParticle;

	//Can the character be moved on X or Y axis. 
	[HideInInspector]
	public bool movementUnlocked;
	private float targetVelocityX;

	//Is the controlls inverse
	[HideInInspector]
	public bool inverseControlX;
	[HideInInspector]
	public static Player _instance;

	public static Player instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<Player>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		movementUnlocked = true;
//		jumpSoundSource = gameObject.transform.GetChild(9).GetComponent<AudioSource>();
		controller = GetComponent<Controller2D>();
		intoLine = GetComponent<IntoLine>();
		animator = transform.GetComponentInChildren<Animator>();		//ANIMATION
	}

	void Update () 
	{
		gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		//Gravity defined based of jumpheigmaxJumpVelocityto reach highest point

		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;				//Max jump velocity defined based on gravity and time to reach highest point
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);	//Min jump velocity defined based on gravity and min jump height

		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));		//Input from player

		if (inverseControlX)
		{
			if (input.x > 0 || input.y > 0)
			{
				input.x = -input.x;
				input.y = -input.y;
			}
			else if (input.x < 0 || input.y < 0)
			{
				input.x = Mathf.Abs(input.x);
				input.y = Mathf.Abs(input.y);
			}
				
		}
		wallDirX = (controller.collisions.left)? -1:1;											//wall direction left or right

		targetVelocityX = (controller.playerOnLeftWall || controller.playerOnRightWall)?input.y * moveSpeed:input.x * moveSpeed;


		if (input.x == 1 || input.x == -1) //[BUG REPORT] Minor problem with changing direction and keeping same speed
		{
			accelerationTimeGrounded = 0.25f;
		}
		else
		{
			accelerationTimeGrounded = 0.05f; //0.05
		}

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborn);		//Calculating velocity x both airborn and on ground with smoothing

//		animator.SetFloat("Speed", Mathf.Abs(velocity.x));			//ANIMATION
		animator.SetBool("ground", controller.collisions.below);	//ANIMATION
//		animator.SetFloat("vSpeed", velocity.y);					//ANIMATION
		//animator.SetBool("OnWall", wallSliding);
		//animator.SetBool("Dashing", abilities.isDashing);
		//animator.SetBool ("Soaring", abilities.soaring);
//		animator.SetBool("DoubleJump", doubleJumped);
//		animator.SetBool("TripleJump", tripleJumped);
			
		if (!controller.collisions.below) {
			timeSinceJump += Time.deltaTime;
		} else {
			timeSinceJump = 0f;
		}

		if (Input.GetButtonDown("Jump"))
		{
			if (controller.collisions.below)
			{
				FirstJump();
			}

//			if(!controller.collisions.below)
//			{
//				/*This is where double jump is handled*/
//				if (!hasDoubleJumped)
//				{	
//					if (timeSinceJump < ghostJumpingBuffer){	
//						FirstJump ();
//					} else {
//						DoubleJump ();
//					}
//
//				}
//				else if (!hasTripleJumped && hasDoubleJumped)
//				{
//					TripleJump ();
//				}
//			}
		}

		if (!controller.collisions.below)
		{
			landed = false;
			moveSpeed = airSpeed;
			volLanding = (velocity.y / -30);
		}
		else if (controller.collisions.below && landed == false)
		{
//			jumpSoundSource.PlayOneShot(jumpSoundLanding, volLanding);
//			hasTripleJumped = false;
//			hasDoubleJumped = false;
//			tripleJumped = false; //animation
//			doubleJumped = false; //animation
			moveSpeed = groundSpeed;
			landed = true;
		}
		if (Mathf.Sign(velocity.y) == -1)
		{
			timeToJumpApex = gravityModifierFall; //55 //0.75
		}
		else 
		{
			timeToJumpApex = gravityModifierJump;//0.92f;	//72
//			doubleJumpVelocity = maxJumpVelocity/doubleJumpReduction;
//			tripleJumpVelocity = maxJumpVelocity/tripleJumpReduction;
		}

		if (Input.GetButtonUp("Jump"))					//For variable jump
		{
			if (velocity.y > minJumpVelocity) //&& (!hasDoubleJumped && !hasTripleJumped) to get it on DJ and TJ
			{
				velocity.y = minJumpVelocity;									//When space is released set velocity y to minimum jump velocity
			}
		}

		if (movementUnlocked == true)
		{
			velocity.y += gravity * Time.deltaTime;	
			controller.Move(velocity * Time.deltaTime, input);				//Moving character
		}


		if (controller.collisions.above  || controller.collisions.below)		//If raycasts hit above or below, velocity on y axis stops
		{
			velocity.y = 0;
		}
	}

	void FirstJump() {
		float vol = Random.Range(volLowRange, volHighRange);
//		jumpSoundSource.PlayOneShot(jumpSoundTakeOff, vol);
		velocity.y = maxJumpVelocity;
//		animator.SetBool("Ground", false);
	}

//	void DoubleJump() {
//		float vol = Random.Range(volLowRange, volHighRange);
//		if (timeSinceJump < ghostJumpingBuffer) {
//			jumpSoundSource.PlayOneShot(jumpSoundTakeOff, vol);
//			velocity.y = maxJumpVelocity;
////			animator.SetBool("Ground", false);
//		}
//		jumpSoundSource.PlayOneShot(jumpSoundTakeOff, (.1f + vol));
//		velocity.y = 0;
//		Debug.Log ("double jumped");
//		doubleJumped = true; //This causes an animation bugg where we jump from a wall and then doublejump is is set to true so we canno transition into jump animation.
//		hasDoubleJumped = true;
//		//abilities.secondJump = true;
//		velocity.y = doubleJumpVelocity;
////		doubleJumpParticle.Play();
//	}

//	void TripleJump() {
//		float vol = Random.Range(volLowRange, volHighRange);
//		jumpSoundSource.PlayOneShot(jumpSoundTakeOff, (.2f + vol));
//		velocity.y = 0;
//		Debug.Log ("tripple jumped");
//		tripleJumped = true; //animation
//		hasTripleJumped = true; 
//		velocity.y = tripleJumpVelocity;
////		tripleJumpParticle.Play();
//	}

	//IEnumerator SpecialJump(float velocity)
	//{
	//	yield return new WaitForEndOfFrame();
	//}
}