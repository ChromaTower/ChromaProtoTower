using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;

	private TowerManager tower;
	private DropperCamera camera;
	private CameraFollow playerCamera;
	private GameObject player;
	private GameObject blobbi;
	private GameObject shadow;

	// Are we using a controller or KB/mouse?
	public bool controller = false;

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
		// TODO: DON'T HARDCODE YOUR NAMES NICK YOU MORON
		camera = GameObject.Find("IsoCamera").GetComponent<DropperCamera>();
		playerCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
		player = GameObject.Find("Player");
		blobbi = GameObject.Find("Blobbi");
		shadow = GameObject.Find("ShadowGoo");
		// Ignore physical collisions between the blocks and the world
		// We're doing this manually instead
		Physics.IgnoreLayerCollision(0, 8);
		// Also blocks and other blocks
		Physics.IgnoreLayerCollision(8, 8);
	}

	public GameObject getPlayer()
	{
		return player;
	}

	public GameObject getBlobbi()
	{
		return blobbi;
	}

	public TowerManager getTower()
	{
		return tower;
	}
	public DropperCamera getBuilderCamera()
	{
		return camera;
	}

	public CameraFollow getPlayerCamera()
	{
		return playerCamera;
	}

	public GameObject getShadow()
	{
		return shadow;
	}

	void RestartLevel()
	{
		getTower ().reset();
		getPlayer ().GetComponent<PlayerMove>().reset();
		getShadow().GetComponent<ShadowManager>().reset();
		getBuilderCamera().reset();
	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey (KeyCode.Return))
		{
			RestartLevel();
		}
	}
}
