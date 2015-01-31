﻿using UnityEngine;
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

	List<Node> currentPath = null;

	// Use this for initialization
	void Start () 
	{
		unit.GetComponent<Unit> ().tileX = (int)unit.transform.position.x;
		unit.GetComponent<Unit> ().tileY = (int)unit.transform.position.y;
		unit.GetComponent<Unit> ().map = this;

		GenerateMapData ();
		GenerateGraph ();
		GenerateMapVisuals ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public float CostToEnterTile(Node toEnter)
	{
		TileType currentTileType = tileTypes[ tiles[toEnter.x,toEnter.y] ];

		if(TileCanBeEntered(toEnter.x,toEnter.y) == false)
		{
			return Mathf.Infinity;
		}

		return currentTileType.movementCost;
	}

	void GenerateMapData()
	{
		tiles = new int[mapSizeX,mapSizeY];

		// New Map Generation
		for(int x = 0; x < mapSizeX; x++)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				// If at start, make grassland, so we don't start in a mountain
				if(x == 0 && y == 0)
				{
					tiles[x,y] = 0;
				}
				else
				{
					tiles[x,y] = Random.Range(0,3);
				}
			}
		}


		/*
		// Default to grassland
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
				// Change half the tiles
				// Used 2 because Random.Range has exclusive max
				if( Random.Range(0,2) == 0 )
				{
					tiles[x,y] = Random.Range(1,3);
				}
			}
		}
		*/
	}

	void GenerateGraph()
	{
		graph = new Node[mapSizeX, mapSizeY];

		for (int x = 0; x < mapSizeX; x++) 
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		for (int x = 0; x < mapSizeX; x++) 
		{
			for (int y = 0; y < mapSizeY; y++)
			{
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

				/* Diagonals */

				if( x > 0 && y > 0 )
				{
					graph[x,y].neighbours.Add( graph[x-1,y-1] );
				}
				if( x < mapSizeX-1 && y < mapSizeX-1 )
				{
					graph[x,y].neighbours.Add( graph[x+1,y+1] );
				}
				
				if( y > 0 && x < mapSizeX-1 )
				{
					graph[x,y].neighbours.Add( graph[x+1,y-1] );
				}
				if( y < mapSizeY-1 && x > 0 )
				{
					graph[x,y].neighbours.Add( graph[x-1,y+1] );
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

	public bool TileCanBeEntered(int x, int y)
	{
		return tileTypes[tiles[x,y]].isWalkable;
	}

	public void GeneratePathTo(int x, int y)
	{
		unit.GetComponent<Unit> ().currentPath = null;
		currentPath = null;

		if(TileCanBeEntered(x,y) == false)
		{
			return;
		}

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
			/* This is very inefficient */
			//Node currentlyVisiting = unvisitedNodes.OrderBy( nodeDistance => distance[nodeDistance] ).First();
			/* This is better - but still suboptimal */
			Node currentlyVisiting = null;
			foreach(Node candidateNode in unvisitedNodes)
			{
				if ( currentlyVisiting == null || distance[candidateNode] < distance[currentlyVisiting] )
				{
					currentlyVisiting = candidateNode;
				}
			}

			if (currentlyVisiting == target)
			{
				break;
			}

			unvisitedNodes.Remove(currentlyVisiting);

			foreach(Node neighbour in currentlyVisiting.neighbours)
			{
				//float alt = distance[currentlyVisiting] + currentlyVisiting.DistanceTo(neighbour);
				float alt = distance[currentlyVisiting] + currentlyVisiting.DistanceTo(neighbour) + CostToEnterTile(neighbour);
				if ( alt < distance[neighbour] )
				{
					distance[neighbour] = alt;
					previous[neighbour] = currentlyVisiting;
				}
			}
		}

		/* In this case, there is no route */
		if (distance [target] == Mathf.Infinity || previous [target] == null) 
		{
			return;
		} 
		else 
		{
			/* Here I "backtrack" from the target to the source, creating the path */
			currentPath = new List<Node>();
			Node current = target;
			while(current != null)
			{
				currentPath.Add(current);
				current = previous[current];
			}

			currentPath.Reverse();
		}

		unit.GetComponent<Unit> ().currentPath = currentPath;
	}

	/* Start Imbricated Class */
	/* The class was added here in order to make it accesible from another script */
	/* Refactoring needed */
	/* The Node class was moved iin it's own file "Node" */
	/* End Imbricated Class */
}
