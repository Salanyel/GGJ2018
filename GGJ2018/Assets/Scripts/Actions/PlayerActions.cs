using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActions : MonoBehaviour {

	#region Variables

	public string _actionKey = "";		
	public float coolDownTime = 1f;

	protected float lastAction;
	bool _currentAction = false;

	#endregion

	protected abstract void DoAction();
	protected abstract void DoReleaseAction();
	protected abstract void Awake();
	protected abstract void DoBehaviour();

	protected void Action() {
		//verifie le cooldown
		if(Time.time >= lastAction + coolDownTime){
			lastAction = Time.time;
			_currentAction = true;
			DoAction();
		}
	}

	protected void ReleaseAction(){
		if (_currentAction) {
			_currentAction = false;
			DoReleaseAction ();
		}
	}

	void Update() {
		if (!SecurityIsOk()) {
			return;
		}

		if (Input.GetButtonDown(_actionKey)) {
			Action();
		}

		if(Input.GetButtonUp(_actionKey)){
			ReleaseAction();
		}

		DoBehaviour();
		
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
