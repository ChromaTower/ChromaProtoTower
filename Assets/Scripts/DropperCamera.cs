﻿using UnityEngine;
using System.Collections;

public class DropperCamera : MonoBehaviour {

	// The tower "object" - comprised of the blocks themselves
	public GameObject tower;

	// The blocks used - prefab
	public GameObject block;

	// The increments the camera is snapped to when rotating
	// Not advised that you change this, but if you want to...
	public int snapAngle = 90;

	// How fast the camera moves up to the top of the building in seconds
	private float moveTime = 0.5f;
	// Counts down when moving
	private float moving = 0f;
	// Position difference
	private float posDifference = 0f;

	// How far the camera sits above the tower
	private float verticalHeight = 10f;
	

	// How much time to wait between rotations, in sec
	private float rotWaitTime = 0.25f;
	// Counts down when rotating
	private float rotWaiting = 0f;
	// -1 for backwards, 1 for forwards
	private int rotAngle = 0;

	// Mouse-to-world positions!
	private float mouseX = 0f;
	private float mouseY = 0f;
	private float mouseZ = 0f;

	// Cooldown for block placing
	private float blockWaitTime = 0.5f;
	private float blockWaiting = 0f;

	// Makes blocks stack on top of tower
	// Change at risk of losing sanity
	private bool blocksOnTop = true;

	// Use this for initialization
	void Start () {
	
	}

	void VerticalMoveUpdate()
	{
		float currentY = transform.position.y;
		float targetY = tower.transform.position.y + verticalHeight;

		if (moving <= 0f)
		{
			posDifference = targetY - currentY;

			if (posDifference > verticalHeight || posDifference < -verticalHeight)
			{
				moving = moveTime;
			}
		} else {
			moving -= Time.deltaTime;
			if (moving > 0)
			{
				transform.position = transform.position + new Vector3(0f, (posDifference/moveTime) * Time.deltaTime, 0f);
			}
			// Possible TODO: Add snapping?
		}
	}

	void rotateUpdate()
	{
		// Only rotate if the time has elapsed
		if (rotWaiting <= 0f)
		{
			int scroll = (int)Input.GetAxis("Mouse ScrollWheel");
			
			if (scroll != 0)
			{
				rotAngle = (int)Mathf.Sign(scroll);
				
				// Make the user wait before they can rotate again
				rotWaiting = rotWaitTime;
				
			}
		} else {
			// Decrease the rotation time
			rotWaiting -= Time.deltaTime;
			
			// If the timer hasn't elapsed, rotate normally
			// If the rotation timer has elapsed, snap to the angle (prevents overshooting)
			if (rotWaiting > 0)
			{
				transform.RotateAround(tower.transform.position, Vector3.up, rotAngle * ((snapAngle / rotWaitTime) * Time.deltaTime));
			} else {
				// Round the angle
				float targetAngle = Mathf.Round(transform.eulerAngles.y / snapAngle) * snapAngle;
				transform.RotateAround(tower.transform.position, Vector3.up, targetAngle - transform.eulerAngles.y);
			}
			
			// Point at the tower object
			transform.LookAt(tower.transform);
			
			
		}
	}

	void mouseUpdate()
	{
		if (blockWaiting <= 0f) {
			// Left mouse click
			if (Input.GetMouseButtonDown(0))
			{
				GameObject b = (GameObject)Object.Instantiate(block, tower.transform.position, Quaternion.identity);
				b.transform.parent = tower.transform;
				blockWaiting = blockWaitTime;
			}
		} else {
			blockWaiting -= Time.deltaTime;
		}
	}

	// Update is called once per frame
	void Update () {
		VerticalMoveUpdate ();
		rotateUpdate();
		mouseUpdate();

	}
}