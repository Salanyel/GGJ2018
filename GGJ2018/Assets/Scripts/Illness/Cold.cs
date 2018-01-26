using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Illness {

	override public bool IsGameFinished() {
		bool _areAllPlayerSick = true;

		foreach (int player in GameManager.Instance.AllPlayers) {
			Debug.Log("--- " + player);
			if (player == 4) {
				_areAllPlayerSick = false;
				break;
			}
		}

		if (_areAllPlayerSick == true) {
			return true;
		} else {
			return false;
		}
	}

}
