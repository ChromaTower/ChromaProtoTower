using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

	// The amount of energy this pickup contains
	// Higher = more blocks can be used from it
	public int energyAmount = 15;
	public AudioClip pickupSound;
	private bool active = true;
	public GameObject ps;
	public GameObject ps2;
	public GameObject ps3;
	public GameObject ps4;

	// Use this for initialization
	void Start () {
		ps2 = (GameObject)Object.Instantiate(ps2, transform.position, Quaternion.identity);
		ps2.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer>().material.color;
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

				GameObject explosion = (GameObject)Object.Instantiate(ps, transform.position, Quaternion.identity);
				GameObject explosion2 = (GameObject)Object.Instantiate(ps3, transform.position, Quaternion.identity);
				GameObject aurora = (GameObject)Object.Instantiate(ps4, transform.position, Quaternion.identity);

				explosion.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer>().material.color;
				explosion2.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer>().material.color;
				aurora.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer>().material.color;

				explosion.GetComponent<ParticleSystem>().Play ();
				Destroy (ps2);
				explosion2.GetComponent<ParticleSystem>().Play ();
				aurora.GetComponent<ParticleSystem>().Play ();

				GetComponent<Light>().color = Color.clear;

				active = false;

				GetComponent<Renderer>().enabled = false;
				Destroy(gameObject, 5);

				// Make the goo rise quicker!

				GameManager.instance.getShadow().GetComponent<ShadowManager>().rise();
				GameManager.instance.getMusicHandler().GetComponent<MusicHandler>().incrementPitch();
				GameManager.instance.getColorEngine().GetComponent<ColorManager>().introduceColor();
			}
		}
	}
}
