using UnityEngine;
using System.Collections;

public class inkBlobParticleManager : MonoBehaviour {

	private float grav = 0f;
	private Vector3 initialPos;

	// Use this for initialization
	void Awake () {
		initialPos = transform.position;
		grav = Mathf.Abs (GetComponent<ParticleSystem>().gravityModifier * Physics.gravity.y * 1.1f);
		//print(grav);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<ParticleSystem>().emissionRate = GameManager.instance.getShadow().GetComponent<ShadowManager>().riseRate * 4;
		GetComponent<ParticleSystem>().startLifetime = Mathf.Sqrt(2 * Vector3.Distance (initialPos,
		                                                                                 new Vector3(initialPos.x, 
		            																				GameManager.instance.getShadow ().transform.position.y,
		            																				initialPos.z)) / grav) + 0.4f;
		//print (GetComponent<ParticleSystem>().startLifetime);
		GetComponentInChildren<ParticleSystem>().startSpeed = GameManager.instance.getShadow().GetComponent<ShadowManager>().riseRate * -200;
	}
}
