using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour 
{
	MainMenu menu;
	public RawImage logo;
	public Button start;
	public Button quit;
	public Canvas quitMenu;
	public Text score1;
	public Text score2;
	public Text score3;
	public Text score4;
	public Text score5;
	private List<float> scores; 
	private string path;
	private string file;
	
	// Use this for initialization
	void Start () {
		menu = GetComponent<MainMenu>();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		start = start.GetComponent<Button>();
		quit = quit.GetComponent<Button>();
		menu.enabled = true;
		logo.enabled = true;
		quitMenu.enabled = false;
		scores = new List<float>();
		path = "C:/Users/Public/Documents/ChromaTower";
		file = "C:/Users/Public/Documents/ChromaTower/HighScores.txt";
		
		
		ReadScores ();
		
		score1.text = "1) " + scores[0].ToString() + "m";
		score2.text = "2) " + scores[1].ToString() + "m";
		score3.text = "3) " + scores[2].ToString() + "m";
		score4.text = "4) " + scores[3].ToString() + "m";
		score5.text = "5) " + scores[4].ToString() + "m";
		
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void ExitGame(){
		quitMenu.enabled = true;
		start.enabled = false;
		quit.enabled = false;
	}
	
	public void NoQuit(){
		quitMenu.enabled = false;
		start.enabled = true;
		quit.enabled = true;
	}
	public void Quit(){
		Application.Quit();
	}
	
	
	public void StartGame()
	{
		Application.LoadLevel (2);	
	}
	
	
	public void ReadScores(){
		using(StreamReader sr = new StreamReader(file))
		{
			string line;
			while ( !sr.EndOfStream)
			{
				line = sr.ReadLine();
				scores.Add(float.Parse(line));
			}
			
		}
	}
	
	
}
