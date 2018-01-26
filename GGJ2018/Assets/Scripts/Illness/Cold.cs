using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Illness {

	override public bool IsGameFinished() {
		bool _areAllPlayerSick = true;

		foreach (Player player in GameManager.Instance.AllPlayers) {
			if (!player.IsContamined) {
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
