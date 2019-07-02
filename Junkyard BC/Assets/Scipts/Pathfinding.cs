using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	public Grid map;

	public int currentX;
	public int currentY;

	public int targetX;
	public int targetY;

	public HexNode sellectedTile;

	public GameObject unit;

	bool leagalMove = true;

	public Vector3 CurrentlyMovingTo;

	public int speed = 1;

	public bool isMoving ;
	public bool movingAllowed ;

	public HashSet<HexNode> BorderClosedSet = new HashSet<HexNode>();

	public List<HexNode> finalPath;

	//maybe bad name :S
	//Should be SetBorder
	public void SetMoveBorder(int n,int curX, int curY, Color? lineColor = null )
	{
		List<HexNode> openSet = new List<HexNode> ();
		BorderClosedSet.Clear ();
		HexNode start = map.hexNodeGrid [curX, curY];
		openSet.Add(start);

		//might be better to have this called at an earlyer point before the function call

		while (openSet.Count > 0) {
			//Debug.Log (openSet.Count);
			HexNode currentSearch = openSet [0];
			BorderClosedSet.Add (currentSearch);
			openSet.Remove (currentSearch);

			//slight optimization to stop a few cicles earlier
			//with a rangwe of 5 cuts from 50 down to 35 cycles;
			if ((currentSearch.gCost + 1) > ((n) * 10)) {
				continue;
			}

			//Debug.Log ("123456789");
			List<HexNode> neighbours;
			if(lineColor == null)
				neighbours = map.GetHexNeighbours (currentSearch);
			else
				neighbours = map.GetHexNeighbours (currentSearch, false);
			foreach (HexNode neighbour in neighbours) 
			{
				if (BorderClosedSet.Contains (neighbour))
				{
					continue;
				}
				int newCostToNeighbour = currentSearch.gCost + GetDistance (currentSearch, neighbour);
//				Debug.Log ("cost " + newCostToNeighbour + " to neighbor x: " + neighbour.posX + ", y:" + neighbour.posY);
				if (newCostToNeighbour > ((n) * 10)) 
				{
					continue;
				}
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) 
				{
					neighbour.gCost = newCostToNeighbour;
					//neighbour.hCost = GetDistance (neighbour, sellectedTile);
					neighbour.parent = currentSearch;
				
					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}
		BorderClosedSet.Remove (start);
		foreach (HexNode node in BorderClosedSet) 
		{
			map.GameObject_hexNodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().startColor = lineColor ?? Color.blue;
			map.GameObject_hexNodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().endColor = lineColor ?? Color.blue;
			map.GameObject_hexNodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().enabled = true;
		}
	}
	public void setNotPassable(int _x, int _y)
	{
		map.queueToBlockX.Add (_x);
		map.queueToBlockY.Add (_y);
	}
	void MoveOrder ()
	{
		
		CurrentlyMovingTo = finalPath[0].Tile.transform.position;
		finalPath.RemoveAt(0);
	}

	void Start()
	{
		isMoving = false;
		movingAllowed = true;
		finalPath = new List<HexNode> ();
	}

	void Update () {

		if (unit && leagalMove) {
			if (unit.transform.position == CurrentlyMovingTo && finalPath.Count != 0 && CurrentlyMovingTo != null) {
				MoveOrder ();
				if (finalPath.Count == 0) {

					unit.GetComponent<Unit> ().posX = targetX;
					unit.GetComponent<Unit> ().posY = targetY;
					//Debug.Log ("finished");
				}
			}
		}

		//This can be optimized
		float step = speed * Time.deltaTime;
		if (sellectedTile != null && CurrentlyMovingTo != unit.transform.position && leagalMove) {
			CurrentlyMovingTo.y = unit.transform.position.y;
			unit.transform.position = Vector3.MoveTowards (unit.transform.position, CurrentlyMovingTo, step);
		} else
			isMoving = false;
		
	}

	public void OnCall(int x , int y, GameObject sellectedUnit,bool AI = false)
	{
		
		if (movingAllowed == true && isMoving!=true)
		{
			isMoving = true;
			unit = sellectedUnit;
			currentX = unit.GetComponent<Unit> ().posX;
			currentY = unit.GetComponent<Unit> ().posY;
			//Debug.Log (currentX);
			targetX = x;
			targetY = y;
			sellectedTile = map.hexNodeGrid[x,y];

			if (FindPath (null,AI) <= 97) {
				MoveOrder();
			} else {
				Debug.Log("FindPath () == 0");
			}
		}
		else Debug.Log("Cant move");
	}

	//target is kinda useless????
	public int FindPath(Charecter target = null, bool AI = false)
	{
		if (target) {
			Debug.Log (map.hexNodeGrid);
			sellectedTile = map.hexNodeGrid [target.posX, target.posY];
		}
		HexNode start = map.hexNodeGrid [currentX, currentY];
		HashSet<HexNode> closedSet = new HashSet<HexNode>();
		List<HexNode> openSet = new List<HexNode> ();

		openSet.Add(start);
		while (openSet.Count > 0) {
			HexNode currentSearch = openSet [0];
			for (int i = 1; i < openSet.Count; i++) {
				if(openSet[i].fCost < currentSearch.fCost || openSet[i].fCost == currentSearch.fCost)
				{
					currentSearch = openSet [i];
				}
			}

			closedSet.Add (currentSearch);
			openSet.Remove (currentSearch);

			if (currentSearch == sellectedTile) {
				return RetracePath(start, sellectedTile, AI);
			}

			foreach (HexNode neighbour in map.GetHexNeighbours(currentSearch)) {
				if (!neighbour.passable || closedSet.Contains (neighbour)) {
					continue;
				}
				int newCostToNeighbour = currentSearch.gCost + GetDistance (currentSearch, neighbour);// should be set to static +10;
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour, sellectedTile);
					neighbour.parent = currentSearch;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}
		return 98;
	}
	public int GetDistance(HexNode nodeA, HexNode nodeB) {
		int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
		int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

		if (dstX > dstY)
			return 10*dstY + 10* (dstX-dstY);
		return 10*dstX + 10 * (dstY-dstX);
	}

	int RetracePath(HexNode startNode, HexNode endNode, bool AI = false) {
		List<HexNode> path = new List<HexNode>();
		HexNode currentNode = endNode;
		startNode.passable = true;

		while (currentNode != startNode) {

			//Debug.Log (currentNode.posX + "  " + currentNode.posY);
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		finalPath = path;
		finalPath.Add (endNode);
		int leftMove = unit.GetComponent<Charecter> ().leftMove;
		if (finalPath.Count-1 > leftMove /*&& finalPath.Count != unit.GetComponent<Monster> ().leftMove*/) {
			if (AI) {
				finalPath.RemoveRange (leftMove, finalPath.Count - leftMove);
				targetX = finalPath [leftMove - 1].posX;
				targetY = finalPath [leftMove - 1].posY;
				endNode = finalPath [leftMove - 1];

			} else {
				Debug.Log ("Not enogh movement");
				leagalMove = false;
				finalPath.Clear ();
				return 99;
			}

		}
		endNode.passable = false;
		leagalMove = true;
		foreach (HexNode node in BorderClosedSet) 
		{
			node.gCost = 0;
			map.GameObject_hexNodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().enabled = false;
		}
		unit.GetComponent<Charecter> ().leftMove -= finalPath.Count-1;
		//Debug.Log (unit.GetComponent<Charecter> ().leftMove);
		SetMoveBorder (unit.GetComponent<Charecter>().leftMove, targetX, targetY);
		return finalPath.Count;
	}

	public void ClearLines()
	{
		foreach (HexNode node in BorderClosedSet) 
		{
			node.gCost = 0;
			map.GameObject_hexNodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().enabled = false;
		}
		BorderClosedSet.Clear ();
	}
}
