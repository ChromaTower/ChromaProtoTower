using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class StartUp : MonoBehaviour {

	private string path;
	private string file;
	private List<float> scores; 
	// Use this for initialization
	void Start () {
		path = "C:/Users/Public/Documents/ChromaTower";
		file = "C:/Users/Public/Documents/ChromaTower/HighScores.txt";
		scores = new List<float>();
		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			if(!File.Exists(file)){
				File.Create(file);
			}
		}
		catch (IOException ex)
		{
			Debug.Log(ex.Message);
		}

		if(File.Exists(file) && scores.Count == 0){
			SetUpHighScore();
		}
		ReadScores ();
		Application.LoadLevel (1);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetUpHighScore(){
		using(StreamWriter sw = new StreamWriter(file))
		{	
			try{
			sw.WriteLine("5");
			sw.WriteLine("4");
			sw.WriteLine("3");
			sw.WriteLine("2");
			sw.WriteLine("1");
			}
			catch (IOException ex)
			{
				Debug.Log(ex.Message);
			}

		}
	}

	public void ReadScores(){
		using(StreamReader sr = new StreamReader(file))
		{
			int i = 0;
			string line;
			while ( !sr.EndOfStream)
			{
				line = sr.ReadLine();
				scores.Add(float.Parse(line));
				Debug.Log(scores[i]);
				i++;
			}
		}
	}

}
