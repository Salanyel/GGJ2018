using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Illness {

	override public bool IsGameFinished() {
		bool _areAllPlayerSick = true;
		int i  = 0;
		foreach (GameObject player in GameManager.Instance.AllPlayers) {
			if (!player.GetComponent<Player>().IsContamined) {
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
