using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour {

	// The blocks used - prefab
	public GameObject block;
	private List<GameObject> blocks = new List<GameObject>();	

	// Effectively the block size, used in all block/position calculations
	public float snap = 2;

	// A 3D array containing positions (index) and if blocks are there
	// TODO: Make these values dynamic with level size
	public int mapXSize = 20;
	public int mapYSize = 100;
	public int mapZSize = 20;

	private int blockArraySizeX;
	private int blockArraySizeY;
	private int blockArraySizeZ;
	private bool[, ,] blockArray;



	// Use this for initialization
	void Start () {
		blockArraySizeX = (int)(mapXSize / snap);
		blockArraySizeY = (int)(mapYSize / snap);
		blockArraySizeZ = (int)(mapZSize / snap);
		blockArray = new bool[blockArraySizeX, blockArraySizeY, blockArraySizeZ];
	}

	public float getSnap()
	{
		return snap;
	}

	// Returns the highest block's y-coordinate
	public float getHeight()
	{
		float maxY = 0f;

		foreach(GameObject b in blocks)
		{
			if (b.transform.position.y > maxY)
			{
				maxY = b.transform.position.y;
			}
		}

		return maxY;
	}

	// Returns the list of blocks
	public List<GameObject> getBlocks()
	{
		return blocks;
	}

	// Attempts to remove a block
	public bool removeBlock(GameObject o)
	{
		if (blocks.Contains (o))
		{
			blocks.Remove (o);
			Destroy(o);
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

		if (blockArray[x, y, z] == true)
		{
			return true;
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
		if ((x >= 0 && x <= blockArraySizeX) && (y >= 0 && y <= blockArraySizeY) && (z >= 0 && z <= blockArraySizeZ))
		{
			//print ("(" + x + ", " + y + ", " + z + ")");
			blockArray[x, y, z] = true;
			return true;
		} else {
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
		if ((x >= 0 && x <= blockArraySizeX) && (y >= 0 && y <= blockArraySizeY) && (z >= 0 && z <= blockArraySizeZ))
		{
			blockArray[x, y, z] = false;
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
		transform.position = new Vector3(transform.position.x, getHeight() + 4f, transform.position.z);
	}

	public Vector3 getPos()
	{
		return transform.position;
	}
}
