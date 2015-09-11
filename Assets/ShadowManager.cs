using UnityEngine;
using System.Collections;

public class ShadowManager : MonoBehaviour {

	private float startY;
	private Vector3 startScale;
	// The rate the goo rises, in units per second
	public float riseRate = 0.5f;

	// Use this for initialization
	void Start () {
		//GameObject o = GameObject.Find("Floor");
		//transform.localScale = new Vector3(o.transform.lossyScale.x, 1f, o.transform.lossyScale.z);
		startY = transform.position.y;
		startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		// Slowly rise
		transform.localScale += new Vector3(0f, riseRate * Time.deltaTime, 0f);
		transform.position = new Vector3(transform.position.x, startY + (transform.localScale.y/2), transform.position.z);
	}

	public void reset()
	{
		transform.localScale = startScale;
		transform.position = new Vector3(transform.position.x, startY, transform.position.z);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player") {
			collider.gameObject.GetComponent<PlayerMove>().death();
		}
		if (collider.gameObject.tag == "Block") {
			collider.GetComponent<Renderer>().material.color = new Color (0f, 0f, 0f);
		}
	}
}
