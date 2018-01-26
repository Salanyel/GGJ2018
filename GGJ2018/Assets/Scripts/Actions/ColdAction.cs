using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	override protected void Awake() {
		_actionKey = InputData._Action + GetComponent<Player>().PlayerNumber;
	}

	override public void Action() {
		Debug.Log("--- Cold action");
	}
}
