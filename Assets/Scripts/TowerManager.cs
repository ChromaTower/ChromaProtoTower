using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour {

	// The blocks used - prefab
	public GameObject block;
	private List<GameObject> blocks = new List<GameObject>();	

	public GameObject floor;

	public GameObject wall;
	private List<GameObject> walls = new List<GameObject>();	

	// Effectively the block size, used in all block/position calculations
	public float snap = 2;

	// A 3D array containing positions (index) and if blocks are there
	public float mapXSize = 20;
	public float mapYSize = 100;
	public float mapZSize = 20;

	public int blockArraySizeX;
	public int blockArraySizeY;
	public int blockArraySizeZ;
	private bool[, ,] blockArray;

	public float minX = 0;
	public float minY = 0;
	public float minZ = 0;

	public float maxX = 0;
	public float maxY = 0;
	public float maxZ = 0;



	// Use this for initialization
	void Start () {
		blockArraySizeX = (int)(mapXSize / snap);
		blockArraySizeY = (int)(mapYSize / snap);
		blockArraySizeZ = (int)(mapZSize / snap);
		blockArray = new bool[blockArraySizeX, blockArraySizeY, blockArraySizeZ];

		minX = transform.position.x - (mapXSize/2);
		minY = transform.position.y;
		minZ = transform.position.z - (mapZSize/2);

		maxX = transform.position.x + (mapXSize/2);
		maxY = transform.position.y + (mapYSize);
		maxZ = transform.position.z + (mapZSize/2);

		// Shift the floor if there is no clear "centre" as this means the blocks will be out of place
		float centreShiftX, centreShiftZ;
		if ((Mathf.FloorToInt(blockArraySizeX) % 2 == 0))
		{
			centreShiftX = snap / 2;
		} else {
			centreShiftX = 0;
		}

		if ((Mathf.FloorToInt(blockArraySizeZ) % 2 == 0))
		{
			centreShiftZ = snap / 2;
		} else {
			centreShiftZ = 0;
		}



		floor = (GameObject)Object.Instantiate(floor, transform.position - new Vector3(centreShiftX, snap/2, centreShiftZ), Quaternion.identity);
		floor.transform.localScale = new Vector3(mapXSize / 10 , 1, mapZSize / 10);
		floor.GetComponent<Renderer>().material.mainTextureScale = new Vector2(blockArraySizeX / 2, blockArraySizeZ / 2);

		for (int i = 0; i <= 3; i++)
		{
			GameObject w = (GameObject)Object.Instantiate(wall, floor.transform.position + new Vector3(0, mapYSize / 2f, 0f), Quaternion.identity);
			w.transform.Rotate(0f, i * 90f, 90f, Space.Self);
			w.transform.localScale = new Vector3(maxY/10f, 1, mapXSize/10f);
			if (i == 0)
			{
				w.transform.position += new Vector3(mapXSize / 2, 0f, 0f);
			} else if (i == 1) {
				w.transform.position += new Vector3(0f, 0f, -mapZSize / 2);
			} else if (i == 2) {
				w.transform.position += new Vector3(-mapXSize / 2, 0f, 0f);
			} else {
				w.transform.position += new Vector3(0f, 0f, mapZSize / 2);
			}


			walls.Add(w);
		}

	}

	public float getSnap()
	{
		return snap;
	}

	// Returns the highest block's y-coordinate
	public float getHeight()
	{
		float maxY = 0f;
		bool any = false;

		foreach(GameObject b in blocks)
		{
			any = true;

			if (b.transform.position.y > maxY)
			{
				maxY = b.transform.position.y + snap;
			}
		}

		if (any == false)
		{
			return 0;
		}

		return maxY;
	}

	// Returns the list of blocks
	public List<GameObject> getBlocks()
	{
		return blocks;
	}

	// Deregisters all the parts of a tetromino
	public void deregisterBlock(GameObject o)
	{
		foreach(GameObject s in o.GetComponent<Block>().getShape())
		{
			Vector3 posDifference = new Vector3((int)((s.transform.position.x - o.transform.position.x) / snap), 
			                                    (int)((s.transform.position.y - o.transform.position.y) / snap), 
			                                    (int)((s.transform.position.z - o.transform.position.z) / snap));
			deregisterPos(posDifference + o.GetComponent<Block>().getTarget());
		}
	}

	private void checkIntegrity()
	{
		// TODO: this
	}

	public void registerBlock(GameObject o)
	{
		foreach(GameObject s in o.GetComponent<Block>().getShape())
		{
			Vector3 posDifference = new Vector3((int)((s.transform.position.x - o.transform.position.x) / snap), 
			                                    (int)((s.transform.position.y - o.transform.position.y) / snap), 
			                                    (int)((s.transform.position.z  - o.transform.position.z)/ snap));
			registerPos(posDifference + o.GetComponent<Block>().getTarget());
		}
	}


	public void reset()
	{
		// Reset array
		foreach(GameObject b in getBlocks ())
		{
			if (b.GetComponent<Block>().isActivated())
			{
				blocks.Remove (b);
				Destroy (b);
			}
		}

		blockArray = new bool[blockArraySizeX, blockArraySizeY, blockArraySizeZ];
	}

	// Attempts to remove a block
	public bool removeBlock(GameObject o)
	{
		if (blocks.Contains (o))
		{
			if (o.GetComponent<Block>().isActivated())
			{
				deregisterBlock(o);
			}

			blocks.Remove (o);
			Destroy(o);
			return true;
		} else {
			return false;
		}
	}

	public bool checkBounds(int x, int y, int z)
	{
		// Return if the position is valid
		if ((x >= 0 && x <= blockArraySizeX - 1) && (y >= 0 && y <= blockArraySizeY - 1) && (z >= 0 && z <= blockArraySizeZ - 1))
		{
			return true;
		} else {
			return false;
		}
	}

	// Returns true/false depending on if an object is in a position
	public bool checkPos(Vector3 pos)
	{
		// Convert the vector to a series of integers
		int x = Mathf.Clamp ((int)pos.x, 0, blockArraySizeX);
		int y = Mathf.Clamp ((int)pos.y, 0, blockArraySizeY);
		int z = Mathf.Clamp ((int)pos.z, 0, blockArraySizeZ);

		//print ("Checking (" + x + ", " + y + ", " + z + ")");
		if (checkBounds(x, y, z))
		{
			if (blockArray[x, y, z] == true)
			{
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	// TODO: This is broken
	// Need to fix, or remove!
	public void printBlockArray()
	{
		string str = "Block Array: ";

		for (int z = 0; z < blockArray.GetLength(2); z++)
		{
				for (int x = 0; x < blockArray.GetLength(0); x++)
				{
					str += "Block: (" + x + ", " + z + "), ";
				}
		}

		print (str);
	}

	// Registers a position from the list, so an object is there
	public bool registerPos(Vector3 pos)
	{
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;
	
		// Only add the block if it's actually possible...

		if (checkBounds(x, y, z))
		{
			blockArray[x, y, z] = true;
			print ("(" + x + ", " + y + ", " + z + ") registered successfully.");
			return true;
		} else {
			print ("(" + x + ", " + y + ", " + z + ") failed.");
			return false;
		}
	}

	// Deregisters a position from the list, so no object is there
	public void deregisterPos(Vector3 pos)
	{
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;

		// Only remove the block if it's actually a valid index
		if (checkBounds(x, y, z))
		{
			blockArray[x, y, z] = false;
			checkIntegrity ();
		}
	}

	// Create a block in the manager's position
	public GameObject createBlock()
	{
		GameObject b = (GameObject)Object.Instantiate(block, transform.position, Quaternion.identity);
		blocks.Add(b);
	
		return b;

	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, getHeight() + 10f, transform.position.z);
	}

	// TODO: Get rid of this
	// Why is this here lol silly nick
	public Vector3 getPos()
	{
		return transform.position;
	}
}
