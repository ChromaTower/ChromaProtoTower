using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using GamepadInput;

public class MainMenu : MonoBehaviour 
{
	MainMenu menu;
	public RawImage logo;
	public Button start;
	public Button quit;
	public Button help;
	public Button back;
	public Canvas quitMenu;
	public Canvas helpMenu;
	public Text playerInput;

	public bool s1 = false;
	public bool s2 = false;

	public bool quitting = false;
	bool q1 = false;
	bool q2 = false;
	bool n1 = false;
	bool n2 = false;
	
	
	// Use this for initialization
	void Start () {
		menu = GetComponent<MainMenu>();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		helpMenu = helpMenu.GetComponent<Canvas> ();
		start = start.GetComponent<Button>();
		quit = quit.GetComponent<Button>();
		help = help.GetComponent<Button>();
		menu.enabled = true;
		logo.enabled = true;
		quitMenu.enabled = false;
		helpMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	/// <summary>
	/// Exits the game. When you click exit it opens up exit menu
	/// </summary>
	public void ExitGame(){
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
	}

	/// <summary>
	/// When they want to go back to the main menu
	/// </summary>
	public void Back(){
		helpMenu.enabled = false;
		start.enabled = true;
		quit.enabled = true;
		help.enabled = true;
	}

	/// <summary>
	/// if the players dont want to quit the game
	/// </summary>
	public void NoQuit(){
		quitMenu.enabled = false;
		start.enabled = true;
		quit.enabled = true;
		help.enabled = true;
	}

	/// <summary>
	/// Quit this game.
	/// </summary>
	public void Quit(){
		Application.Quit();
	}
	
	/// <summary>
	/// Starts the game. Jumps to the next scene
	/// </summary>
	public void StartGame()
	{
		Application.LoadLevel (2);	
	}
	
}
