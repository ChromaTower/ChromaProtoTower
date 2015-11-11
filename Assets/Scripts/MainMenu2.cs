using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using GamepadInput;
using System.IO;

public class MainMenu2 : MonoBehaviour 
{
	MainMenu2 menu;
	public MainMenu menu2;
	public RawImage logo;
	public Button start;
	public Button quit;
	public Button help;
	public Button back;
	public Canvas quitMenu;
	public Canvas helpMenu;
	public Text playerInput;
	
	public Text score1;
	public Text score2;
	public Text score3;
	public Text score4;
	public Text score5;
	private List<string> scores; 
	private string path;
	private string file;
	private bool starting;
	private bool quitting;
	private bool y;
	private bool n;
	private bool h;
	private bool b;
	// Use this for initialization
	void Start () {
		menu = GetComponent<MainMenu2>();
		menu2 = menu2.GetComponent<MainMenu>();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		helpMenu = helpMenu.GetComponent<Canvas> ();
		start = start.GetComponent<Button>();
		quit = quit.GetComponent<Button>();
		help = help.GetComponent<Button>();
		playerInput = playerInput.GetComponent<Text>();
		menu.enabled = true;
		logo.enabled = true;
		quitMenu.enabled = false;
		helpMenu.enabled = false;
		starting = false;
		quitting = false;
		y = false;
		n = false;
		h = false;
		b = false;
		file = "HighScores.txt";
		scores = new List<string>();
		ReadScores ();
		
		score1.text = "1) " + scores[0].ToString() + "m";
		score2.text = "2) " + scores[1].ToString() + "m";
		score3.text = "3) " + scores[2].ToString() + "m";
		score4.text = "4) " + scores[3].ToString() + "m";
		score5.text = "5) " + scores[4].ToString() + "m";
		
	}
	
	// Update is called once per frame
	void Update () {
		if(menu2.Starting && starting){
			Application.LoadLevel(2);
		}
		
		
		if(menu2.Starting){
			playerInput.text = "Builder Wants To Play";
		}
		
		if(GamePad.GetButtonDown(GamePad.Button.B,GamePad.Index.Two)){
			Quit ();
		}
		
		if(GamePad.GetButtonUp(GamePad.Button.X,GamePad.Index.Two)){
			HelpMenu();
		}
		if(GamePad.GetButtonDown(GamePad.Button.Y,GamePad.Index.Two) ){
			Back();
		}
		
		if(y || menu2.Y){
			ExitGame();
			if(GamePad.GetButtonDown(GamePad.Button.Back,GamePad.Index.One)){
				NoQuit ();
			}
			
			if(GamePad.GetButtonDown(GamePad.Button.Start,GamePad.Index.One)){
				y = true;;
			}
			
			if(quitting && menu2.Quitting){
				Quiter();
			}
			
		}

		
	}
	
	/// <summary>
	/// Exits the game. When you click exit it opens up exit menu
	/// </summary>
	public void ExitGame(){
		quitting = true;
		y = false;
		n = false;
		quitMenu.enabled = true;
		start.enabled = false;
		quit.enabled = false;
		help.enabled = false;
		
	}
	
	/// <summary>
	/// Activates the Help Menu
	/// </summary>
	public void HelpMenu(){
		helpMenu.enabled = true;
		start.enabled = false;
		quit.enabled = false;
		help.enabled = false;
		b = true;
		
	}
	
	/// <summary>
	/// When they want to go back to the main menu
	/// </summary>
	public void Back(){
		helpMenu.enabled = false;
		start.enabled = true;
		quit.enabled = true;
		help.enabled = true;
		b = false;

	}
	
	/// <summary>
	/// if the players dont want to quit the game
	/// </summary>
	public void NoQuit(){
		n = true;
		quitting = false;
		quitMenu.enabled = false;
		start.enabled = true;
		quit.enabled = true;
		help.enabled = true;
		quitting = false;
	}
	
	/// <summary>
	/// Quit this game.
	/// </summary>
	public void Quit(){
		bool y = true;
	}
	
	/// <summary>
	/// Starts the game. Jumps to the next scene
	/// </summary>
	public void StartGame()
	{
		starting = true;
	}
	
	public bool Starting{
		get{return starting;}
	}
	
	public bool Quitting{
		get{return quitting;}
	}
	public bool N{
		get{return n;}
	}
	public bool Y{
		get{return y;}
	}
	public void ReadScores(){
		using(StreamReader sr = new StreamReader(file))
		{
			string line;
			while ( !sr.EndOfStream)
			{
				line = sr.ReadLine();
				scores.Add(line);
			}
			
		}
	}
	
	public void Quiter(){
		Application.LoadLevel(3);
	}
}
