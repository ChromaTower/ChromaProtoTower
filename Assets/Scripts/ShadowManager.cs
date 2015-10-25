using UnityEngine;
using System.Collections;

public class ShadowManager : MonoBehaviour {

	private float startY;
	private Vector3 startScale;
	// The rate the goo rises, in units per second
	public float riseRate = 0.5f;

	private Material mat;

	// Use this for initialization
	void Start () {
		//GameObject o = GameObject.Find("Floor");
		//transform.localScale = new Vector3(o.transform.lossyScale.x, 1f, o.transform.lossyScale.z);
		startY = transform.position.y;
		startScale = transform.localScale;

		mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		// Slowly rise
		transform.position += new Vector3(0f, riseRate * Time.deltaTime, 0f);
		mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + 0.00001f, mat.mainTextureOffset.y + 0.00001f);
	}

	public void reset()
	{
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
