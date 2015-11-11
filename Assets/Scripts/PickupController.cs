using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

	// The amount of energy this pickup contains
	// Higher = more blocks can be used from it
	public int energyAmount = 15;
	public AudioClip pickupSound;
	private bool active = true;
	public ParticleSystem ps;
	public ParticleSystem ps2;

	// Use this for initialization
	void Start () {
		ps.startColor = GetComponent<Renderer>().material.color;
		ps2.startColor = GetComponent<Renderer>().material.color;
		GetComponent<Light>().color = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		// When a player touches it
		if (other.gameObject.CompareTag("Player"))
		{
			if (active)
			{
				GameManager.instance.getTower().blockEnergy += energyAmount;


				GetComponent<AudioSource>().volume = 1;
				GetComponent<AudioSource>().clip = pickupSound;
				GetComponent<AudioSource>().Play();

				ps.Play ();

				active = false;

				GetComponent<Renderer>().enabled = false;
				Destroy(gameObject, pickupSound.length);

				// Make the goo rise quicker!
				GameManager.instance.getShadow().GetComponent<ShadowManager>().riseRate += energyAmount/80f;
				//GameManager.instance.getColorEngine().GetComponent<ColorManager>().introduceColor();
			}
		}
	}
}
