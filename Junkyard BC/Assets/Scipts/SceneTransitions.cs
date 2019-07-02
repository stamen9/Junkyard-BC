using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour {

	public Animator animTransition;
	// Use this for initialization
	void Start () {
		
	}

	public void TransitionToScene(string sceneName)
	{
		Debug.Log ("what??");
		StartCoroutine(PlayLeveSceneAnim (sceneName));
	}

	public IEnumerator PlayLeveSceneAnim(string sceneName)
	{
		Debug.Log ("AnimHit");
		animTransition.SetTrigger ("end");
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene (sceneName);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
