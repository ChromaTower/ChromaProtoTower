using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

	private float heightPitch = 0.4456f;

	public void incrementPitch()
	{
		if (heightPitch < 1)
		{
			heightPitch *= 1.1225f;
			GetComponent<AudioSource>().pitch = Mathf.Min (Mathf.Max (0.01f, heightPitch), 1f);
		}
	}

	public void stopPitch()
	{
		heightPitch = 0.1f;
		GetComponent<AudioSource>().pitch = Mathf.Min (Mathf.Max (0.01f, heightPitch), 1f);
	}

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().pitch = Mathf.Min (Mathf.Max (0.01f, heightPitch), 1f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
