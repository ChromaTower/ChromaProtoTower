﻿using UnityEngine;
using GamepadInput;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour 
{
	public Transform target;									//object camera will focus on and follow
	public Vector3 targetOffset =  new Vector3(0f, 3.5f, 7);	//how far back should camera be from the lookTarget
	public bool lockRotation;									//should the camera be fixed at the offset (for example: following behind the player)
	public float followSpeed = 6;								//how fast the camera moves to its intended position
	public float inputRotationSpeed = 100;						//how fast the camera rotates around lookTarget when you press the camera adjust buttons
	public bool mouseFreelook;									//should the camera be rotated with the mouse? (only if camera is not fixed)
	public float rotateDamping = 100;							//how fast camera rotates to look at target
	public GameObject waterFilter;								//object to render in front of camera when it is underwater
	public string[] avoidClippingTags;							//tags for big objects in your game, which you want to camera to try and avoid clipping with
	
	private Transform followTarget;
	private bool camColliding;

	private Vector3 posPrev, posPrev2;
	private Quaternion rotPrev, rotPrev2;
	private bool notColliding = true;

	public bool closeUp = false;

	private List<GameObject> obscuringBlocks = new List<GameObject>();

	//setup objects
	void Awake()
	{
		followTarget = new GameObject().transform;	//create empty gameObject as camera target, this will follow and rotate around the player
		followTarget.name = "Camera Target";
		if(waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = false;
		if(!target)
			Debug.LogError("'CameraFollow script' has no target assigned to it", transform);
		
		//don't smooth rotate if were using mouselook
		if(mouseFreelook)
			rotateDamping = 0f;

	}

	void FixedUpdate()
	{
		notColliding = true;

	}

	void LateUpdate()
	{
	}

	//run our camera functions each frame
	void Update()
	{

		if (!target)
			return;
		
		SmoothFollow ();
		if(rotateDamping > 0)
			SmoothLookAt();
		else
			transform.LookAt(target.position);


		if (closeUp)
		{
			GameManager.instance.getPlayer ().transform.rotation = transform.rotation;
		}
	}

	//toggle waterfilter, is camera clipping walls?
	void OnTriggerEnter(Collider other)
	{

			
		if (other.tag == "Water" && waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = true;
	}
	
	//toggle waterfilter, is camera clipping walls?
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Water" && waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = false;
	}
	
	//rotate smoothly toward the target
	void SmoothLookAt()
	{
		Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotateDamping * Time.deltaTime);
	}
		
	//move camera smoothly toward its target
	void SmoothFollow()
	{
		if (notColliding)
		{
			//move the followTarget (empty gameobject created in awake) to correct position each frame
			followTarget.position = target.position;
			followTarget.Translate(targetOffset, Space.Self);
			if (lockRotation)
			{
				if (GameManager.instance.controllerBlobbi)
				{
					float axisX = GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.One).x * inputRotationSpeed * Time.deltaTime;
					float axisY = GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.One).y * inputRotationSpeed * Time.deltaTime;

						followTarget.RotateAround (target.position, Vector3.up, axisX);
						followTarget.RotateAround (target.position, transform.right, -axisY);
				}
				else
				{
					followTarget.rotation = target.rotation;
				}

			}
			
			if(mouseFreelook)
			{
				//mouse look
				float axisX = Input.GetAxis ("Mouse X") * inputRotationSpeed * Time.deltaTime;
				followTarget.RotateAround (target.position,Vector3.up, axisX);
				float axisY = Input.GetAxis ("Mouse Y") * inputRotationSpeed * Time.deltaTime;
				followTarget.RotateAround (target.position, transform.right, -axisY);
			}
			else
			{
				//keyboard camera rotation look
				float axis = 0;//Input.GetAxis ("CamHorizontal") * inputRotationSpeed * Time.deltaTime;
				followTarget.RotateAround (target.position, Vector3.up, axis);
			}
			
			//where should the camera be next frame?
			Vector3 nextFramePosition = Vector3.Lerp(transform.position, followTarget.position, followSpeed * Time.deltaTime);
			Vector3 direction = nextFramePosition - target.position;
			//raycast to this position
			RaycastHit hit;
			if(Physics.Raycast (target.position, direction, out hit, direction.magnitude + 0.3f))
			{
				transform.position = nextFramePosition;
				foreach(string tag in avoidClippingTags)
					if(hit.transform.tag == tag)
						transform.position = hit.point - direction.normalized * 0.3f;
			}
			else
			{
				//otherwise, move cam to intended position
				transform.position = nextFramePosition;
			}


			if (transform.position.y < Mathf.Max(-1f, GameManager.instance.getShadow().transform.position.y))
			{
				transform.position = new Vector3(transform.position.x,
				                                 Mathf.Max(-1f, GameManager.instance.getShadow().transform.position.y),
				                                 transform.position.z);
			}

		}
	} 

	void checkObscuringBlocks()
	{
		foreach (GameObject b in obscuringBlocks)
		{
			if (b != null)
			{
				b.GetComponent<Block>().setAlpha (1f);
			}
		}

		obscuringBlocks.Clear ();

		RaycastHit[] hits;
		hits = Physics.RaycastAll (transform.position, transform.forward, Vector3.Distance (transform.position, target.transform.position), 1 << 16);



		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			obscuringBlocks.Add (hit.transform.gameObject);
		}

		foreach (GameObject b in obscuringBlocks)
		{
			b.GetComponent<Block>().setAlpha (0.2f);
		}

	}

	void OnPreCull()
	{
		checkObscuringBlocks();
		GameManager.instance.getTower ().transparentCameraPrep();
	}

}