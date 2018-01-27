using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Levels : MonoBehaviour {
	public void OnClickRetry() {
		GameManager.Instance.ChangeGameState (ENUM_GAMESTATE.RESET);
	}
}
