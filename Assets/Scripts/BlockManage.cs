using UnityEngine;
using System.Collections;

public class BlockManage : MonoBehaviour {

	private Rigidbody rb;
	public Renderer re;

	// Divisions for snapping
	private int snap;

	public bool active = false;

	Vector3 prevPos = Vector3.zero;
	float prevY = 0;

	// Use this for initialization
	void Start () {
		re = GetComponent<Renderer>();
		re.enabled = true;

		rb = GetComponent<Rigidbody>();

		// Set to a random size... either 1x1x1, 2x1x1, 1x1x2, or 2x1x2
		transform.localScale = new Vector3(Mathf.Round(transform.localScale.x * Mathf.Round (Random.Range(1f, 2f))), transform.localScale.y, transform.localScale.z * Mathf.Round(Random.Range(1f, 2f)));
		snap = (int)transform.localScale.y;

		if (active == false)
		{
			Snooze();
		}
	}

	// Set the transparency of the object
	public void setAlpha(float alpha)
	{
		if (alpha < 0f)
		{
			alpha = 0f;
		}
		if (alpha > 1f)
		{
			alpha = 1f;
		}

		re.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
	}

	// Makes the object motionless and transparent
	public void Snooze()
	{
		rb.isKinematic = true;
		setAlpha(0.5f);

		// Save prior position, will use for later
		if (prevY == 0)
		{
			prevY = transform.position.y;
		}

		active = false;
	}

	// Allows the object to move and deactivates transparency
	public void Awaken()
	{
		rb.WakeUp();
		rb.isKinematic = false;
		Color prevColor = GetComponent<Renderer>().material.color;
		setAlpha(1f);

		// Restore position 
		transform.position = new Vector3(transform.position.x, prevY, transform.position.z);
		prevY = 0;

		active = true;
	}

	// Freeze/snap the block once it reaches its destination
	void OnCollisionEnter(Collision collision) {
		// Collide if active/falling
		if (rb.isKinematic == false)
		{
			if (collision.gameObject.tag == "FreezeOnTouch")
			{
				// Snap the block to the set position
				float snapX = Mathf.Round(transform.position.x / snap) * snap;
				//float snapY = Mathf.Round(transform.position.y / snap) * snap;
				float snapZ = Mathf.Round(transform.position.z / snap) * snap;
				transform.position = new Vector3(snapX, transform.position.y, snapZ);
	
				rb.isKinematic = true;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (active == false)
		{
			// Snap the block to the set position
			float snapX = Mathf.Round(transform.position.x / snap) * snap;
			float snapY = Mathf.Round(transform.position.y / snap) * snap;
			float snapZ = Mathf.Round(transform.position.z / snap) * snap;
			transform.position = new Vector3(snapX, transform.position.y, snapZ);
		}
	}
}
