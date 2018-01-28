using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForInteraction : MonoBehaviour {

	bool _isInformationSent = false;

	void Update() {
		if (!_isInformationSent) {
			DetectInput (1);
			DetectInput (2);
			DetectInput (3);
			DetectInput (4);
			if (Input.GetMouseButtonDown(0)) {
				_isInformationSent = true;
				GameManager.Instance.ChangeGameState(ENUM_GAMESTATE.CINEMATICS);
			}
		}
	}

	void OnEnable() {
		_isInformationSent = false;
	}

	void DetectInput(int p_player) {
		if (Input.GetButtonDown(InputData._Action + p_player)) {
			_isInformationSent = true;
			GameManager.Instance.ChangeGameState(ENUM_GAMESTATE.CINEMATICS);
		}
	}

}
