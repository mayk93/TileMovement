using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	public TileType[] tileTypes;
	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;

	// Use this for initialization
	void Start () {
		tiles = new int[mapSizeX,mapSizeY];

		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				tiles[x,y] = 0;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
