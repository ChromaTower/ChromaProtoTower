using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class UIManager : MonoBehaviour {
	private GameManager manager;
	public Text height;
	public Text shadowheight;
	public Text heightTower;
	public Text blocksLeft;

	public RawImage bckLeft1;
	public RawImage bckLeft2;
	public RawImage bckLeft3;
	public RawImage bckLeft4;
	public RawImage bckLeft5;
	public RawImage bckLeft6;
	public RawImage bckLeft7;
	public RawImage bckLeft8;
	public RawImage bckLeft9;
	public RawImage bckLeft10;

	public Text reset;
	public Text score;
	private string file;
	public int leftoverBlocks;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		file = "C:/Users/Public/Documents/ChromaTower/HighScores.txt";
		//leftoverBlocks = GameManager.instance.getTower ().blockEnergy;
		leftoverBlocks = 9;
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: check if still exists
		float pos = Mathf.Round (((manager.getPlayer().transform.position.y + 0.7f) )* 5)/5;
		height.text = (pos / 2) + "m";	
		heightTower.text = height.text;

		//TODO: Kill magic values
		float pos2 = Mathf.Round(((GameManager.instance.getShadow().transform.position.y + (GameManager.instance.getShadow().transform.localScale.y / 2) + 0.7f))* 5)/5;
		shadowheight.text = (pos2 / 2) + "m";

		blocksLeft.text = leftoverBlocks + " available";	


		ReadScores ((pos / 2));

		if (manager.getPlayer().GetComponent<PlayerMove>().alive == false)
		{
			reset.color = new Color(1f, 1f, 1f, 1f);
		} else 
		{
			reset.color = new Color(1f, 1f, 1f, 0f);
		}

		if(leftoverBlocks == 9){
			bckLeft1.enabled = false;
			bckLeft2.enabled = false;
			bckLeft3.enabled = false;
		}
	}


	public void ReadScores(float curH){
		string line;
		using(StreamReader sr = new StreamReader(file))
		{
			line = sr.ReadLine();
			score.text = line;
		}

		if(curH > float.Parse(line)){
			score.text = curH + "m";
		}

	}
}
