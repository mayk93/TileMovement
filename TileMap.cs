using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	Node[,] graph;

	// Use this for initialization
	void Start () 
	{
		GenerateMapData ();
		GenerateGraph ();
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

		for( int x = 1; x < mapSizeX; x++ )
		{
			for( int y = 1; y < mapSizeY; y++ )
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

	void GenerateGraph()
	{
		graph = new Node[mapSizeX, mapSizeY];

		for (int x = 0; x < mapSizeX; x++) 
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				graph[x,y].x = x;
				graph[x,y].y = y;

				/* To Add Diagonals */
				if( x > 0 )
				{
					graph[x,y].neighbours.Add( graph[x-1,y] );
				}
				if( x < mapSizeX-1 )
				{
					graph[x,y].neighbours.Add( graph[x+1,y] );
				}

				if( y > 0 )
				{
					graph[x,y].neighbours.Add( graph[x,y-1] );
				}
				if( y < mapSizeY-1 )
				{
					graph[x,y].neighbours.Add( graph[x,y+1] );
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
		/*
		// Unit Data
		unit.GetComponent<Unit> ().tileX = x;
		unit.GetComponent<Unit> ().tileY = y;
		// Visual Movement
		unit.transform.position = TileCoordinatesToWorldCoordinates(x,y);
		*/

		/* Implementing Dijkstras Algorithm */
		Dictionary<Node,float> distance = new Dictionary<Node,float>();
		Dictionary<Node,Node> previous = new Dictionary<Node,Node>();

		List<Node> unvisitedNodes = new List<Node>();

		Node source = graph[unit.GetComponent<Unit>().tileX,unit.GetComponent<Unit>().tileY];
		Node target = graph[x,y];

		distance [source] = 0;
		previous [source] = null;

		/* Initialize with infinity if not soource ( as in we don't know the distance ) */
		foreach (Node graphNode in graph) 
		{
			if(graphNode != source)
			{
				distance[graphNode] = Mathf.Infinity;
				previous[graphNode] = null;
			}

			unvisitedNodes.Add(graphNode);
		}

		while (unvisitedNodes.Count > 0) 
		{
			Node currentlyVisiting = unvisitedNodes.OrderBy( nodeDistance => distance[nodeDistance] ).First();
			unvisitedNodes.Remove(currentlyVisiting);

			foreach(Node neighbour in currentlyVisiting.neighbours)
			{
				float alt = distance[currentlyVisiting] + currentlyVisiting.DistanceTo(neighbour);
				if ( alt < distance[neighbour] )
				{
					distance[neighbour] = alt;
					previous[neighbour] = currentlyVisiting;
				}
			}
		}
	}
}

public class Node
{
	public List<Node> neighbours; /* The edges */
	public int x;
	public int y;

	public Node()
	{
		neighbours = new List<Node>();
	}

	public float DistanceTo(Node otherNode)
	{
		return Vector2.Distance ( new Vector2(x,y) , new Vector2(otherNode.x,otherNode.y) );
	}
}
