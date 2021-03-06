﻿using UnityEngine;
using System.Collections;
using GamepadInput;

//handles player movement, utilising the CharacterMotor class
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(DealDamage))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMove : MonoBehaviour 
{
	public bool alive = true;

	//setup
	public bool sidescroller;					//if true, won't apply vertical input
	public Transform mainCam, floorChecks;		//main camera, and floorChecks object. FloorChecks are raycasted down from to check the player is grounded.
	public Animator animator;					//object with animation controller on, which you want to animate
	public AudioClip jumpSound;					//play when jumping
	public AudioClip landSound;					//play when landing on ground
	public AudioClip moveSound;					//play when moving on ground
	public AudioClip deathSound;

	public GameObject arrowPrefab;
	private GameObject arrow;
	private bool arrowing = false;
	Quaternion originalRot;

	//movement
	public float accel = 70f;					//acceleration/deceleration in air or on the ground
	public float airAccel = 18f;			
	public float decel = 7.6f;
	public float airDecel = 1.1f;
	[Range(0f, 5f)]
	public float rotateSpeed = 0.7f, airRotateSpeed = 0.4f;	//how fast to rotate on the ground, how fast to rotate in the air
	public float maxSpeed = 9;								//maximum speed of movement in X/Z axis
	public float slopeLimit = 40, slideAmount = 35;			//maximum angle of slopes you can walk on, how fast to slide down slopes you can't
	public float movingPlatformFriction = 7.7f;				//you'll need to tweak this to get the player to stay on moving platforms properly
	
	//jumping
	public Vector3 jumpForce =  new Vector3(0, 13, 0);		//normal jump force
	public Vector3 secondJumpForce = new Vector3(0, 13, 0); //the force of a 2nd consecutive jump
	public Vector3 thirdJumpForce = new Vector3(0, 13, 0);	//the force of a 3rd consecutive jump
	public float jumpDelay = 0.1f;							//how fast you need to jump after hitting the ground, to do the next type of jump
	public float jumpLeniancy = 0.17f;						//how early before hitting the ground you can press jump, and still have it work
	[HideInInspector]
	public int onEnemyBounce;					
	
	private int onJump;
	private bool grounded;
	private Transform[] floorCheckers;
	private Quaternion screenMovementSpace;
	private float airPressTime, groundedCount, curAccel, curDecel, curRotateSpeed, slope;
	private Vector3 turnDirection, walkDirection, moveDirection, screenMovementForward, screenMovementRight, movingObjSpeed;
	
	private CharacterMotor characterMotor;
	private EnemyAI enemyAI;
	private DealDamage dealDamage;

	private Vector3 startPos;

	public GameObject splashEffect;
	public GameObject splashCloudEffect;

	//setup
	void Awake()
	{	
		//create single floorcheck in centre of object, if none are assigned
		if(!floorChecks)
		{
			floorChecks = new GameObject().transform;
			floorChecks.name = "FloorChecks";
			floorChecks.parent = transform;
			floorChecks.position = transform.position + new Vector3(0f, 0f, 0f);
			GameObject check = new GameObject();
			check.name = "Check1";
			check.transform.parent = floorChecks;
			check.transform.position = transform.position;
			Debug.LogWarning("No 'floorChecks' assigned to PlayerMove script, so a single floorcheck has been created", floorChecks);
		}
		//assign player tag if not already
		if(tag != "Player")
		{
			tag = "Player";
			Debug.LogWarning ("PlayerMove script assigned to object without the tag 'Player', tag has been assigned automatically", transform);
		}
		//usual setup
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		dealDamage = GetComponent<DealDamage>();
		characterMotor = GetComponent<CharacterMotor>();
		//gets child objects of floorcheckers, and puts them in an array
		//later these are used to raycast downward and see if we are on the ground
		floorCheckers = new Transform[floorChecks.childCount];
		for (int i=0; i < floorCheckers.Length; i++)
			floorCheckers[i] = floorChecks.GetChild(i);

		startPos = transform.position;
	}

	public void reset()
	{
		GameManager.instance.getBlobbi().GetComponent<BlobbiManager>().GetComponent<Renderer>().material.color = new Color (1f, 1f, 1f, 1f);
		Rigidbody rb = characterMotor.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.detectCollisions = true;
		transform.position = startPos;
		alive = true;
	}



	public void death()
	{
		if (alive == true)
		{
			GameManager.instance.getMusicHandler().GetComponent<MusicHandler>().stopPitch ();

			GameObject splash = (GameObject)Object.Instantiate(splashEffect,
			                                                   new Vector3(transform.position.x,
			            													GameManager.instance.getShadow().transform.position.y,
			            													transform.position.z),
			                                                   Quaternion.Euler (new Vector3(90f, 0f, 0f)));
			GameObject splashCloud = (GameObject)Object.Instantiate(splashCloudEffect,
			                                                   new Vector3(transform.position.x,
			          										   GameManager.instance.getShadow().transform.position.y,
			         										   transform.position.z),
			                                                   Quaternion.Euler (new Vector3(90f, 0f, 0f)));
			splash.GetComponent<ParticleSystem>().startSpeed = characterMotor.GetComponent<Rigidbody>().velocity.y / 3f;
			print (characterMotor.GetComponent<Rigidbody>().velocity.y / 5f);
			GameManager.instance.getBlobbi().GetComponent<BlobbiManager>().alive = false;
			GameManager.instance.getBlobbi().GetComponent<BlobbiManager>().GetComponent<Renderer>().enabled = false;
			// Stop the rising since we're dead!
			GameManager.instance.getShadow().GetComponent<ShadowManager>().riseRate = 0f;
			Rigidbody rb = characterMotor.GetComponent<Rigidbody>();
			rb.isKinematic = true;
			rb.detectCollisions = false;
			alive = false;

			GetComponent<AudioSource>().volume = 1;
			GetComponent<AudioSource>().clip = deathSound;
			GetComponent<AudioSource>().Play ();
		}
	}

	//get state of player, values and input
	void Update()
	{	
		//handle jumping
		JumpCalculations ();
		checkArrow ();

		//adjust movement values if we're in the air or on the ground
		curAccel = (grounded) ? accel : airAccel;
		curDecel = (grounded) ? decel : airDecel;
		curRotateSpeed = (grounded) ? rotateSpeed : airRotateSpeed;

		//get movement axis relative to camera
		screenMovementSpace = Quaternion.Euler (0, mainCam.eulerAngles.y, 0);
		screenMovementForward = screenMovementSpace * Vector3.forward;
		screenMovementRight = screenMovementSpace * Vector3.right;
		
		//get movement input, set direction to move in
		float h, v;
		if (GameManager.instance.controllerBlobbi)
		{
			if (arrowing == true)
			{
				h = 0;
				v = 0;
			} else {
				h = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.One).x;
				v = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.One).y;
			}
			
			float dist = Mathf.Sqrt((h * h) + (v * v));
			float angle = Mathf.Atan2(v, h);

			// Allow for smooth movement as well
			curAccel *= dist;
			curRotateSpeed *= dist;

			turnDirection = transform.position + (GameManager.instance.getPlayerCamera ().transform.rotation * new Vector3(Mathf.Cos(angle) * dist, 0f, Mathf.Sin(angle) * dist));
			walkDirection = transform.position + (GameManager.instance.getPlayerCamera ().transform.rotation * new Vector3(Mathf.Cos(angle) * dist, 0f, Mathf.Sin(angle) * dist));

		} else {
			h = Input.GetAxisRaw ("Horizontal");
			v = Input.GetAxisRaw ("Vertical");

			turnDirection = screenMovementRight * h;
			walkDirection = screenMovementForward * v;

			turnDirection += transform.position;
			walkDirection += transform.position;
		}
	}
	
	//apply correct player movement (fixedUpdate for physics calculations)
	void FixedUpdate() 
	{
		//are we grounded
		grounded = IsGrounded ();
		//move, rotate, manage speed
		characterMotor.MoveTo (walkDirection, curAccel, 0f, true); // 0f (third argument) is forward movement dead zone
		if (rotateSpeed != 0 && turnDirection.magnitude != 0)
			characterMotor.RotateToDirection (turnDirection , curRotateSpeed * 5, true);

	
			characterMotor.ManageSpeed (curDecel, maxSpeed + movingObjSpeed.magnitude, true);

		//set animation values
		if(animator)
		{
			animator.SetFloat("DistanceToTarget", characterMotor.DistanceToTarget);
			animator.SetBool("Grounded", grounded);
			animator.SetFloat("YVelocity", GetComponent<Rigidbody>().velocity.y);
		}
		//movement sound
		if(grounded && moveSound && Input.GetKeyDown(KeyCode.W) ||Input.GetKeyDown(KeyCode.S)){

			GetComponent<AudioSource>().volume = 1;
			GetComponent<AudioSource>().clip = moveSound;
			GetComponent<AudioSource>().Play ();

			/*if(curAccel == 0){
				GetComponent<AudioSource>().volume = 1;
				GetComponent<AudioSource>().clip = moveSound;
				GetComponent<AudioSource>().Stop ();
			}
			*/

		}
		
	}
	
	//prevents rigidbody from sliding down slight slopes (read notes in characterMotor class for more info on friction)
	void OnCollisionStay(Collision other)
	{
		//only stop movement on slight slopes if we aren't being touched by anything else
		if (other.collider.tag != "Untagged" || grounded == false)
			return;
		//if no movement should be happening, stop player moving in Z/X axis
		if(turnDirection.magnitude == 0 && slope < slopeLimit && GetComponent<Rigidbody>().velocity.magnitude < 2)
		{
			//it's usually not a good idea to alter a rigidbodies velocity every frame
			//but this is the cleanest way i could think of, and we have a lot of checks beforehand, so it shou
			//GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
	
	//returns whether we are on the ground or not
	//also: bouncing on enemies, keeping player on moving platforms and slope checking
	private bool IsGrounded() 
	{
		//get distance to ground, from centre of collider (where floorcheckers should be)
		float dist = GetComponent<Collider>().bounds.extents.y;
		//check whats at players feet, at each floorcheckers position
		foreach (Transform check in floorCheckers)
		{
			RaycastHit hit;
			if(Physics.Raycast(check.position, Vector3.down, out hit, dist + 0.05f))
			{
				if(!hit.transform.GetComponent<Collider>().isTrigger)
				{
					//slope control
					slope = Vector3.Angle (hit.normal, Vector3.up);
					//slide down slopes
					if(slope > slopeLimit && hit.transform.tag != "Pushable")
					{
						Vector3 slide = new Vector3(0f, -slideAmount, 0f);
						GetComponent<Rigidbody>().AddForce (slide, ForceMode.Force);
					}
					//enemy bouncing
					if (hit.transform.tag == "Enemy" && GetComponent<Rigidbody>().velocity.y < 0)
					{
						enemyAI = hit.transform.GetComponent<EnemyAI>();
						enemyAI.BouncedOn();
						onEnemyBounce ++;
						dealDamage.Attack(hit.transform.gameObject, 1, 0f, 0f);
					}
					else
						onEnemyBounce = 0;
					//moving platforms
					if (hit.transform.tag == "MovingPlatform" || hit.transform.tag == "Pushable")
					{
						movingObjSpeed = hit.transform.GetComponent<Rigidbody>().velocity;
						movingObjSpeed.y = 0f;
						//9.5f is a magic number, if youre not moving properly on platforms, experiment with this number
						GetComponent<Rigidbody>().AddForce(movingObjSpeed * movingPlatformFriction * Time.fixedDeltaTime, ForceMode.VelocityChange);
					}
					else
					{
						movingObjSpeed = Vector3.zero;
					}
					//yes our feet are on something
					return true;
				}
			}
		}
		movingObjSpeed = Vector3.zero;
		//no none of the floorchecks hit anything, we must be in the air (or water)
		return false;
	}
	
	//jumping
	private void JumpCalculations()
	{
		//keep how long we have been on the ground
		groundedCount = (grounded) ? groundedCount += Time.deltaTime : 0f;

		//play landing sound
		if(groundedCount < 0.1 && groundedCount != 0 && GetComponent<Rigidbody>().velocity.y < 1 && landSound && !GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().volume = 1;
			GetComponent<AudioSource>().clip = landSound;
			GetComponent<AudioSource>().Play ();
		}
		//if we press jump in the air, save the time

		if (!grounded)
		{
			if (arrowing == false)
			{
				if (GameManager.instance.controllerBlobbi && GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One)
				    || !GameManager.instance.controllerBlobbi && Input.GetButtonDown ("Jump"))
				{
					airPressTime = Time.time;
				}
			}
		}
			
		
		//if were on ground within slope limit
		if (grounded && slope < slopeLimit)
		{
			if (arrowing == false)
			{
			//and we press jump, or we pressed jump justt before hitting the ground
				if ((GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) && GameManager.instance.controllerBlobbi) 
				    || (!GameManager.instance.controllerBlobbi && Input.GetButtonDown ("Jump"))
				    || (airPressTime + jumpLeniancy > Time.time))
				{	
					Jump (jumpForce);
				}
			}
		}
	}
	
	//push player at jump force
	public void Jump(Vector3 jumpVelocity)
	{
		if(jumpSound)
		{
			GetComponent<AudioSource>().volume = 1;
			GetComponent<AudioSource>().clip = jumpSound;
			GetComponent<AudioSource>().Play ();
		}
		GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z);
		GetComponent<Rigidbody>().AddRelativeForce (jumpVelocity, ForceMode.Impulse);
		airPressTime = 0f;
	}

	public void checkArrow()
	{
		GamepadState state = GamePad.GetState(GamePad.Index.One);

		if ((state.LeftStick) && GameManager.instance.controllerBlobbi)
		{
			if (arrowing == false)
			{
				arrow = (GameObject)Object.Instantiate(arrowPrefab, transform.position, Quaternion.identity);
				//arrow.GetComponent<Renderer>().material = GameManager.instance.getBlobbi().GetComponent<Renderer>().material;
				arrow.transform.position = transform.position + new Vector3(0f, 0.8f, 0f);
			}
			arrowing = true;

			arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, arrow.transform.localScale.z);
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().targetOffset = new Vector3(0f, 0.1f, -3f);
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().target = arrow.transform;
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().closeUp = true;
			originalRot = GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().transform.rotation;

		}

		if ((GamePad.GetButtonUp(GamePad.Button.LeftStick, GamePad.Index.One) && GameManager.instance.controllerBlobbi))
		{
			arrowing = false;
			Destroy (arrow);
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().targetOffset = new Vector3(0f, 2f, -7f);
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().target = transform;
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().closeUp = false;
			GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().transform.rotation = originalRot;
		}

		if (!grounded)
		{
			if (arrowing == true)
			{
				Destroy (arrow);
				GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().targetOffset = new Vector3(0f, 2f, -7f);
				GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().target = transform;
				GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().closeUp = false;
				GameManager.instance.getPlayerCamera().GetComponent<CameraFollow>().transform.rotation = originalRot;
				arrowing = false;
			}
		}
	}

}