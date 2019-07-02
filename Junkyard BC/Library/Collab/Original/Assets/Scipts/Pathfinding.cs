using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	public Grid map;

	public int currentX;
	public int currentY;

	public int targetX;
	public int targetY;

	public Node sellectedTile;

	public GameObject unit;

	bool leagalMove = true;

	public Vector3 CurrentlyMovingTo;

	public int speed = 1;

	public bool isMoving ;
	public bool movingAllowed ;

	public HashSet<Node> BorderClosedSet = new HashSet<Node>();


	public List<Node> finalPath;


	public void SetMoveBorder(int n,int curX, int curY)
	{
		
		List<Node> openSet = new List<Node> ();
		BorderClosedSet.Clear ();
		Node start = map.nodeGrid [curX, curY];
		openSet.Add(start);
		while (openSet.Count > 0) {
			Node currentSearch = openSet [0];


			BorderClosedSet.Add (currentSearch);
			openSet.Remove (currentSearch);

			foreach (Node neighbour in map.GetNeighbours(currentSearch)) {
				if (!neighbour.passable || BorderClosedSet.Contains (neighbour)) {
					continue;
				}
				int newCostToNeighbour = currentSearch.gCost + GetDistance (currentSearch, neighbour);
				if (newCostToNeighbour > ((n + 1) * 10)) {
					continue;
				}
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					//neighbour.hCost = GetDistance (neighbour, sellectedTile);
					neighbour.parent = currentSearch;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}
		foreach (Node node in BorderClosedSet) {
			map.GameObject_nodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().enabled = true;
		}
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
		finalPath = new List<Node> ();
	}

	void Update () {

		if (unit && leagalMove) {

			if (unit.transform.position == CurrentlyMovingTo && finalPath.Count != 0 && CurrentlyMovingTo != null) {
				MoveOrder ();
				if (finalPath.Count == 0) {

					unit.GetComponent<Unit> ().posX = targetX;
					unit.GetComponent<Unit> ().posY = targetY;
					Debug.Log ("finished");
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

	public void OnCall(int x , int y, GameObject sellectedUnit)
	{

		if (movingAllowed == true && isMoving!=true)
		{

			unit = sellectedUnit;
			currentX = unit.GetComponent<Unit> ().posX;
			currentY = unit.GetComponent<Unit> ().posY;
			targetX = x;
			targetY = y;
			sellectedTile = map.nodeGrid[x,y];


			if (FindPath () == 1) {
				MoveOrder ();
			} else {
				Debug.Log("FindPath () == 0");
			}

		}
		else Debug.Log("Cant move");
	}
	int FindPath()
	{
		Node start = map.nodeGrid [currentX, currentY];
		HashSet<Node> closedSet = new HashSet<Node>();
		List<Node> openSet = new List<Node> ();

		openSet.Add(start);
		while (openSet.Count > 0) {
			Node currentSearch = openSet [0];
			for (int i = 1; i < openSet.Count; i++) {
				if(openSet[i].fCost < currentSearch.fCost || openSet[i].fCost == currentSearch.fCost)
				{
					currentSearch = openSet [i];
				}
			}

			closedSet.Add (currentSearch);
			openSet.Remove (currentSearch);

			if (currentSearch == sellectedTile) {
				return RetracePath(start, sellectedTile);
			}

			foreach (Node neighbour in map.GetNeighbours(currentSearch)) {
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
		Debug.Log ("Log trace 6456");
		return 0;
	}
	public int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
		int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

	int RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {

			Debug.Log (currentNode.posX + "  " + currentNode.posY);
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		finalPath = path;
		finalPath.Add (endNode);

		if (finalPath.Count-1 > unit.GetComponent<Monster> ().leftMove /*&& finalPath.Count != unit.GetComponent<Monster> ().leftMove*/) {
			Debug.Log ("Not enogh movement");
			leagalMove = false;
			finalPath.Clear();
			return 0;
		}
		leagalMove = true;
		foreach (Node node in BorderClosedSet) {
			map.GameObject_nodeGrid [node.posX, node.posY].GetComponentInChildren<LineRenderer> ().enabled = false;
		}
		Debug.Log ("remove movemoent");
		unit.GetComponent<Monster> ().leftMove -= finalPath.Count-1;


		SetMoveBorder (unit.GetComponent<Monster>().leftMove, targetX, targetY);

		return 1;
	}
}
