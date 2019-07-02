using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexNode {

	public int posX;
	public int posY;
	public int posZ;//useless? actually probably not
	public int gCost;
	public int hCost;
	public bool passable;
	public HexNode parent;//useless?
	public List<HexNode> children;//useless?

	public int index;

	public GameObject Tile;

	public HexNode(int _x , int _y , int _z,bool pas , GameObject _tile)
	{
		posX = _x;
		posY = _y;
		posZ = _z;
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
