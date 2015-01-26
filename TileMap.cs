using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	public TileType[] tileTypes;
	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;

	// Use this for initialization
	void Start () 
	{
		tiles = new int[mapSizeX,mapSizeY];

		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				tiles[x,y] = 0;
			}
		}

		tiles [0, 3] = 2;
		tiles [0, 2] = 2;
		tiles [0, 1] = 2;
		tiles [0, 0] = 2;

		tiles [1, 0] = 2;
		tiles [2, 0] = 2;

		tiles [3, 3] = 2;
		tiles [3, 2] = 2;
		tiles [3, 1] = 2;
		tiles [3, 0] = 2;

		GenerateMapVisuals ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GenerateMapVisuals()
	{
		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				Instantiate( tileTypes[tiles[x,y]].tilleVisualPrefab , new Vector3(x,y,0), Quaternion.identity);
			}
		}
	}
}
