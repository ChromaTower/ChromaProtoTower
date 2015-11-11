using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu1 : MonoBehaviour {

	private PauseMenu1 menu;
	public Canvas quitMenu;

	public Button resume;
	public Button quit;
	public Button help;
	public Text t;
	public Text t1;
	public Text t2;

	// Use this for initialization
	void Start () {
		menu = GetComponent<PauseMenu1>();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		resume = resume.GetComponent<Button>();
		quit = quit.GetComponent<Button>();
		help = help.GetComponent<Button>();
		t = t.GetComponent<Text>();
		t1 = t1.GetComponent<Text>();
		t2 = t2.GetComponent<Text>();
		quitMenu.enabled = false;
		t.enabled = false;
		t1.enabled = false;
		t2.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HelpButton(){
		t.enabled = true;
		t1.enabled = true;
		t2.enabled = true;
	}
}
