using UnityEngine;
using System.Collections;
using GamepadInput;

public class DropperCamera : MonoBehaviour {

	// Used for placing new blocks on screen
	private GameObject blockPlace = null;
	private Vector3 lastBlockPos = Vector3.zero;

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
		transform.position = new Vector3(0f, (Mathf.Round(camY/ tower.snap) * tower.snap) + maxDistAboveTower, 0f);

		if (transform.position.y < tower.minY)
		{
			transform.position = new Vector3(transform.position.x, tower.minY, transform.position.z);
		} else if (transform.position.y > tower.getHeight() + maxDistAboveTower) {
			transform.position = new Vector3(transform.position.x, tower.getHeight() + maxDistAboveTower, transform.position.z);
		}


		GameObject grid = GameManager.instance.getTower().previewGrid;
		// TODO: Change the magic values
		//grid.transform.position = new Vector3(grid.transform.position.x, ((Mathf.Round (transform.position.y / tower.snap) - 5f) * tower.snap), grid.transform.position.z);
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

		// TODO: Get rid of this soon
		if (!GameManager.instance.controllerBuilder)
		{
			if (Input.mousePosition.y > (Screen.height - 100))
			{
				camMouseY += Time.deltaTime * ((Input.mousePosition.y - (Screen.height - 100))/100) ;
			} else if (Input.mousePosition.y < 100)
			{
				camMouseY -= Time.deltaTime * ((100 - Input.mousePosition.y)/100) ;
			}
		} 


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



						Vector3 b = blockPlace.transform.position;
						if (camX > tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x + tower.snap, b.y, b.z);
							camX -= tower.snap;
						} else if (camX < -tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x - tower.snap, b.y, b.z);
							camX += tower.snap;
						}
						if (camZ > tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x, b.y, b.z + tower.snap);
							camZ -= tower.snap;
						} else if (camZ < -tower.snap)
						{
							blockPlace.transform.position = new Vector3(b.x, b.y, b.z - tower.snap);
							camZ += tower.snap;
						}
				}
				else
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
							                                            Mathf.Clamp(pt.x, tower.transform.position.y - (tower.mapYSize/2), tower.transform.position.y + (tower.mapYSize/2)),
							                                            Mathf.Clamp(pt.z, tower.transform.position.z - (tower.mapZSize/2), tower.transform.position.z + (tower.mapZSize/2)));
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
					
					// Reset the timer so the player has to wait a bit before spawning another block
					blockWaiting = blockWaitTime;
					
					blockPlace = null;
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
