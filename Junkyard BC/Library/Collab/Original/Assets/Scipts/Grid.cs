using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Grid : MonoBehaviour {


	public GameObject currentSelectedUnit;

	public GameObject InstanceTile;
	public GameObject MapContainer;
	public int sizeX;
	public int sizeY;

	public Player playerOne;
	public Player playerTwo;

	//public List<Player> players;

	//[SyncVar]
	//public int currentPlayer = 0;

	public Pathfinding pathfinder;

	public Node[,] nodeGrid;
	public GameObject[,] GameObject_nodeGrid;

	// Use this for initialization
	void Start () {

		nodeGrid = new Node[sizeX, sizeY];
		GameObject_nodeGrid = new GameObject[sizeX, sizeY];
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				GameObject Clone = (GameObject) Instantiate (InstanceTile, new Vector3(i*0.16f,0,j*0.16f),Quaternion.Euler(90,0,0),this.transform);
				Clone.GetComponent<CallMove> ().CallForMove = this;
				Clone.GetComponent<CallMove> ().x = i;
				Clone.GetComponent<CallMove> ().y = j;
				GameObject_nodeGrid [i, j] = Clone;
				nodeGrid [i, j] = new Node (i, j, true, Clone);
			}
		}
	}

	public void Pass_X_Y_toPathfinding(int x, int y)
	{
		if (currentSelectedUnit.GetComponent<Unit> ().type == 'b') {
			return;
		} else if (currentSelectedUnit.GetComponent<Unit> ().type == 'm') {
			if (currentSelectedUnit.GetComponent<Monster> ().setToMove) {
				pathfinder.OnCall (x, y, currentSelectedUnit);
			} else if (currentSelectedUnit.GetComponent<Monster> ().setToAttack) {
				
			}
		} else {
			Debug.Log ("No unit type?");
		}
		
		//currentSelectedUnit.GetComponent<Pathfinding>().OnCall(x,y);
	}
	public List<Node> GetNeighbours(Node center)
	{
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ( ( x + y ) < -1 || ( y + x ) > 1  || ( x + y ) == 0 )
					continue;
				int checkX = center.posX + x;
				int checkY = center.posY + y;

				if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
				{
					neighbours.Add(nodeGrid[checkX, checkY]);
				}
			}
		}

		return neighbours;

	}

	public void SetToMove()
	{
		Monster monster = currentSelectedUnit.GetComponent<Monster> ();
		pathfinder.SetMoveBorder (monster.leftMove,monster.posX,monster.posY);
		monster.setToMove = true;
		monster.setToAttack = false;
	}
	public void SetToAttack()
	{
		Monster monster = currentSelectedUnit.GetComponent<Monster> ();
		monster.setToMove = false;
		monster.setToAttack = true;
		monster.attemptingAttack = true;
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.deltaTime);
		
	}
}
