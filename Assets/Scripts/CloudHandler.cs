using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudHandler : MonoBehaviour {

	public int numClouds = 50;

	public GameObject cloud1;
	public GameObject cloud2;
	public GameObject cloud3;
	public GameObject cloud4;
	public GameObject cloud5;

	private List<GameObject> clouds = new List<GameObject>();

	public List<GameObject> getCloudList()
	{
		return clouds;
	}


	void AddCloud()
	{
		int selection = Random.Range (1, 5);
		GameObject select;

		if (selection == 1)
		{
			select = cloud1;
		} else if (selection == 2)
		{
			select = cloud2;
		} else if (selection == 3)
		{
			select = cloud3;
		} else if (selection == 4)
		{
			select = cloud4;
		} else {
			select = cloud5;
		}

		GameObject cl = (GameObject)Object.Instantiate(select, 
			                               new Vector3(0f, Random.Range (20f, 100f), Random.Range (110f, 200f)),
			                               Quaternion.identity);

		float xScale = Random.Range (20f, 80f);
		float yScale = xScale * Random.Range (0.4f, 1f);
		cl.transform.localScale = new Vector3(xScale, yScale, 1f);

		cl.transform.RotateAround(Vector3.zero, Vector3.up, Random.Range (0f, 360f));

		cl.transform.LookAt(Vector3.zero);

		clouds.Add(cl);
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numClouds; i++)
		{
			AddCloud ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(0f, GameManager.instance.getPlayerCamera().transform.position.y, 0f);

		foreach (GameObject c in clouds)
		{
			c.transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Sqrt ((160f - (c.transform.localScale.x + c.transform.localScale.y)))/ 800f);
		}
	}
}
