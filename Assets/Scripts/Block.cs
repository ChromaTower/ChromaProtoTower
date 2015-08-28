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
	public Vector3 target = Vector3.zero;

	public float colourTime = 5f;
	private bool coloured = false;

	private TowerManager tower;


	private List<GameObject> shape = new List<GameObject>();

	// Sets up the tetronimo
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

	void tetronimoSetup()
	{
		int selection = Random.Range (0, 6);

		// Setup the tetronimo shape based on a random selection
		if (selection == 0) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 0f, 0f));
		} else if (selection == 1) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 0f, 1f, 0f));
		} else if (selection == 2) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
		} else if (selection == 3) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 1f, 0f));
		} else if (selection == 4) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 0f, 1f, 0f));
		} else if (selection == 5) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3( 1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3( 2f, 1f, 0f));
		} else if (selection == 6) {
			shape[1].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 0f, 0f));
			shape[2].transform.position = shape[0].transform.position + (snap * new Vector3(-1f, 1f, 0f));
			shape[3].transform.position = shape[0].transform.position + (snap * new Vector3(-2f, 1f, 0f));
		}
	}

	// Use this for initialization
	void Start () {
		tower = GameManager.instance.getTower();
		snap = tower.getSnap ();

		shape = createShape();
		tetronimoSetup();

		re = GetComponent<Renderer>();
		re.enabled = true;

		rb = GetComponent<Rigidbody>();

		if (falling == false && activated == false)
		{
			Snooze();
		}
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
			// Register a position in the tower blocklist for each cube
			foreach(GameObject o in shape)
			{
				Vector3 posDifference = new Vector3((int)((o.transform.position.x - transform.position.x) / snap), 
				    (int)((o.transform.position.y - transform.position.y) / snap), 
				     (int)((o.transform.position.z  - transform.position.z)/ snap));
				tower.registerPos(posDifference + target);
			}
			rb.WakeUp();
			rb.isKinematic = false;

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
		rb.detectCollisions = true;
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
	void OnCollisionEnter(Collision collision) {
		// Collide if active/falling
		if (rb.isKinematic == true && coloured == false)
		{
			if (collision.gameObject.tag == "Player")
			{
				Rigidbody prb = collision.gameObject.GetComponent<Rigidbody>();
				//if (prb.velocity.y <= 0 && (collision.gameObject.transform.position.y > transform.position.y))
				//{
					re.material.color = randomiseColor();
					coloured = true;
				//}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// Interactions when the block is placed down
		if (activated == true)
		{
			if (coloured == true)
			{
				colourTime -= Time.deltaTime;

				if (colourTime < 0)
				{
					tower.removeBlock(gameObject);
				}
			}
		}
		else if (activated == false) // Before the block is falling
		{
			int blockNum = 0;

			// All the blocks should follow a consistent position
			foreach(GameObject o in shape)
			{
				o.GetComponent<Renderer>().material = re.material;

				int xSnap = (int)(o.transform.position.x / snap);
				int ySnap = (int)(o.transform.position.y / snap);
				int zSnap = (int)(o.transform.position.z / snap);
			
				// Check if any parts of the tower are outside the world and snap accordingly
				// TODO: Clamp in TowerManager instead??
				if (tower.checkBounds(xSnap, ySnap, zSnap) == false)
				{
					print ("Block " + blockNum + "- (" + xSnap + ", " + ySnap + ", " + zSnap + ") is out of bounds: " + o.transform.position);
					// Move it back in boundaries
					if (xSnap < 0)
					{
						transform.position = new Vector3(transform.position.x - ((xSnap) * snap), transform.position.y, transform.position.z);
					} else if (xSnap >= tower.blockArraySizeX - 1) {
						transform.position = new Vector3(transform.position.x - ((xSnap - tower.blockArraySizeX + 1) * snap), transform.position.y, transform.position.z);
					}

					// Move it back in boundaries
					if (ySnap < 0)
					{
						transform.position = new Vector3(transform.position.x, transform.position.y - ((ySnap) * snap), transform.position.z);
					} else if (ySnap >= tower.blockArraySizeY - 1) {
						transform.position = new Vector3(transform.position.x, transform.position.y - ((ySnap - tower.blockArraySizeY + 1) * snap), transform.position.z);
					}

					// Move it back in boundaries
					if (zSnap < 0)
					{
						transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - ((zSnap) * snap));
					} else if (zSnap >= tower.blockArraySizeZ - 1) {
						transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - ((zSnap - tower.blockArraySizeZ + 1) * snap));
					}

					xSnap = (int)(o.transform.position.x / snap);
					ySnap = (int)(o.transform.position.y / snap);
					zSnap = (int)(o.transform.position.z / snap);

					print ("Block " + blockNum + " - Revised to (" + xSnap + ", " + ySnap + ", " + zSnap + "): " + o.transform.position);

				}

				//print ("Block " + blockNum + ": (" + (xSnap * snap) + ", " + (ySnap * snap) + ", " + (zSnap * snap) + "): " + o.transform.position);

				blockNum++;
			}
						
			if (falling == false)
			{
				List<float> yChange = new List<float>();
				// Check all the blocks in the shape to see how it should fall
				foreach(GameObject o in shape)
				{
					float xSnap = (int)(o.transform.position.x / snap);
					// Get the difference for this one, since we're spawning from the sky
					float ySnap = (int)((tower.getPos().y + (o.transform.position.y - transform.position.y)) / snap);
					float zSnap = (int)(o.transform.position.z / snap);
					yChange.Add(checkYPosition(new Vector3(xSnap, ySnap, zSnap)));
					//print ("Block " + blockNum + "- (" + xSnap + ", " + ySnap + ", " + zSnap + ")");
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
	
				transform.position = target * snap;
			} else {
				// Check if the block has fallen past its designated point
				if (transform.position.y < target.y * snap)
				{
					transform.position = target * snap;
					// Set velocity/angles/etc to zero just in case it got screwed at some point
					rb.velocity = Vector3.zero;
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);

					// The block is now stuck and jumpable
					activate();
				}
			}
		}
	}
}
