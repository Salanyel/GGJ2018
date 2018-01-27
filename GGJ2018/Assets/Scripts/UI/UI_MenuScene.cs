using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuScene : MonoBehaviour {

	public void OnClickPlayGame() {
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void OnClickExitGame() {
		Application.Quit();
	}
}
