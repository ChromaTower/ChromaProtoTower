using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float size = Vector3.Distance (transform.position, GameManager.instance.getPlayerCamera().transform.position);
		GetComponent<Renderer>().material.mainTextureScale = new Vector2 (30f / size, 30f / size);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
