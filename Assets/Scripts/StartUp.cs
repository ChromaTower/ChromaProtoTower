using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class StartUp : MonoBehaviour {
	
	private string path;
	private string file;
	private string file2;
	private string file3;
	private List<float> scores; 
	// Use this for initialization
	void Start () {
		path = "ChromaTower_Data";
		file = "ChromaTower_Data/HighScores.txt";
		scores = new List<float>();
		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			if(!File.Exists(file)){
				CreateFile();
			}
		}
		catch (IOException ex)
		{
			Debug.Log(ex.Message);
		}
		
		if(File.Exists(file) && ReadScores() == false){
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
				sw.Close();
			}
			catch (IOException ex)
			{
				Debug.Log(ex.Message);
			}
			
		}
	}
	
	public bool ReadScores(){
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
			
			if(i == 5){
				return true;
			}
			
			return false;
		}
	}
	
	public void CreateFile(){
		using (FileStream fs =  File.Create(file))
		{
			Debug.Log("Working");
		}

	}
	
}
