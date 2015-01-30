using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	/*
		Grassland - 0
		Swamp - 1
		Mountain - 2
	*/

	public GameObject unit;

	public TileType[] tileTypes;
	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;

	// Use this for initialization
	void Start () 
	{
		GenerateMapData ();
		GenerateMapVisuals ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GenerateMapData()
	{
		tiles = new int[mapSizeX,mapSizeY];

		/* Default to grassland */
		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				tiles[x,y] = 0;
			}
		}

		/* Old, hardcoded generation for test */
		/*
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
		*/

		for( int x = 0; x < mapSizeX; x++ )
		{
			for( int y = 0; y < mapSizeY; y++ )
			{
				/* Change half the tiles */
				/* Used 2 because Random.Range has exclusive max */
				if( Random.Range(0,2) == 0 )
				{
					tiles[x,y] = Random.Range(1,3);
				}
			}
		}
	}

	void GenerateMapVisuals()
	{
		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				GameObject instantiatedTile = (GameObject)Instantiate( tileTypes[tiles[x,y]].tilleVisualPrefab , new Vector3(x,y,0), Quaternion.identity);
				ClickHandler instantiatedTileClickHandler = instantiatedTile.GetComponent<ClickHandler>();
				instantiatedTileClickHandler.tileX = x;
				instantiatedTileClickHandler.tileY = y;
				instantiatedTileClickHandler.map = this;
			}
		}
	}

	public Vector3 TileCoordinatesToWorldCoordinates(int x, int y)
	{
		return new Vector3 (x, y, 0);
	}

	public void MoveUnitTo(int x, int y)
	{
		/* Unit Data */
		unit.GetComponent<Unit> ().tileX = x;
		unit.GetComponent<Unit> ().tileY = y;
		/* Visual Movement */
		unit.transform.position = TileCoordinatesToWorldCoordinates(x,y);
	}
}
