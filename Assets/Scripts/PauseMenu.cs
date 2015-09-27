using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {
	
	bool paused = false;
	bool quit = false;
	bool help = false;
	DropperCamera getClick;
	GameManager getManager;
	/// <summary>
	/// Gets the component of the Dropper Camera 
	/// </summary>
	void Start()
	{
		Time.timeScale = 1f;
		getClick = GameObject.Find("IsoCamera").GetComponent<DropperCamera>();
		getManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	/// <summary>
	/// When you press the escape key it pauses the game
	/// </summary>
	void Update()
	{
		//if you press escape it will toggle between on and off
		if (Input.GetKeyDown (KeyCode.Escape)) {
			paused = togglePause ();
		} 
	}
	
	void OnGUI()
	{
		//if paused  = true
		if (paused) {
			GUI.Box ( new Rect (0, 0, Screen.currentResolution.width, Screen.currentResolution.height), "Pause Menu");
			// Make the Quit button.
			if (GUI.Button (new Rect (Screen.currentResolution.width / 2, (Screen.currentResolution.height / 2) + 100, 150, 35), "Quit")) {
				// quits game
				quit = true;
			}
			
			// Resumes the game when paused
			if (GUI.Button (new Rect (Screen.currentResolution.width / 2, (Screen.currentResolution.height / 2) - 100, 150, 35), "Back")) {
				paused = togglePause();
				
			}
			
			//Toggles between displaying the keys
			if (GUI.Toggle (new Rect (Screen.currentResolution.width / 2, (Screen.currentResolution.height / 2), 150, 35), help,"Help","Button")!= help) {
				help = !help;
			}
			if (help){
				//Debug.Log("Working");
				GUI.Label(new Rect (200, 100, 150, 35), "Blobbi Moves");
				GUI.Label(new Rect (200, 150, 150, 35), "Forward:  W");
				GUI.Label(new Rect (200, 200, 150, 35), "Backward: S");
				GUI.Label(new Rect (200, 250, 150, 35), "Left:     A");
				GUI.Label(new Rect (200, 300, 150, 35), "Right:    D");
				GUI.Label(new Rect (200, 350, 150, 35), "Jump:     Spacebar");
				
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 100, 300, 35), "Builders Moves");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 150, 300, 35), "Add Block:\tHold Left Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 200, 300, 35), "Drop Block:\tRealease Left Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 250, 300, 35), "Rotate Block:\tRight Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 300, 300, 35), "Rotate Screen:\tMouse Wheel");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 350, 300, 35), "Move Screen Up:\tMove Mouse Up");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 400, 300, 35), "Move Screen Down:\tMove Mouse Down");
			}
		}
		
		if(quit){
			paused = false;
			GUI.Box ( new Rect (0, 0, Screen.currentResolution.width, Screen.currentResolution.height), "Pause Menu");
			if (GUI.Button (new Rect (Screen.currentResolution.width / 2, (Screen.currentResolution.height / 2) - 100, 150, 35), "Yes")) {
				quit = false;
				togglePause();
				Application.LoadLevel(0);
				
			}
			if (GUI.Button (new Rect (Screen.currentResolution.width / 2, (Screen.currentResolution.height / 2) - 150, 150, 35), "No")) {
				quit = false;
				paused = true;
				
			}
			
		}
		
		
	}
	
	
	/// <summary>
	/// Toggles between a paused playing state.
	/// </summary>
	/// <returns><c>true</c>, if pause was toggled, <c>false</c> otherwise.</returns>
	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			// time scale = 1f which unpauses the game
			Time.timeScale = 1f;
			// enables the DropperCamera component so you can add blocks
			getClick.enabled = !getClick.enabled;
			return(false);
		}
		else
		{
			// time scale = 0f which pauses the game.
			Time.timeScale = 0f;
			// disables the DropperCamera component so people can't add blocks while the game is pause
			getClick.enabled = !getClick.enabled;
			return(true);    
		}
	}
	
	
	public void QuitGame()
	{
		paused = false;
		getClick.enabled = !getClick.enabled;
		Application.LoadLevel (0);	
	}
}
