using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public int posX;
	public int posY;
	public int gCost;
	public int hCost;
	public bool passable;
	public Node parent;

	public GameObject Tile;

	public Node(int _x , int _y , bool pas , GameObject _tile)
	{
		posX = _x;
		posY = _y;
		passable = pas;
		Tile = _tile;
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}
}
