using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	private Rigidbody rb;
	public Renderer re;

	float snap;

	private bool falling = false;
	private bool activated = false;

	// target positions
	public Vector3 target = Vector3.zero;

	private TowerManager tower;

	// Use this for initialization
	void Start () {
		tower = GameManager.instance.getTower();
		snap = tower.getSnap ();
		re = GetComponent<Renderer>();
		re.enabled = true;

		rb = GetComponent<Rigidbody>();

		// Set to a random size... either 1x1x1, 2x1x1, 1x1x2, or 2x1x2
		transform.localScale = new Vector3(snap, snap, snap); // * new Vector3(Mathf.Round(transform.localScale.x * Mathf.Round (Random.Range(1f, 2f))), transform.localScale.y, transform.localScale.z * Mathf.Round(Random.Range(1f, 2f)));

		if (falling == false && activated == false)
		{
			Snooze();
		}
	}

	// Returns if the block is activated (jumpable)
	public bool isActivated()
	{
		return activated;
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
		rb.detectCollisions = false;
		setAlpha(0.5f);

		falling = false;
	}
	
	// Allows the object to move and deactivates transparency
	public void Drop()
	{
		// Only drop the block if it's in a valid position
		if (tower.registerPos(target))
		{
			rb.WakeUp();
			rb.isKinematic = false;

			setAlpha(1f);

			// Restore position 
			transform.position = new Vector3(transform.position.x, tower.getPos().y, transform.position.z);
			falling = true;
		} else {
			tower.removeBlock (gameObject);
		}

	}

	public void activate()
	{
		rb.detectCollisions = true;
		activated = true;
		rb.isKinematic = true;
	
	}

	// Looks up the block list to find the position the block will fall to
	float checkYPosition(Vector3 startPos)
	{
		// Returns the lowest y-snap
		float result = startPos.y;

		// Start from the block's position and check downward
		for (int yCheck = (int)result; yCheck >= 0; yCheck--)
		{
			if (tower.checkPos(new Vector3(startPos.x, yCheck, startPos.z)) == true)
			{
				break;
			} else {
				result = yCheck;
			}
		}
		return result;
	}

	// Update is called once per frame
	void Update () {
		if (activated == false)
		{
			if (falling == false)
			{
				// Used to look up the tower blocks to see what blocks are located where
				float xSnap = (int)(transform.position.x / snap);
				float ySnap = (int)(tower.getPos().y / snap);
				float zSnap = (int)(transform.position.z / snap);
				target = new Vector3(xSnap, ySnap, zSnap);
	
				target.y = checkYPosition(target);
	
				transform.position = target * snap;
			} else {
				// Check if the block has fallen past its designated point
				if (transform.position.y < target.y * snap)
				{
					transform.position = target * snap;
	
					// Set velocity/angles/etc to zero just in case it got screwed at some point
					rb.velocity = Vector3.zero;
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);

					// The block is now stuck and jumpable
					activate();
				}
			}
		}
	}
}
