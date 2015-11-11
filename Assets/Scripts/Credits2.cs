using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits2 : MonoBehaviour {
	public Text t;
	public Text t1;
	public Text t2;
	public Text t3;
	public Text t4;
	public Text t5;
	private GameObject g1;
	private GameObject g2;
	private GameObject g3;
	private GameObject g4;
	private GameObject g5;
	private GameObject g6;
	private Vector3 test;
	private Vector3 test2;

	
	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		g1 = GameObject.Find("T6");
		g2 = GameObject.Find("T7");
		g3 = GameObject.Find("T8");
		g4 = GameObject.Find("T9");
		g5 = GameObject.Find("T10");
		g6 = GameObject.Find("Text2");
		t1= g1.GetComponent<Text>();
		t2= g2.GetComponent<Text>();
		t3= g3.GetComponent<Text>();
		t4= g4.GetComponent<Text>();
		t5= g5.GetComponent<Text>();
		t= g6.GetComponent<Text>();

		test = new Vector3 (1000, -316.5f, 0);
		test2 = new Vector3 (5000, 0, 0);
		test2 = new Vector3 (5000, 0,0);
		LeanTween.init (1000);
		StartCoroutine(Wait (1));
		StartCoroutine(Wait1 (5));
		StartCoroutine(Wait2 (10));
		StartCoroutine(Wait3 (20));
		StartCoroutine(Wait4 (30));
		StartCoroutine(Wait5 (40));
		StartCoroutine(Quit (52));
	}
	
	// Update is called once per frame
	void Update () {
	}
	IEnumerator Wait1(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		LeanTween.moveLocal( g1, g1.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g2, g2.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g6, g6.transform.position + test2, 1.5f);
		t.text = "Artists";
		t1.text = "Althea Capuno";
		t2.text = "Nicholas Todd";
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Wait2(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);
		LeanTween.moveLocal( g1, g1.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g2, g2.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g6, g6.transform.position + test, 3f).setDelay(1f);
		yield return new WaitForSeconds (duration-5f);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		LeanTween.moveLocal( g1, g1.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g2, g2.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g6, g6.transform.position + test2, 1.5f);
		t.text = "3D Modeling";
		t1.text = "Althea Capuno";
		t2.text = "Steven Efthimiadis";
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Wait3(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);
		LeanTween.moveLocal( g1, g1.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g2, g2.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g6, g6.transform.position + test, 3f).setDelay(1f);
		yield return new WaitForSeconds (duration-15f);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		LeanTween.moveLocal( g1, g1.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g2, g2.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g6, g6.transform.position + test2, 1.5f);
		t.text = "Music and Sound";
		t1.text = "David Harris";
		t2.text = "Robert Villella";
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Wait4(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);
		LeanTween.moveLocal( g1, g1.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g2, g2.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g6, g6.transform.position + test, 3f).setDelay(1f);
		yield return new WaitForSeconds (duration-25f);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		LeanTween.moveLocal( g1, g1.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g2, g2.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g6, g6.transform.position + test2, 1.5f);
		t.text = "Play Testing";
		t1.text = "Andrew Martin";
		t2.text = "David Harris";
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Wait5(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);
		LeanTween.moveLocal( g1, g1.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g2, g2.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g6, g6.transform.position + test, 3f).setDelay(1f);
		yield return new WaitForSeconds (duration-35f);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		LeanTween.moveLocal( g1, g1.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g2, g2.transform.position + test2, 1.5f);
		LeanTween.moveLocal( g6, g6.transform.position + test2, 1.5f);
		t.text = "Programming";
		t1.text = "Nicholas Todd";
		t2.text = "Steven Efthimiadis";
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Wait(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		LeanTween.moveLocal( g1, g1.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g2, g2.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g3, g3.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g4, g4.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g5, g5.transform.position + test, 3f).setDelay(1f);
		LeanTween.moveLocal( g6, g6.transform.position + test, 3f).setDelay(1f);
		yield return new WaitForSeconds (duration);   //Wait
		g6.transform.position = new Vector3 (-5000, 54,0);
		g1.transform.position = new Vector3 (-5000, -33,0);
		g2.transform.position = new Vector3 (-5000, -83,0);
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
	IEnumerator Quit(float duration)
	{
		//This is a coroutine
		Debug.Log ("Start Wait() function. The time is: " + Time.time);
		Debug.Log ("Float duration = " + duration);
		yield return new WaitForSeconds (duration);   //Wait
		Application.Quit ();
		Debug.Log ("End Wait() function and the time is: " + Time.time);
	}
	
}
