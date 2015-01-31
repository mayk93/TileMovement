using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Node
{
	public List<Node> neighbours; // The edges
	public int x;
	public int y;
	
	public Node()
	{
		neighbours = new List<Node>();
	}
	
	public float DistanceTo(Node otherNode)
	{
		if(otherNode == null)
		{
			Debug.Log("Null other node!");
		}
		return Vector2.Distance ( new Vector2(x,y) , new Vector2(otherNode.x,otherNode.y) );
	}
}
