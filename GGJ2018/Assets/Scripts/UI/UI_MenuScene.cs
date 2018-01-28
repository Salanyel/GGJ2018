using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuScene : MonoBehaviour {

	GameObject _loadingScreen;

	void Awake() {
		_loadingScreen = GameObject.FindGameObjectWithTag (Tags._loadingScreen);
	}

	void Start() {
		_loadingScreen.SetActive (false);
	}

	public void OnClickPlayGame() {
		_loadingScreen.SetActive (true);
		StartCoroutine (NextLevel ());
	}

	public void OnClickExitGame() {
		Application.Quit();
	}

	IEnumerator NextLevel() {
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
}
