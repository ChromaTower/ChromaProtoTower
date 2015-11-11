using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BlobbiManager : MonoBehaviour {
	JellyMesh m_JellyMesh;
	bool col = false;
	private Renderer re;
	private float hue, hueSpeed, sat;
	public bool alive = true;
	//private float hueMax,  hueMin = 0;

	private List<Color> colList = new List<Color>();
	private int colListTotal = 0;
	private Color colNext;
	private Color colPrev;
	private int colIndex = 0;
	private float colSpeed = 0;
	private float colLevel = 0;

	//private int hueSwitch = 1;

	// Use this for initialization
	void Start () {
		m_JellyMesh = GetComponent<JellyMesh>();
		re = GetComponent<Renderer>();
		colNext = Color.grey;
		colPrev = Color.grey;
	}

	void setColor(Color col)
	{
		re.material.color = col;
	}

	// Update is called once per frame
	void Update() {
		if (alive)
		{
			// Allows for smooth colour changing
			float vel = transform.parent.GetComponent<Rigidbody>().velocity.magnitude;

			colSpeed *= 0.985f;
			colSpeed += (float)((vel * 2) * 0.0005f) * Time.deltaTime;
			colLevel += colSpeed;

			float r = ((1 - colLevel) * colPrev.r) + (colLevel * colNext.r);
			float g = ((1 - colLevel) * colPrev.g) + (colLevel * colNext.g);
			float b = ((1 - colLevel) * colPrev.b) + (colLevel * colNext.b);

			setColor (new Color(r, g, b));

			//Debug.LogWarning (r + ", " + g + ", " + b);

			if (colLevel >= 1)
			{
				colLevel -= 1;

				colPrev = colNext;

				if (colIndex + 1 > colList.Count)
				{
					if (colList.Count > 0)
					{
						colNext = colList[0];
						colIndex = 0;
					}
				} else {
					colNext = colList[colIndex];
					colIndex += 1;
				}

			}


		/*	// primitive friction
			hueSpeed *= 0.985f;

			hue += hueSpeed * Time.deltaTime * hueSwitch;

			// Value rollover
			if (hue > hueMax)
			{
					hueSwitch = -1;
					hue = hueMax;
			} else if (hue < hueMin)
			{
					hueSwitch = 1;
					hue = hueMin;
			}

			hueSpeed += (float)(vel * ((hueMax - hueMin)) * 0.5f) * Time.deltaTime;


			if (hue < 0f)
			{
				re.material.color = colorFromHSV(hue + 1f, sat, 0.5f);
			} else if (hue > 1f){
				re.material.color = colorFromHSV(hue - 1f, sat, 0.5f);
			} else {
				re.material.color = colorFromHSV(hue, sat, 0.5f);
			}*/

			//print (re.material.color);
		}
	}

	public void addColor(Color col)
	{
		colList.Add (col);
	}

	// Set the maximum hue and saturation
	/*public void setHS(float h, float s)
	{
		if (h > 0.5f)
		{
			hueMin = h - 1;
		} else {
			hueMax = h;
		}


		sat = s;
	}*/


	// Gets the colour of the blob
	public Color getColor()
	{
		return colorFromHSV(Mathf.Abs (hue), Mathf.Abs(sat), 0.5f);
	}

	// Converts a color from HSV values
	Color colorFromHSV(float h, float s, float v)
	{
		// Algorithm adapted from http://cs.rit.edu/~ncs/color/t_convert.html
		int i;
		float f, p, q, t;
		float r, g, b;

		if (s == 0f)
		{
			return new Color(v, v, v);
		}

		h /= (60f/360f);
		i = (int)(Mathf.Floor (h));

		f = h - i;
		p = v * (1 - s);
		q = v * (1 - s * f);
		t = v * (1 - s * (1 - f));

		switch(i)
		{
			case 0:
				r = v;
				g = t;
				b = p;
				break;
			case 1:
				r = q;
				g = v;
				b = p;
				break;
			case 2:
				r = p;
				g = v;
				b = t;
				break;
			case 3:
				r = p;
				g = q;
				b = v;
				break;
			case 4:
				r = t;
				g = p;
				b = v;
				break;
			default:		// case 5:
				r = v;
				g = p;
				b = q;
				break;
		}
		return new Color(r, g, b);

	}

	void FixedUpdate () {
		if (col == false)
		{
			m_JellyMesh.ignoreCollision(transform.parent.GetComponent<Rigidbody>().GetComponent<Collider>(), true);
			m_JellyMesh.ignoreCollision(GameManager.instance.getPlayer().GetComponent<Rigidbody>().GetComponent<Collider>(), true);
			col = true;
		}

		m_JellyMesh.SetVelocity(transform.parent.GetComponent<Rigidbody>().velocity);
		m_JellyMesh.SetPosition(transform.parent.position, false);
		//transform.position = transform.parent.position;

		//transform.position = transform.parent.position;

		if (GameManager.instance.getPlayerCamera ().GetComponent<CameraFollow>().closeUp)
		{
			transform.parent.rotation = GameManager.instance.getPlayerCamera().transform.rotation;
			m_JellyMesh.SetPosition(transform.parent.position, false);
			//m_JellyMesh.SetVelocity(transform.parent.GetComponent<Rigidbody>().velocity);
		}

	}

}
