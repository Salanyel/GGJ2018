using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActions : MonoBehaviour {

	public string _actionKey = "";

	protected abstract void Awake();
	public abstract void Action();

	void Update() {
		
		if (!SecurityIsOk()) {
			return;
		}

		if (Input.GetButtonDown(_actionKey)) {
			Action();
		}
	}

	bool SecurityIsOk() {
		if (_actionKey == "") {
			return false;
		}

		return true;
	}

	public void SetActionKey(int p_playerNumber) {
		_actionKey = InputData._Action + p_playerNumber;
		Debug.Log("New action key: " + _actionKey);
	}

}
