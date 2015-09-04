using UnityEngine;
using System.Collections;

public class DirLight : MonoBehaviour {
	public GameObject track;
	
	// Update is called once per frame
	void Update () {
		// Point at the tracked object
		transform.LookAt (track.transform.position);
	}
}
