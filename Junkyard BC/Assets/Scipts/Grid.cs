using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class Grid : MonoBehaviour {

	//instance of the prop which will be spaed for blocking tiles
	//All variables should be [SerializeField]private if possible
	//Need to fix this at a later point
	[SerializeField]private GameObject blockInstance;

	public struct Tile{
		public Sprite rawImage;
		public string name;

		public Tile(string _source, string _name)
		{
			name = _name;
			if(File.Exists(_source))
			{
				Texture2D temp;
				rawImage = Resources.Load<Sprite>(_source);
			}
			else
			{
				//Debug.Log("File missing? -> " + _source);
				rawImage = Resources.Load<Sprite>(_source);
			}

		}
	}
	public struct Block{
		int x;
		int y;
		GameObject instance;
		public Block(int _x, int _y, Sprite img, GameObject _instance)
		{
			x = _x;
			y = _y;
			instance = (GameObject) Instantiate (_instance, new Vector3(x*(0.264f/2f) + y*(0.264f),-0.05f,x*(0.264f*0.75f)),Quaternion.Euler(0,0,0));
			instance.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
			instance.GetComponent<SpriteRenderer>().sprite = img;
			instance = null;
		}
	}

	//Unit which is currently turn
	public GameObject currentSelectedUnit;

	//tile which is used for the groud
	public GameObject InstanceTile;
	public GameObject HexInstanceTile;
	public GameObject MapContainer;
	public int sizeX;
	public int sizeY;
	public int radius;
	public Player playerOne;
	public Player playerTwo;
	public string tileResourceFile;

	public List<int> queueToBlockX = new List<int>();
	public List<int> queueToBlockY = new List<int>();
	//public List<Player> players;



	//[SyncVar]
	//public int currentPlayer = 0;

	public Pathfinding pathfinder;

	public List<Tile> tilesArray;

	public Node[,] nodeGrid;
	//Would be a good ideas if eventually changed to a hash table?
	//Array of arrays is a saves memory but will not work well for finding neighbours
	//when the mab has holes/is not a `normal` shape
	public HexNode[,] hexNodeGrid;
	public GameObject[,] GameObject_nodeGrid;
	public GameObject[,] GameObject_hexNodeGrid;
	public List<Block> blockingTiles;
	// Use this for initialization


	public void GenerateRectGrid()
	{
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
	//This will need to be changed once I have tought of an algorithm which can generate maps 
	public void GenerateHexGrid(int r/* hex radius*/)
	{

		hexNodeGrid = new HexNode[r * 2 + 1, r * 2 + 1];
		GameObject_hexNodeGrid = new GameObject[r * 2 + 1, r * 2 + 1];
		for (int i = 0; i < (r * 2 + 1); i++) {
			for (int j = 0; j < (r * 2 + 1); j++) {
				if ((i + j) < (r) || (i + j) > (r * 3)) {
					hexNodeGrid [i, j] = null;
				} else {
					GameObject Clone = (GameObject) Instantiate (HexInstanceTile, new Vector3(i*(0.264f/2f) + j*(0.264f),0,i*(0.264f*0.75f)),Quaternion.Euler(90,0,0),this.transform);
					Clone.GetComponent<CallMove> ().x = i;
					Clone.GetComponent<CallMove> ().y = j;
					GameObject_hexNodeGrid [i, j] = Clone;
					hexNodeGrid [i, j] = new HexNode (i, j, 0, true, Clone);
//					Debug.Log ("i: " + i + " j: " + j);
				}
			}
		}


	}
	public void ReadTileFiles()
	{
		string fullPath = Path.Combine ("../Junkyard BC/Assets/Resources/TileAssets/", tileResourceFile);
		tilesArray = new List<Tile> ();
		if(File.Exists(fullPath))
		{
			StreamReader file = new StreamReader (fullPath);
			string line = "";
			while ((line = file.ReadLine()) != null) {
				string[] splitData = line.Split (' ');
				string assetPath = Path.Combine ("TileAssets/", splitData [0]);
				Tile temp= new Tile(assetPath,splitData [1]);
				tilesArray.Add (temp);
			}

		}
	}

	public void ReadAndApplyTiles()
	{
		string fullPath = "../Junkyard BC/Assets/Resources/TileAssets/mapBlockedTiles.txt";
		blockingTiles = new List<Block> ();
		if(File.Exists(fullPath))
		{
			StreamReader file = new StreamReader (fullPath);
			string line = "";
			while ((line = file.ReadLine()) != null) {
				string[] splitData = line.Split (' ');
				Block temp = new Block (int.Parse(splitData [0]), int.Parse(splitData [1]),tilesArray[int.Parse(splitData [3])].rawImage, blockInstance);
				blockingTiles.Add (temp);
				hexNodeGrid [int.Parse (splitData [0]), int.Parse (splitData [1])].passable = false;
			}

		}
	}

	void Start () {
		ReadTileFiles ();
		//GenerateRectGrid ();
		GenerateHexGrid(radius);

		ReadAndApplyTiles ();

		int len = queueToBlockX.Count;
		for (int i = 0; i < len; i++) {
			Debug.Log ( "check X: " + queueToBlockX [0] + " check Y: " + queueToBlockY [0]);
			hexNodeGrid [queueToBlockX [0], queueToBlockY [0]].passable = false;
			queueToBlockX.RemoveAt (0);
			queueToBlockY.RemoveAt (0);
		}
		//pathfinder.enabled = true;
		GameObject.Find ("TurnController").GetComponent<TurnController> ().enabled = true;
	}

	public void Pass_X_Y_toPathfinding(int x, int y)
	{
		if (currentSelectedUnit.GetComponent<Unit> ().type == 'b') {
			return;
		} else if (currentSelectedUnit.GetComponent<Unit> ().type == 'm') {
			if (currentSelectedUnit.GetComponent<Charecter> ().setToMove) {
				pathfinder.OnCall (x, y, currentSelectedUnit);
			} else if (currentSelectedUnit.GetComponent<Charecter> ().setToAttack) {
				
			}
		} else {
			Debug.Log ("No unit type?");
		}
		
		//currentSelectedUnit.GetComponent<Pathfinding>().OnCall(x,y);
	}

	//obsolete method
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

	//obsolete method
	public List<Node> GetAllNeighbours(Node center)
	{
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ( x == 0 && y == 0)
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

	public List<HexNode> GetHexNeighbours(HexNode center, bool ignoreBlocked = true)
	{
		//Debug.Log ("Call!!!!");
		List<HexNode> neighbours = new List<HexNode>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ((x + y) == -2 || (y + x) == 2 || (x == 0 && y == 0) ) {
					continue;
				}
				if (x == -1 && y == 1) {
					//Debug.Log ("What???");
				}
				int checkX = center.posX + x;
				int checkY = center.posY + y;
				int limit = radius * 2 + 1;
				if (checkX >= 0 && checkX < limit && checkY >= 0 && checkY < limit)
				{
					if (ignoreBlocked) {
						if (hexNodeGrid [checkX, checkY] != null && hexNodeGrid [checkX, checkY].passable == true) {
							//Debug.Log (hexNodeGrid [checkX, checkY].passable);
							neighbours.Add (hexNodeGrid [checkX, checkY]);
						}
					} else {
						if (hexNodeGrid [checkX, checkY] != null) {
							//Debug.Log (hexNodeGrid [checkX, checkY].passable);
							neighbours.Add (hexNodeGrid [checkX, checkY]);
						}
					}
				}
			}
		}
		return neighbours;
	}

	public void SetToMove()
	{
		Charecter monster = currentSelectedUnit.GetComponent<Charecter> ();
		//Debug.Log ("987654321");
		pathfinder.SetMoveBorder (monster.leftMove,monster.posX,monster.posY);
		monster.setToMove = true;
		monster.setToAttack = false;
		monster.attemptingAttack = 0;
	}
	public void SetToAttack(int skill)
	{
		Charecter monster = currentSelectedUnit.GetComponent<Charecter> ();
		monster.setToMove = false;
		monster.setToAttack = true;
		monster.attemptingAttack = skill;
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.deltaTime);
		
	}
}
