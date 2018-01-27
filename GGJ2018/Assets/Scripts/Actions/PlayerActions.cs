using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActions : MonoBehaviour {

	#region Variables

	public string _actionKey = "";		
	public float coolDownTime = 1f;

	protected float lastAction;

	#endregion

	protected abstract void DoAction();
	protected abstract void Awake();

	protected void Action() {
		//verifie le cooldown
		if(Time.time >= lastAction + coolDownTime){
			lastAction = Time.time;

			DoAction();
		}
	}

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
	}

}
