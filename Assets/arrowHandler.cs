using UnityEngine;
using System.Collections;

public class arrowHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position += new Vector3(0f, 1f, 0f);
		GetComponent<Renderer>().material = GameManager.instance.getBlobbi().GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion ang = Quaternion.Euler(new Vector3(-90f, 180f, 0f));
		transform.rotation = GameManager.instance.getPlayerCamera ().transform.rotation * ang;
	}
}
