using UnityEngine;
using System.Collections;

public class ClickHandler : MonoBehaviour {

	public int tileX;
	public int tileY;

	public TileMap map;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp()
	{
		/*
		Debug.Log (tileX);
		Debug.Log (tileY);
		*/

		map.GeneratePathTo (tileX, tileY);
	}
}
