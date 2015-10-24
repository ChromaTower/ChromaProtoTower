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

		// Selects the start button for player one
		if (!quitting && GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One)){
			playerInput.text = "Player Two Please Select an Option";
			s1 = true;
		}

		// Selects the start button for player two
		if (!quitting &&GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Two)){
			playerInput.text = "Player One Please Select an Option";
			s2 = true;
		}


		// if both players select start games goes to the next scene
		if(s1 && s2){
			StartGame();
		}


		// either player selects the quit button
		if (GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.Two) || GamePad.GetButtonUp(GamePad.Button.B, GamePad.Index.One)){
			quitting = true;
			s1 = false;
			s2= false;
		}

		if (quitting) {
			ExitGame();
			// if player one presses a not to quit
			if (quitting && GamePad.GetButtonUp (GamePad.Button.A, GamePad.Index.One)) {
				n1 = true;
			}
			// if player two presses a not to quit
			if (quitting && GamePad.GetButtonUp (GamePad.Button.A, GamePad.Index.Two)) {
				
				n2 = true;
			}

			// if player one presses b wants to quit
			if (quitting && GamePad.GetButtonUp (GamePad.Button.B, GamePad.Index.One)) {
				playerInput.text = "Player One Would Like To Quit";
				q1 = true;
			}

			// if player two presses b wants to quit
			if (quitting && GamePad.GetButtonUp (GamePad.Button.B, GamePad.Index.Two)) {
				playerInput.text = "Player Two Would Like To Quit";
				q2 = true;
			}

			//if both players want to quit
			if(q1 && q2){
				Quit ();
			}
			// if either player doesnt want to quit or both. resets to main menu
			if(n1 && n2 || n1 && q2 || n1 && q1 || q2 && n2){
				quitting = false;
				q1 = false;
				q2 = false;
				n1 = false;
				n2 = false;
				playerInput.text = "";
				NoQuit();
			}
		}
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
