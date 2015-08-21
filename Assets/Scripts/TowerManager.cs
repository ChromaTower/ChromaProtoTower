using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour {

	// The blocks used - prefab
	public GameObject block;
	private List<GameObject> blocks = new List<GameObject>();	


	// Use this for initialization
	void Start () {
	
	}

	// Returns the highest block's y-coordinate
	public float getHeight()
	{
		float maxY = 0f;

		foreach(GameObject b in blocks)
		{
			if (b.GetComponent<BlockManage>().active)
			{
				if (b.transform.position.y > maxY)
				{
					maxY = b.transform.position.y;
				}
			}
		}

		return maxY;
	}

	public GameObject createBlock()
	{
		GameObject b = (GameObject)Object.Instantiate(block, transform.position, Quaternion.identity);
		blocks.Add(b);
	
		return b;

	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, getHeight() + 2f, transform.position.z);
	}
}
