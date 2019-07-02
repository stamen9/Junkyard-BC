using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//not sure if the MonoBehavior is needed
abstract public class Ability : MonoBehaviour {

	public Image abilityIcon;
	public string abilityName;
	public string abilityDescription;

	abstract public void Effect ();
}
