using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actions : MonoBehaviour {

	protected string _actionKey;

	protected abstract void Awake();
	public abstract void Action();

	void Update() {
		if (Input.GetButtonDown(_actionKey)) {
			Action();
		}
	}
}
