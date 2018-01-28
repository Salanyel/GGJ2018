using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSick : MonoBehaviour {

	void Start() {
		GetComponentInChildren<Animator> ().SetBool ("isSick", true);
	}
}
