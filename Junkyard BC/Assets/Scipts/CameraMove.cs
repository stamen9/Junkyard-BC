using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			if (Input.GetKey("w"))
			{

				this.transform.Translate(new Vector3(0f, 0f, 1f * Time.deltaTime));

			}
			else if (Input.GetKey("s"))
			{

				this.transform.Translate(new Vector3(0f, 0f, -1f * Time.deltaTime));

			}
			else if (Input.GetKey("a"))
			{

				this.transform.Translate(new Vector3(-1f * Time.deltaTime, 0f, 0f));

			}
			else if (Input.GetKey("d"))
			{

				this.transform.Translate(new Vector3(1f * Time.deltaTime, 0f, 0f));

			}
	}
}
