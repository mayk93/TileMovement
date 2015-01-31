using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public List<Node> currentPath = null;

	public TileMap map;

	//Part of the old code
	/*
	int currentNode;
	float epsilon;
	float movementSpeed = 5f;
	*/

	// Use this for initialization
	void Start () 
	{
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

		/* This needs heavy reworking */
		/* This is the old code */
		/*
		if(currentPath != null)
		{
			if(currentPath[currentNode] != null)
			{
				transform.position = new Vector3( Mathf.Lerp(transform.position.x,currentPath[currentNode].x,Time.deltaTime*movementSpeed) , Mathf.Lerp(transform.position.y,currentPath[currentNode].y,Time.deltaTime*movementSpeed) , 0 );
			}
		
			if( Mathf.Abs( transform.position.x - currentPath[currentNode].x ) < epsilon && Mathf.Abs( transform.position.y - currentPath[currentNode].y ) < epsilon )
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
		*/
	}
}
