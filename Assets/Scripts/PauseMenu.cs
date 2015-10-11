using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using GamepadInput;

public class PauseMenu : MonoBehaviour {
	
	bool paused = false;
	bool p1 = false;
	bool p2 = false;
	bool r1 = false;
	bool r2 = false;
	bool q1 = false;
	bool q2 = false;
	bool t1= false;
	bool t2 = false;
	bool quit = false;
	bool help1 = false;
	bool help2 = false;
	DropperCamera getClick;
	
	/// <summary>
	/// Gets the component of the Dropper Camera 
	/// </summary>
	void Start()
	{
		Time.timeScale = 1f;
		getClick = GameObject.Find("IsoCamera").GetComponent<DropperCamera>();
	}
	
	/// <summary>
	/// When you press the escape key it pauses the game
	/// </summary>
	void Update()
	{
		//if you press escape it will toggle between on and off
		if (Input.GetKeyDown (KeyCode.Escape) || GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One) || GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two)) {
			paused = togglePause ();
		} 
	}
	
	void OnGUI()
	{
		//if paused  = true
		if (paused) {
			
			GUI.Box ( new Rect (0, 0, (Screen.currentResolution.width/2), (Screen.currentResolution.height)), "Pause Menu Player One");
			GUI.Box ( new Rect ((Screen.currentResolution.width/2), 0, (Screen.currentResolution.width/2), (Screen.currentResolution.height)), "Pause Menu Player Two");
			// Make the Quit button.
			if (GUI.Button (new Rect (330, 600, 150, 35), "Quit" )|| GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One)) {
				// quits game
				q1 = true;
			}
			
			// Resumes the game when paused
			if (GUI.Toggle (new Rect (330, 400, 150, 35), r1,"Resume","Button")!= r1 || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.One)) {
				r1 = !r1;
			}
			
			//Toggles between displaying the keys
			if (GUI.Toggle (new Rect (330, 500, 150, 35), help1,"Help","Button")!= help1 || GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.One)) {
				help1 = !help1;
			}
			if (help1){
				//Debug.Log("Working");
				GUI.Label(new Rect (100, 100, 150, 35), "Blobbi Moves");
				GUI.Label(new Rect (100, 150, 150, 35), "Forward:  W");
				GUI.Label(new Rect (100, 200, 150, 35), "Backward: S");
				GUI.Label(new Rect (100, 250, 150, 35), "Left:     A");
				GUI.Label(new Rect (100, 300, 150, 35), "Right:    D");
				GUI.Label(new Rect (100, 350, 150, 35), "Jump:     Spacebar");
			}
			
			// Make the Quit button.
			if (GUI.Button (new Rect (Screen.currentResolution.width - 300, 600, 150, 35), "Quit") || GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Two)) {
				// quits game
				q2 = true;
			}
			
			// Resumes the game when paused
			if (GUI.Toggle (new Rect (Screen.currentResolution.width - 300, 400, 150, 35), r2,"Resume","Button")!= r2 || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Two)) {
				r2 = !r2;
			}
			
			//Toggles between displaying the keys
			if (GUI.Toggle (new Rect (Screen.currentResolution.width - 300, 500, 150, 35), help2,"Help","Button")!= help2 || GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Two)) {
				help2 = !help2;
			}
			
			if (help2){
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 100, 300, 35), "Builders Moves");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 150, 300, 35), "Add Block:\tHold Left Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 200, 300, 35), "Drop Block:\tRealease Left Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 250, 300, 35), "Rotate Block:\tRight Mouse");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 300, 300, 35), "Rotate Screen:\tMouse Wheel");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 350, 300, 35), "Move Screen Up:\tMove Mouse Up");
				GUI.Label(new Rect (Screen.currentResolution.width - 300, 400, 300, 35), "Move Screen Down:\tMove Mouse Down");
			}
			
			if(r1 && r2){
				paused = togglePause();
			}
			if (q1 || q2){
				quit = true;
			}
		}
		
		if(quit){
			paused = false;
			
			
			GUI.Box ( new Rect (0, 0, (Screen.currentResolution.width/2), (Screen.currentResolution.height)), "Pause Menu Player One");
			GUI.Box ( new Rect ((Screen.currentResolution.width/2), 0, (Screen.currentResolution.width/2), (Screen.currentResolution.height)), "Pause Menu Player Two");
			if (GUI.Toggle (new Rect (330, 620, 150, 35), p1,"Yes","Button")!= p1) {
				p1 = !p1;	
			}
			if (GUI.Toggle (new Rect (330, 800, 150, 35), t1,"No","Button")!= t1) {
				t1 = !t1;	
			}
			if (GUI.Toggle (new Rect (Screen.currentResolution.width - 300, 400, 150, 35), p2,"Yes","Button")!= p2) {
				p2 = !p2;	
			}
			if (GUI.Toggle (new Rect (Screen.currentResolution.width - 300, 500, 150, 35), t2,"No","Button")!= t2) {
				t2 = !t2;
			}
			
			if (p1 && p2){
				QuitGame();
			}
			
			if (t1 && t2 || t1 && p2 || p1 && t2){
				p1 = false;
				p2= false;
				t1 = false;
				t2 = false;
				q1 = false;
				q2 = false;
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
			r1 = false;
			r2 = false;
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
