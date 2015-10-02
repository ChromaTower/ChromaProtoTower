using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public Text height;
	public Text shadowheight;
	public Text reset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: check if still exists
		float pos = Mathf.Round (((GameManager.instance.getPlayer().transform.position.y + 0.7f) )* 5)/5;
		height.text = (pos / 2) + "m";	

		//TODO: Kill magic values
		float pos2 = Mathf.Round(((GameManager.instance.getShadow().transform.position.y + (GameManager.instance.getShadow().transform.localScale.y / 2) + 0.7f))* 5)/5;
		shadowheight.text = (pos2 / 2) + "m";

		if (GameManager.instance.getPlayer().GetComponent<PlayerMove>().alive == false)
		{
			reset.color = new Color(1f, 1f, 1f, 1f);
		} else 
		{
			reset.color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
