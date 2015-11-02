using UnityEngine;
using System.Collections;
using GamepadInput;

public class DropperCamera : MonoBehaviour {

	// Used for placing new blocks on screen
	private GameObject blockPlace = null;
	private Vector3 lastBlockPos = Vector3.zero;
	int previousRotates = 0;
	int currentRotates = 0;

	// The increments the camera is snapped to when rotating
	// Not advised that you change this, but if you want to...
	public int snapAngle = 90;

	private float initAngle;

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

	// Moving up/down
	private float moveVerticalWaitTime = 0.25f;
	private float moveVerticalWaiting = 0f;

	private float gridMoveRate = 15f;

	private TowerManager tower;

	private float camMouseY = 0f;
	private float camX = 0f;
	private float camY = 0f;
	private float camZ = 0f;


	private float maxDistAboveTower;

	private float controllerInertia = 0f;


	// Use this for initialization
	void Start () {
		// Get the tower object
		tower = GameManager.instance.getTower();

		// Set the initial angle based on the camera's rotation
		initAngle = transform.eulerAngles.y;

		Camera camera = (Camera)GetComponent<Camera>();

		// Makes the map fit in the camera
		camera.orthographicSize = Mathf.Max(tower.maxX - tower.minX, tower.maxZ - tower.minZ) - 1f;

		transform.position = new Vector3(tower.transform.position.x - (tower.mapXSize/2), -12, tower.transform.position.z - (tower.mapZSize/2));
		maxDistAboveTower = camera.orthographicSize;


	}

	public void reset()
	{
		camMouseY = 0f;
		camX = 0f;
		camZ = 0f;
		camY = 0f;
	}

	public Vector3 getEulerAngles()
	{
		return transform.eulerAngles;
	}

	void VerticalMoveUpdate()
	{
		// Prevent camera from leaving boundaries
		TowerManager tower = GameManager.instance.getTower();

		GamepadState state = GamePad.GetState(GamePad.Index.Two);


		float scroll = 0;
		
		if (GameManager.instance.controllerBuilder)
		{
			if (state.RightShoulder)
			{
				scroll += 1;
			} else if (state.LeftShoulder)
			{
				scroll -= 1;
			}
		}

		float yMovement = scroll * 9f * Time.deltaTime;
		//Debug.LogWarning(yMovement);
		transform.position = new Vector3(transform.position.x, transform.position.y + yMovement, transform.position.z);

		if (transform.position.y < GameManager.instance.getShadow().transform.position.y + 3f)
		{
			transform.position = new Vector3(transform.position.x, GameManager.instance.getShadow().transform.position.y + 3f, transform.position.z);
		} else if (transform.position.y > tower.getHeight() + maxDistAboveTower) {
			transform.position = new Vector3(transform.position.x, tower.getHeight() + maxDistAboveTower, transform.position.z);
		}


		GameObject grid = GameManager.instance.getTower().previewGrid;
		// TODO: Change the magic values
		grid.transform.position = new Vector3(grid.transform.position.x, ((Mathf.Round (transform.position.y / tower.snap) - 5f) * tower.snap), grid.transform.position.z);
	}

