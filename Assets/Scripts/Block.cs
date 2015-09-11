using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

	private Rigidbody rb;
	public Renderer re;

	float snap;

	private bool falling = false;
	private bool activated = false;

	// target positions
	private Vector3 target = Vector3.zero;

	public float resetColourTime = 5f;
	private float colourTime;
	private bool coloured = false;

	private TowerManager tower;

	private Vector3 shakePosition = new Vector3(0f, 0f, 0f);


	private List<GameObject> shape = new List<GameObject>();

	// Returns the block array
	public List<GameObject> getShape()
	{
		return shape;
	}


	// Sets up the tetromino
	List<GameObject> createShape()
	{
		List<GameObject> result = new List<GameObject>();

		// Add 4 cubes to the shape
		for (int i = 0; i < 4; i++)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = transform;
			cube.transform.localScale = new Vector3(snap, snap, snap);
			cube.transform.position = transform.position;
			//cube.AddComponent<Rigidbody>();
			result.Add(cube);
		}

		return result;
	}

	void tetrominoSetup()
	{
		int selection = Random.Range (0, 6);

		// Setup the tetromino shape based on a random selection
		if (selection == 0) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 3f, 0f, 0f));
		} else if (selection == 1) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 0f, 0f));
		} else if (selection == 2) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
		} else if (selection == 3) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 0f, 1f, 0f));
		} else if (selection == 4) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 0f, 1f, 0f));
		} else if (selection == 5) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 1f, 0f));
		}
	}

	// Use this for initialization
	void Start () {
		tower = GameManager.instance.getTower();
		snap = tower.getSnap ();

		shape = createShape();
		tetrominoSetup();

		re = GetComponent<Renderer>();
		re.enabled = true;

		rb = GetComponent<Rigidbody>();

		if (falling == false && activated == false)
		{
			Snooze();
		}
	}

	// Returns the target (registered) position
	public Vector3 getTarget()
	{
		return target;
	}

	// Returns if the block is activated (jumpable)
	public bool isActivated()
	{
		return activated;
	}

	// Set the transparency of the object
	public void setAlpha(float alpha)
	{
		if (alpha < 0f)
		{
			alpha = 0f;
		}
		if (alpha > 1f)
		{
			alpha = 1f;
		}

		re.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
	}

	// Makes the object motionless and transparent
	public void Snooze()
	{
		rb.isKinematic = true;
		rb.detectCollisions = false;
		setAlpha(0.5f);

		falling = false;
	}
	
	// Allows the object to move and deactivates transparency
	public void Drop()
	{
		// Only drop the block if it's in a valid position
		if (tower.checkBounds ((int)(target.x), (int)(target.y), (int)(target.z)))
		{
			tower.registerBlock(gameObject);
			rb.WakeUp();
			rb.isKinematic = false;
			rb.detectCollisions = true;
			setAlpha(1f);

			// Restore position 
			transform.position = new Vector3(transform.position.x, tower.getPos().y, transform.position.z);
			falling = true;
		} else {
			tower.removeBlock (gameObject);
		}

	}

	public void activate()
	{
		activated = true;
		rb.isKinematic = true;
	}

	// Looks up the block list to find the lowest position a block will fall to
	float checkYPosition(Vector3 startPos)
	{
		// Returns the lowest y-snap
		float result = startPos.y;
		//print (startPos);

		// Start from the block's position and check downward
		for (int yCheck = (int)result; yCheck >= 0; yCheck--)
		{
			Vector3 checking = new Vector3(startPos.x, yCheck, startPos.z);
			if (tower.checkPos(checking) == true)
			{
				break;
			} else {
				result = yCheck;
			}
		}
		return result;
	}

	Color randomiseColor()
	{
		// TODO: HSV to RGB sometime
		// For now, this will work
		float r = Random.Range(0, 255);
		float g = Random.Range(0, 255);
		float b = Random.Range(0, 255);
		float total = r + g + b;
		return new Color(r / total, g / total, b / total);
	}

	// Colourise on player touch
	void OnCollisionStay(Collision collision) {
		// Collide if active/falling
		if (rb.isKinematic == true) // && coloured == false) // DISABLED so timer resets constantly
		{
			// Turn black if touched by goo
			if (collision.gameObject.tag == "Shadow")
			{
				re.material.color = new Color(0f, 0f, 0f);
				coloured = false;
			}
			if (collision.gameObject.tag == "Player")
			{
				Rigidbody prb = collision.gameObject.GetComponent<Rigidbody>();

				// TODO: Use floorchecks/loop through children
				//if (prb.velocity.y < 0)
				//{
					re.material.color = GameManager.instance.getBlobbi().GetComponent<BlobbiManager>().getColor ();
					coloured = true;
				colourTime = resetColourTime; // TODO: Standardise the goddamn spelling
				//}
			}

		}
	}

	// Checks if parts of the block are out of bounds, and moves it within the boundaries if needed
	void checkBlockBounds()
	{
		// The required amounts to move blocks
		float xShift = 0;
		float yShift = 0;
		float zShift = 0;

		int blockNum = 0;

		foreach(GameObject o in shape)
		{
			blockNum++;
			// Get the snapped-position of the block
			int xSnap = (int)(o.transform.position.x / snap);
			int ySnap = (int)(o.transform.position.y / snap);
			int zSnap = (int)(o.transform.position.z / snap);
			
			//print ("Block " + blockNum + "- " + o.transform.position);

				// Move it back in boundaries
				if (o.transform.position.x < tower.minX)
				{
					// Adjust it only if it's the lowest value (so far)
					float shiftAttempt = -o.transform.position.x;
					
					if (shiftAttempt > xShift)
					{
						xShift = shiftAttempt;
					}
				} else if (o.transform.position.x > tower.maxX) {
					// Adjust it only if it's the lowest value (so far)
					float shiftAttempt = ((tower.blockArraySizeX - 1)*snap) - o.transform.position.x;

					if (shiftAttempt < xShift)
					{
						xShift = shiftAttempt;
					}
				}
				
				// Move it back in boundaries
				if (o.transform.position.y < tower.minY)
				{
					// Adjust it only if it's the lowest value (so far)
					float shiftAttempt = -o.transform.position.y;

					if (shiftAttempt > yShift)
					{
						yShift = shiftAttempt;
					}
				} else if (o.transform.position.y > tower.maxY) {
					float shiftAttempt = ((tower.blockArraySizeY - 1)*snap) - o.transform.position.y;

					if (shiftAttempt < yShift)
					{
						yShift = shiftAttempt;
					}
				}
				
				// Move it back in boundaries
				if (o.transform.position.z < tower.minZ)
				{
					// Adjust it only if it's the lowest value (so far)
					float shiftAttempt = -o.transform.position.z;

					if (shiftAttempt > zShift)
					{
						zShift = shiftAttempt;
					}
				} else if (o.transform.position.z > tower.maxZ) {
					float shiftAttempt = ((tower.blockArraySizeZ - 1)*snap) - o.transform.position.z;

					if (shiftAttempt < zShift)
					{
						zShift = shiftAttempt;
					}
				}
		}

		// If any movement is required to move the block back inside the boundaries, do so
		transform.position += new Vector3(xShift, yShift, zShift);
	}

	// Update is called once per frame
	void Update () {
		// Interactions when the block is placed down
		if (activated == true)
		{
			if (coloured == true)
			{
				colourTime -= Time.deltaTime;

				if (colourTime < 1.5f)
				{
					shakePosition = new Vector3((float)Random.Range(-15 - (colourTime *10), 15 - (colourTime *10))/30,
					                            (float)Random.Range(-15 - (colourTime *10), 15 - (colourTime *10))/30,
					                            (float)Random.Range(-15 - (colourTime *10), 15 - (colourTime *10))/30);
					transform.position = (target * snap) + shakePosition;
				}

				if (colourTime < 0)
				{
					tower.removeBlock(gameObject);
				}
			}
		}
		else if (activated == false) // Before the block is falling
		{
			// All the blocks should follow a consistent position
			int blockNum = 0;
			foreach(GameObject o in shape)
			{
				o.GetComponent<Renderer>().material = re.material;
			}
						
			if (falling == false)
			{
				// Move the block inside the boundaries
				checkBlockBounds();

				List<float> yChange = new List<float>();
				// Check all the blocks in the shape to see how it should fall
				blockNum = 0;

				transform.position = new Vector3(Mathf.Round(transform.position.x / (float)snap) * snap,
				                                 Mathf.Round(tower.transform.position.y/ (float)snap) * snap,
				                                 Mathf.Round(transform.position.z / (float)snap) * snap);

				foreach(GameObject o in shape)
				{
					blockNum++;

					int xSnap = (int)(o.transform.position.x / snap);
					// Get the difference for this one, since we're spawning from the sky
					int ySnap = (int)(o.transform.position.y / snap);
					int zSnap = (int)(o.transform.position.z / snap);

					yChange.Add(checkYPosition(new Vector3(xSnap, ySnap, zSnap)));
				}


				// We are finding the lowest point the block can logically fall to
				float maxFall = 0;
				foreach (float f in yChange)
				{
					if (f > maxFall)
					{
						maxFall = f;
					}
				}

				// Set the target accordingly
				target = new Vector3(Mathf.Round(transform.position.x / (float)snap),
				                                 maxFall,
				                                 Mathf.Round(transform.position.z / (float)snap));
	
				transform.position = new Vector3(transform.position.x, maxFall * snap, transform.position.z);
			} else {
				// Check if the block has fallen past its designated point
				if (transform.position.y < target.y * snap)
				{
					transform.position = target * snap;
					// Set velocity/angles/etc to zero just in case it got screwed at some point
					rb.velocity = Vector3.zero;
					transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

					// The block is now stuck and jumpable
					activate();
				}
			}
		}
	}
}
