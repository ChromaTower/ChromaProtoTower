using UnityEngine;
using System.Collections;
using GamepadInput;

public class DropperCamera : MonoBehaviour {

	// Used for placing new blocks on screen
	private GameObject blockPlace;


	// The increments the camera is snapped to when rotating
	// Not advised that you change this, but if you want to...
	public int snapAngle = 90;

	private float initAngle;

	// How fast the camera moves up to the top of the building in seconds
	private float moveTime = 0.5f;
	// Counts down when moving
	private float moving = 0f;
	// Position difference
	private float posDifference = 0f;

	// How far the camera sits above the tower
	private float verticalHeight = 8f;

	// How much time to wait between rotations, in sec
	private float rotWaitTime = 0.25f;
	// Counts down when rotating
	private float rotWaiting = 0f;
	// -1 for backwards, 1 for forwards
	private int rotAngle = 0;
	private float targetAngle = 0;

	// Cooldown for block placing
	private float blockWaitTime = 0.25f;
	private float blockWaiting = 0f;


	private TowerManager tower;
	private GameObject player;

	private float camMouseY = 0f;
	private float camX = 0f;
	private float camZ = 0f;


	// Use this for initialization
	void Start () {
		// Get the tower object
		tower = GameManager.instance.getTower();
		player = GameManager.instance.getPlayer();

		// Set the initial angle based on the camera's rotation
		initAngle = transform.eulerAngles.y;
	}

	public Vector3 getEulerAngles()
	{
		return transform.eulerAngles;
	}

	void VerticalMoveUpdate()
	{
		float currentY = transform.position.y;
		float targetY = camMouseY + verticalHeight;

		if (moving <= 0f)
		{
			posDifference = targetY - currentY;

			if (posDifference > 0 || posDifference < 0)
			{
				moving = moveTime;
			}
		} else {
			moving -= Time.deltaTime;
			if (moving > 0)
			{
				transform.position = transform.position + new Vector3(0f, (posDifference/moveTime) * Time.deltaTime, 0f);
			}
		}
	}

	void rotateUpdate()
	{
		// Only rotate if the time has elapsed
		if (rotWaiting <= 0f)
		{
			int scroll;

			if (GameManager.instance.controller)
			{
				int scroll1 = GamePad.GetButtonUp (GamePad.Button.RightShoulder, GamePad.Index.Two) ? 1 : 0;
				int scroll2 = GamePad.GetButtonUp (GamePad.Button.LeftShoulder, GamePad.Index.Two) ? 1 : 0;
				scroll = (scroll2 - scroll1);
			} else {
				// Mouse instead
				scroll = (int)Input.GetAxis("Mouse ScrollWheel");
			}

			
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
				transform.RotateAround(tower.getPos(), Vector3.up, rotAngle * ((snapAngle / rotWaitTime) * Time.deltaTime));
			} else {
				// Round the angle
				targetAngle = Mathf.Round((transform.eulerAngles.y - initAngle) / snapAngle) * snapAngle;
				transform.RotateAround(tower.getPos(), Vector3.up, targetAngle - (transform.eulerAngles.y - initAngle));
			}
		}
	}

	void mouseUpdate()
	{
		if (GameManager.instance.controller)
		{
			camMouseY += GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.Two).y * Time.deltaTime * 10;
		}
		else
		{
			if (Input.mousePosition.y > (Screen.height - 100))
			{
				camMouseY += 0.05f * ((Input.mousePosition.y - (Screen.height - 100))/100) ;
			} else if (Input.mousePosition.y < 100)
			{
				if (camMouseY > 0)
				{
					camMouseY -= 0.05f * ((100 - Input.mousePosition.y)/100) ;
				}
			}
		} 


		if (blockWaiting <= 0f) {
			if (GameManager.instance.controller)
			{
				if (GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.Two))
				{
					// Draw the block "preview" before the mouse is released
					if (blockPlace == null)
					{
						blockPlace = tower.createBlock();
					}
				} else if (GamePad.GetButtonUp (GamePad.Button.A, GamePad.Index.Two))
				{
					// Wake up the block - time to drop
					blockPlace.GetComponent<Block>().Drop();

					// Reset the timer so the player has to wait a bit before spawning another block
					blockWaiting = blockWaitTime;
					blockPlace = null;
				}
				
				if (blockPlace)
				{
					if (GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two))
					{
						blockPlace.transform.RotateAround(blockPlace.transform.position, Vector3.up, snapAngle);
					}

					camX += GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).x * Time.deltaTime * 15f;
					camZ += GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).y * Time.deltaTime * 15f;

					TowerManager tower = GameManager.instance.getTower ();
					camX = Mathf.Clamp (camX, tower.transform.position.x - tower.mapXSize, tower.transform.position.x + tower.mapXSize);
					camZ = Mathf.Clamp (camZ, tower.transform.position.z - tower.mapZSize, tower.transform.position.z + tower.mapZSize);

					blockPlace.transform.position = new Vector3(camX,
					                                            tower.transform.position.y,
					                                            camZ);

				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					// Draw the block "preview" before the mouse is released
					if (blockPlace == null)
					{
						blockPlace = tower.createBlock();
					}
				} else if (Input.GetMouseButtonUp(0))
				{
					// Wake up the block - time to drop
					blockPlace.GetComponent<Block>().Drop();
					
					// Reset the timer so the player has to wait a bit before spawning another block
					blockWaiting = blockWaitTime;
					blockPlace = null;
				}
				
				if (blockPlace)
				{
					if (Input.GetMouseButtonDown(1))
					{
						blockPlace.transform.RotateAround(blockPlace.transform.position, Vector3.up, snapAngle);
					}
					
					// Line tracing for "previews"
					Plane plane = new Plane(Vector3.up, 0);
					
					float distance;
					Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					if (plane.Raycast(ray, out distance))
					{
						Vector3 pt = ray.GetPoint(distance);
						// TODO: Predicting the place
						blockPlace.transform.position = new Vector3(Mathf.Clamp(pt.x, tower.transform.position.x - (tower.mapXSize/2), tower.transform.position.x + (tower.mapXSize/2)),
						                                            tower.transform.position.y,
						                                            Mathf.Clamp(pt.z, tower.transform.position.z - (tower.mapZSize/2), tower.transform.position.z + (tower.mapZSize/2)));
					}
				}
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
