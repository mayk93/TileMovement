using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public List<Node> currentPath = null;

	public TileMap map;

	public float movementPoints;

	//Part of the old code
	/*
	int currentNode;
	float epsilon;
	float movementSpeed = 5f;
	*/

	// Use this for initialization
	void Start () 
	{
		movementPoints = 2f;
		//Part of the old code
		/*
		currentNode = 0;
		epsilon = 0.1f;
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(currentPath != null)
		{
			int currentNode = 0;
			while( currentNode < currentPath.Count-1 )
			{
				Vector3 start = map.TileCoordinatesToWorldCoordinates( currentPath[currentNode].x , currentPath[currentNode].y ) + new Vector3(0,0,-1f);
				Vector3 end = map.TileCoordinatesToWorldCoordinates( currentPath[currentNode + 1].x , currentPath[currentNode+1].y ) + new Vector3(0,0,-1f);

				Debug.DrawLine(start,end,Color.red);

				currentNode++;
			}
		}

		/* This will be called via the move button */
		//MoveToNextTile ();

		/* This needs heavy reworking */
		/* This is the old code */
		/*
		if(currentPath != null)
		{	
			try
			{
				if(currentPath[currentNode] != null && currentPath != null)
				{
					transform.position = new Vector3( Mathf.Lerp(transform.position.x,currentPath[currentNode].x,Time.deltaTime*movementSpeed) , Mathf.Lerp(transform.position.y,currentPath[currentNode].y,Time.deltaTime*movementSpeed) , 0 );
				}
			
				if( Mathf.Abs( transform.position.x - currentPath[currentNode].x ) < epsilon && Mathf.Abs( transform.position.y - currentPath[currentNode].y ) < epsilon && currentPath != null )
				{
					try
					{
						tileX = currentPath[currentNode].x;
						tileY = currentPath[currentNode].y;
						if(currentPath[currentNode+1] != null)
						{
							currentNode = currentNode + 1;
						}
					}
					catch
					{
						currentPath = null;
						currentNode = 0;
					}
				}
			}
			catch
			{
				currentPath = null;
				currentNode = 0;
			}
		}
		*/
	}

	public void MoveToNextTile()
	{
		float remainingMovementPoints = movementPoints;
		while(remainingMovementPoints > 0)
		{
			if(currentPath != null)
			{
				// The first node is the tile we are allready standing on.
				currentPath.RemoveAt (0);
				
				transform.position = map.TileCoordinatesToWorldCoordinates(currentPath[0].x, currentPath[0].y);
				tileX = currentPath[0].x;
				tileY = currentPath[0].y;

				remainingMovementPoints = remainingMovementPoints - map.CostToEnterTile(currentPath[0]);

				if(currentPath.Count == 1)
				{
					currentPath = null;
				}
			}
			else
			{
				remainingMovementPoints = 0;
			}
		}
	}
}
