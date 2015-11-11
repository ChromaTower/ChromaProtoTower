using UnityEngine;
using System.Collections;

public class ShadowManager : MonoBehaviour {

	private float startY;
	private Vector3 startScale;
	// The rate the goo rises, in units per second
	public float riseRate = 0.5f;
	private int rises = 0;
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

		GameObject player = GameManager.instance.getPlayer();

		if (transform.position.y > player.transform.position.y) {
			player.GetComponent<PlayerMove>().death();
		}
	}

	public void reset()
	{
		transform.position = new Vector3(transform.position.x, startY, transform.position.z);
	}


	public void rise()
	{
		rises += 1;

		if (rises >= 7)
		{
			riseRate += 0.1f;
		} else {
			if (rises <= 3)
			{
				riseRate += 0.125f;
			} else {
				riseRate += 0.125f - ((rises - 3) * 0.025f);
			}
		}
	}
}
