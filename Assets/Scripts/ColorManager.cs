using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {

	private List<GameObject> colorEnts = new List<GameObject>();

	private List<List<GameObject>> hueEnts = new List<List<GameObject>>();

	public float hue = 0f;
	private int hueNum = 0;
	private bool cycleComplete = false;

	private List<float> hues = new List<float>();


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



	public void introduceColor()
	{
		if (cycleComplete == false)
		{
			foreach(GameObject ent in hueEnts[hueNum])
			{
				ent.GetComponent<Renderer>().material.color = colorFromHSV(Mathf.Abs (hue), 1f, 0.6f);
			}
		}

		hueNum += 1;

		if (hueNum + 1 > hues.Count)
		{
			hueNum = 0;
			cycleComplete = true;
		}

		GameManager.instance.getBlobbi().GetComponent<BlobbiManager>().addColor(colorFromHSV(Mathf.Abs (hue), 1f, 0.5f));
		//Debug.LogWarning(hue);

		hue = hues[hueNum];

		GameManager.instance.getTower ().generatePickup(3, colorFromHSV(Mathf.Abs (hue), 1f, 0.6f) ); // previously 4
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 7; i++)
		{
			hueEnts.Add(new List<GameObject>());
		}


		addEnts();
		sortEnts();

		// A D D  T H E  RAINBOW
		hues.Add (0f); 			// R
		hues.Add (120f / 360f);	// G
		hues.Add (320f / 360f);	// V
		hues.Add (30f / 360f);	// O
		hues.Add (280f / 360f);	// I
		hues.Add (60f / 360f);	// Y
		hues.Add (240f / 360f);	// B




		//hues.Sort();
	}

	void addEnts()
	{
		foreach(GameObject cl in GameManager.instance.getCloudManager().GetComponent<CloudHandler>().getCloudList())
		{
			colorEnts.Add(cl);
		}

		GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");

		foreach(GameObject t in terrains)
		{
			colorEnts.Add(t);
		}


	}




	void sortEnts()
	{
		int i = 0;

		foreach(GameObject ent in colorEnts)
		{
			hueEnts[i].Add(ent);

			i += 1;

			if (i > 6)
			{
				i = 0;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
