using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CallMove : MonoBehaviour {

	public int x;
	public int y;
	public Grid CallForMove;

	GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;
	EventSystem m_EventSystem;

	void Start()
	{
		Vector3[] vertices = new Vector3[7];
		int[] triangles = new int[18];

		vertices [0] = new Vector3 (   0f,    0f, 0f);
		vertices [1] = new Vector3 (   0f,    0.3f, 0f);
		vertices [2] = new Vector3 (   0.3f,  0.15f, 0f);
		vertices [3] = new Vector3 (   0.3f, -0.15f, 0f);
		vertices [4] = new Vector3 (   0f,   -0.3f, 0f);
		vertices [5] = new Vector3 (  -0.3f, -0.15f, 0f);
		vertices [6] = new Vector3 (  -0.3f,  0.15f, 0f);



		triangles [0] = 0;
		triangles [1] = 1;
		triangles [2] = 2;

		triangles [3] = 2;
		triangles [4] = 3;
		triangles [5] = 0;

		triangles [6] = 0;
		triangles [7] = 3;
		triangles [8] = 4;

		triangles [9] = 4;
		triangles [10] = 5;
		triangles [11] = 0;

		triangles [12] = 0;
		triangles [13] = 5;
		triangles [14] = 6;

		triangles [15] = 6;
		triangles [16] = 1;
		triangles [17] = 0;

		Mesh collider = new Mesh ();

		collider.vertices = vertices;
		collider.triangles = triangles;
		 
		this.transform.GetComponent<MeshCollider> ().sharedMesh = collider;

		this.name = x + "," + y;

		//CallForMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<BattleMove>();
		//Fetch the Raycaster from the GameObject (the Canvas)
		m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		//Fetch the Event System from the Scene
		m_EventSystem = GameObject.Find("Canvas").GetComponent<EventSystem>();
	}



	public void OnMouseOver()
	{
		Vector3 worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		/*Collider2D blocker = Physics2D.OverlapPoint(worldMouse);
		Debug.Log (blocker);*/
		if (Input.GetMouseButtonDown (0)) {
			
			//Set up the new Pointer Event
			m_PointerEventData = new PointerEventData(m_EventSystem);
			//Set the Pointer Event Position to that of the mouse position
			m_PointerEventData.position = Input.mousePosition;

			//Create a list of Raycast Results
			List<RaycastResult> results = new List<RaycastResult>();

			//Raycast using the Graphics Raycaster and mouse click position
			m_Raycaster.Raycast(m_PointerEventData, results);

			if (results.Count > 0) {
				Debug.Log ("UI is in the way!");
				return;
			}

			CallForMove.Pass_X_Y_toPathfinding (x, y);
		}
	}
}
