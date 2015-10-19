using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

	private float heightPitch = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		heightPitch = Mathf.Sqrt((GameManager.instance.getPlayer().transform.position.y + 1f)/20f);
		print (heightPitch);
		GetComponent<AudioSource>().pitch = Mathf.Min (Mathf.Max (0.01f, heightPitch), 1f);
	}
}
