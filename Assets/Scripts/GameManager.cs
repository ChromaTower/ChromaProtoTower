using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;

	private TowerManager tower;
	private DropperCamera camera;
	private GameObject player;


	// Use this for initialization
	void Awake () {
		// Singleton stuff - there can only be one
		if (instance == null)
		{
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}

		DontDestroyOnLoad(gameObject);
		tower = GameObject.Find("Tower").GetComponent<TowerManager>();
		//camera = GameObject.Find("Camera").GetComponent<DropperCamera>();
		player = GameObject.Find("Player");
	}

	public GameObject getPlayer()
	{
		return player;
	}

	public TowerManager getTower()
	{
		return tower;
	}
	public DropperCamera getCamera()
	{
		return camera;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
