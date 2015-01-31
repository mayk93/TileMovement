using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClickHandler : MonoBehaviour {

	public int tileX;
	public int tileY;

	public TileMap map;

	/* Part of Unit old move code */
	/*
	public GameObject unit;
	*/

	// Use this for initialization
	void Start () 
	{
		/* Part of Unit old move code */
		/*
		unit = GameObject.FindGameObjectWithTag ("Player");
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnMouseUp()
	{
		/* Part of Unit old move code */
		/*
		unit.GetComponent<Unit>().currentPath = null;
		*/

		map.GeneratePathTo (tileX, tileY);
	}
}
