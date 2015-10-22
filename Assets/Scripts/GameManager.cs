using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	
	private GameObject tower;
	private TowerManager towerComp;
	private GameObject camera;
	private GameObject playerCamera;
	private GameObject player;
	private GameObject blobbi;
	private GameObject shadow;
	private List<string> scores;
	private string file;
	
	
	// Are we using a controller or KB/mouse?
	public bool controllerBlobbi = false;
	public bool controllerBuilder = false;
	
	void OnLevelWasLoaded(int level) {
		tower = GameObject.Find("Tower");
		towerComp = tower.GetComponent<TowerManager>();
		// TODO: DON'T HARDCODE YOUR NAMES NICK YOU MORON
		camera = GameObject.Find("IsoCamera");
		playerCamera = GameObject.Find("Main Camera");
		player = GameObject.Find("Player");
		blobbi = GameObject.Find("Blobbi");
		shadow = GameObject.Find("ShadowGoo");
		// Ignore physical collisions between the blocks and the world
		// We're doing this manually instead
		Physics.IgnoreLayerCollision(0, 8);
		// Also blocks and other blocks
		Physics.IgnoreLayerCollision(8, 8);
		scores = new List<string>();
		file = "C:/Users/Public/Documents/ChromaTower/HighScores.txt";
		ReadScores ();
		
		
	} 
	
	// Use this for initialization
	void Awake () {
		// Singleton stuff - there can only be one
		if (instance == null)
		{
			instance = this;
		} else if (instance != this) {
			Destroy(instance);    
		}
		
		//DontDestroyOnLoad(instance);
		OnLevelWasLoaded (1);
		Debug.Log (float.Parse(scores[0]));
		Debug.Log (float.Parse(scores[1]));
		Debug.Log (float.Parse(scores[2]));
		Debug.Log (float.Parse(scores[3]));
		Debug.Log (float.Parse(scores[4]));
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
		return towerComp;
	}

	public GameObject getTowerObject()
	{
		return tower;
	}

	public GameObject getBuilderCamera()
	{
		return camera;
	}
	
	public GameObject getPlayerCamera()
	{
		return playerCamera;
	}
	
	public GameObject getShadow()
	{
		return shadow;
	}
	
	void RestartLevel()
	{	
		string line = (((getPlayer().transform.position.y + 0.7f) * 5/5)/ 2).ToString ("0.0");
		CheckScores (float.Parse(line));
		SaveHighScore ();
		Application.LoadLevel (Application.loadedLevelName);
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (Input.GetKey (KeyCode.Return))
		{
			RestartLevel();
		}
	}
	
	/// <summary>
	/// Reads the scores from a text file.
	/// </summary>
	public void ReadScores(){
		using(StreamReader sr = new StreamReader(file))
		{
			string line;
			
			// checks to see if you have reached the end of the file
			while (!sr.EndOfStream)
			{
				line = sr.ReadLine();
				scores.Add(line);
			}
		}
	}
	
	
	/// <summary>
	/// Checks the scores. This goes through the loop and checks to see
	/// if you score has beaten any of the other scores.
	/// </summary>
	/// <param name="scoreCheck">Score check.</param>
	public void CheckScores(float scoreCheck){
		if(scoreCheck > float.Parse(scores[0])){
			scores[4] = scores[3];
			scores[3] = scores[2];
			scores[2] = scores[1];
			scores[1] = scores[0];
			scores[0] = scoreCheck.ToString();
		}else if(scoreCheck > float.Parse(scores[1])){
			scores[4] = scores[3];
			scores[3] = scores[2];
			scores[2] = scores[2];
			scores[1] = scoreCheck.ToString();
		}else if(scoreCheck > float.Parse(scores[2])){
			scores[4] = scores[3];
			scores[3] = scores[2];
			scores[2] = scoreCheck.ToString();
		}else if(scoreCheck > float.Parse(scores[3])){
			scores[4] = scores[3];
			scores[3] = scoreCheck.ToString();
		}else if(scoreCheck > float.Parse(scores[4])){
			scores[4] =  scoreCheck.ToString();
		} else {
			Debug.Log("These aren't the scores you are looking for!");
		}
	}
	
	
	public void SaveHighScore(){
		using(StreamWriter sw = new StreamWriter(file))
		{
			foreach (string line in scores)
			{
				sw.WriteLine(line);
			}
		}
	}
}
