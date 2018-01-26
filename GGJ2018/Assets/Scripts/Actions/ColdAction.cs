using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : Actions {

	override protected void Awake() {
		_actionKey = InputData._ActionKeyboard;
	}

	override public void Action() {
		Debug.Log("--- Cold action");
	}
}
