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

	// Use this for initialization
	void Start () {
		// Get the tower object
		tower = GameManager.instance.getTower();

		// Set the initial angle based on the camera's rotation
		initAngle = transform.eulerAngles.y;

		// Create a new block to start
		blockPlace = tower.createBlock();


		Camera camera = (Camera)GetComponent<Camera>();

		// Makes the map fit in the camera
		camera.orthographicSize = Mathf.Max(tower.blockArraySizeX, tower.blockArraySizeZ) - 0.5f;

		transform.position = new Vector3(tower.transform.position.x - (tower.mapXSize/2), -8, tower.transform.position.z - (tower.mapZSize/2));
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
		transform.position = new Vector3(0f, camY + maxDistAboveTower, 0f);

		// Prevent camera from leaving boundaries
		TowerManager tower = GameManager.instance.getTower();

		if (transform.position.y < tower.minY)
		{
			transform.position = new Vector3(transform.position.x, tower.minY, transform.position.z);
		} else if (transform.position.y > tower.getHeight() + maxDistAboveTower) {
			transform.position = new Vector3(transform.position.x, tower.getHeight() + maxDistAboveTower, transform.position.z);
		}


		GameObject grid = GameManager.instance.getTower().previewGrid;
		// TODO: Change the magic values
		grid.transform.position = new Vector3(grid.transform.position.x, (Mathf.Round(camY / tower.snap) * tower.snap) - (tower.snap / 2), grid.transform.position.z);
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
		// TODO: Get rid of this soon
		if (!GameManager.instance.controller)
		{
			if (Input.mousePosition.y > (Screen.height - 100))
			{
				camMouseY += Time.deltaTime * ((Input.mousePosition.y - (Screen.height - 100))/100) ;
			} else if (Input.mousePosition.y < 100)
			{
				camMouseY -= Time.deltaTime * ((100 - Input.mousePosition.y)/100) ;
			}
		} 


		if ((GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.Two)) || (Input.GetMouseButtonDown(0)))
		{
			blockPlace.GetComponent<Block>().preview();
		} else if ((GamePad.GetButtonUp (GamePad.Button.A, GamePad.Index.Two)) || (Input.GetMouseButtonUp(0))) {
			if (blockWaiting <= 0f)
			{
				// Wake up the block - time to dropPlace();
				blockPlace.GetComponent<Block>().Drop();
				
				// Reset the timer so the player has to wait a bit before spawning another block
				blockWaiting = blockWaitTime;
				
				// Create a new block
				blockPlace = tower.createBlock();
			}
		} else {		
			// Reduce the timer
			blockWaiting -= Time.deltaTime;

			if (GameManager.instance.controller)
			{
				if (blockPlace)
				{
					if (GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two))
					{
						blockPlace.transform.RotateAround(blockPlace.transform.position, Vector3.up, snapAngle);
					}

					float leftStickX = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).x * Time.deltaTime * gridMoveRate;
					float leftStickY = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Two).y * Time.deltaTime * gridMoveRate;
					float dPadX = GamePad.GetAxis (GamePad.Axis.Dpad, GamePad.Index.Two).x * Time.deltaTime * gridMoveRate;
					float dPadY = GamePad.GetAxis (GamePad.Axis.Dpad, GamePad.Index.Two).y * Time.deltaTime * gridMoveRate;

					// As the camera rotates, it has to be relative
					if (transform.eulerAngles.y >= 270)
					{
						camX -= leftStickY;
						camZ += leftStickX;

						camX -= dPadY;
						camZ += dPadX;
					} else if (transform.eulerAngles.y >= 180) {
						camX -= leftStickX;
						camZ -= leftStickY;

						camX -= dPadX;
						camZ -= dPadY;
					} else if (transform.eulerAngles.y >= 90) {
						camX += leftStickY;
						camZ -= leftStickX;

						camX += dPadY;
						camZ -= dPadX;
					} else {
						camX += leftStickX;
						camZ += leftStickY;

						camX += dPadX;
						camZ += dPadY;
					}

					camY += GamePad.GetAxis (GamePad.Axis.RightStick, GamePad.Index.Two).y * Time.deltaTime * 15f;

					TowerManager tower = GameManager.instance.getTower ();
					camX = Mathf.Clamp (camX, tower.transform.position.x - (tower.mapXSize / 2), tower.transform.position.x + (tower.mapXSize / 2));
					camY = Mathf.Clamp (camY, tower.transform.position.y - (tower.mapYSize / 2), tower.transform.position.y + (tower.mapYSize / 2));
					camZ = Mathf.Clamp (camZ, tower.transform.position.z - (tower.mapZSize / 2), tower.transform.position.z + (tower.mapZSize / 2));
					blockPlace.transform.position = new Vector3(camX, camY, camZ);
				}
			}
			else
			{
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
						                                            tower.previewGrid.transform.position.y,
						                                            Mathf.Clamp(pt.z, tower.transform.position.z - (tower.mapZSize/2), tower.transform.position.z + (tower.mapZSize/2)));
					}
				}
			}
		}

	}

	// Update is called once per frame
	void Update () {
		VerticalMoveUpdate ();
		rotateUpdate();
		mouseUpdate();
	}
}
