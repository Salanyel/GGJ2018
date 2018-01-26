using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotContamined : Actions {

	override protected void Awake() {
		_actionKey = InputData._ActionKeyboard;
	}

	override public void Action() {
		Debug.Log("--- Not contamined action");
	}
}
