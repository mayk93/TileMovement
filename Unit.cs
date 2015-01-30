using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public List<TileMap.Node> currentPath = null;

	int currentNode;
	float epsilon;

	float movementSpeed = 5f;

	// Use this for initialization
	void Start () {
		currentNode = 0;
		epsilon = 0.1f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/* This needs heavy reworking */
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
	}
}
