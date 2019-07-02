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

	public Vector3 CurrentlyMovingTo;

	public int speed = 1;

	public bool isMoving ;
	public bool movingAllowed ;



	public List<Node> finalPath;


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

		if (unit) {
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
		if (sellectedTile != null && CurrentlyMovingTo != unit.transform.position) {
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
			isMoving = true;
			FindPath();
			MoveOrder();
		}
		else Debug.Log("Cant move");
	}
	void FindPath()
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
				RetracePath(start, sellectedTile);
				return;
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
	}
	public int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
		int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

	void RetracePath(Node startNode, Node endNode) {
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

		if (finalPath.Count > unit.GetComponent<Monster> ().leftMove) {
			Debug.Log ("Not enogh movement");
			finalPath.Clear();
			return;
		}
		unit.GetComponent<Monster> ().leftMove -= finalPath.Count;
	}
}