	void rotateUpdate()
	{
		// Only rotate if the time has elapsed
		if (rotWaiting <= 0f)
		{
			int scroll;

			if (GameManager.instance.controllerBuilder)
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
				transform.RotateAround(tower.floor.transform.position, Vector3.up, rotAngle * ((snapAngle / rotWaitTime) * Time.deltaTime));
			} else {
				// Round the angle
				targetAngle = Mathf.Round((transform.eulerAngles.y - initAngle) / snapAngle) * snapAngle;
				transform.RotateAround(tower.floor.transform.position, Vector3.up, targetAngle - (transform.eulerAngles.y - initAngle));
			}
		}
	}

	void mouseUpdate()
	{
		TowerManager tower = GameManager.instance.getTower ();

		if (GamePad.GetButtonUp (GamePad.Button.X, GamePad.Index.Two))
		{
			tower.undoBlock ();
		}


		if (!blockPlace)
		{
			if (tower.blockEnergy > 0)
			{
				// Create a new block
				blockPlace = tower.createBlock();
				blockPlace.transform.position = lastBlockPos;
				//Debug.LogWarning(lastBlockRot);
				blockPlace.GetComponent<Block>().preview();


			}
		} else {
			// Reduce the time
			blockWaiting -= Time.deltaTime;

				if (GameManager.instance.controllerBuilder)
				{
						// Why isn't CamX-Z a Vector3 anyway?

						if (GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two))
						{
							blockPlace.transform.RotateAround(blockPlace.transform.position, Vector3.up, snapAngle);
							currentRotates += 1;
						}

				if (previousRotates > 0)
				{
					for (int i = 0; i <= previousRotates - 1; i++)
					{
						blockPlace.transform.RotateAround(blockPlace.transform.position, Vector3.up, snapAngle);
					}
					previousRotates = 0;
				}

						float leftStickX = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).x * Time.deltaTime * gridMoveRate;
						float leftStickY = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).y * Time.deltaTime * gridMoveRate;
						float dPadX = GamePad.GetAxis (GamePad.Axis.Dpad, GamePad.Index.Two).x * Time.deltaTime * gridMoveRate;
						float dPadY = GamePad.GetAxis (GamePad.Axis.Dpad, GamePad.Index.Two).y * Time.deltaTime * gridMoveRate;



				float controllerAngle = (Mathf.Atan2(leftStickY + (dPadY * 10f), leftStickX + (dPadX * 10f)) * Mathf.Rad2Deg) - transform.eulerAngles.y;
				float controllerMagnitude = (Mathf.Sqrt (((leftStickX + dPadX) * (leftStickX + dPadX)) + ((leftStickY + dPadY) * (leftStickY + dPadY)))) * controllerInertia;

				/*if (controllerAngle > 45f && controllerAngle < 135f)
				{
					controllerAngle = 90f;
				}

				if (controllerAngle > 45f && controllerAngle < 135f)
				{
					controllerAngle = 90f;
				}*/

				if (controllerInertia > 10f)
				{
					controllerInertia = 10f;
				} else {
					controllerInertia += Mathf.Max(0, 1f - (controllerMagnitude * 10f));
				}
				
				if (controllerInertia < 1f)
				{
					controllerInertia = 1f;
				}

				camX += Mathf.Cos(controllerAngle * Mathf.Deg2Rad) * 50 * Time.deltaTime * controllerMagnitude;
				camZ += Mathf.Sin(controllerAngle * Mathf.Deg2Rad) * 50 * Time.deltaTime * controllerMagnitude;

				/*Debug.LogWarning((Mathf.Cos(controllerAngle * Mathf.Deg2Rad) * 50 * Time.deltaTime * controllerMagnitude) + " | " + 
				                 (Mathf.Sin(controllerAngle * Mathf.Deg2Rad) * 50 * Time.deltaTime * controllerMagnitude));*/

						//camY += GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.Two).y * Time.deltaTime * 15f;



						Vector3 b = blockPlace.transform.position;
						if (camX > tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x + tower.snap, b.y, b.z);
							camX -= tower.snap;
							controllerInertia = 1f;
						} else if (camX < -tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x - tower.snap, b.y, b.z);
							camX += tower.snap;
							controllerInertia = 1f;
						}
						if (camZ > tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x, b.y, b.z + tower.snap);
							camZ -= tower.snap;
							controllerInertia = 0f;
						} else if (camZ < -tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x, b.y, b.z - tower.snap);
							camZ += tower.snap;
							controllerInertia = 1f;
						}
				}

			if (blockWaiting <= 0f)
			{
				if ((GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.Two)) || (Input.GetMouseButtonDown(0)))
				{
					blockPlace.GetComponent<Block>().preview();
				} else if ((GamePad.GetButtonUp (GamePad.Button.A, GamePad.Index.Two)) || (Input.GetMouseButtonUp(0))) {
					// Wake up the block - time to dropPlace();
					lastBlockPos = blockPlace.transform.position;
					blockPlace.GetComponent<Block>().Drop();
					previousRotates = currentRotates % 4;
					// Reset the timer so the player has to wait a bit before spawning another block
					blockWaiting = blockWaitTime;
					
					blockPlace = null;
				}
			}
		}

	}

	void cameraUpdate()
	{
		//TODO: Figure out why Z rotation is applied below 50 degrees on X
		Vector3 pos = new Vector3(tower.previewGrid.transform.position.x,
		                          tower.previewGrid.transform.position.y,
		                          tower.previewGrid.transform.position.z);
		transform.RotateAround(pos, Vector3.up, GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.Two).x * Time.deltaTime * 50f);
		transform.RotateAround(pos, transform.right, GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.Two).y * Time.deltaTime * 50f);

		transform.rotation = Quaternion.Euler(Mathf.Clamp (transform.rotation.eulerAngles.x, 1f, 80f), transform.rotation.eulerAngles.y, Mathf.Clamp(transform.rotation.eulerAngles.z, -1f, 1f));
	}

	// Update is called once per frame
	void Update () {
		//rotateUpdate();
		mouseUpdate();
		cameraUpdate();
		VerticalMoveUpdate ();
	}
}
