using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Illness {

	GIF _gif;

	void Awake() {
		_gif = FindObjectOfType<GIF> ();
		_gif.gameObject.SetActive (false);
	}

	override public bool IsGameFinished() {
		bool _areAllPlayerSick = true;

		foreach (GameObject player in GameManager.Instance.AllPlayers) {
			if (!player.GetComponent<Player>()._isContamined) {
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

	override public void LaunchCinematic(GameObject p_camera, Vector3 p_firstPosition, Vector3 p_firstEulerAngles) {
		StartCoroutine (Cinematic (p_camera, p_firstPosition, p_firstEulerAngles));
	}

	IEnumerator Cinematic(GameObject p_camera, Vector3 p_firstPosition, Vector3 p_firstEulerAngles) {
		float cinematicDuration = 2f;
		float timeElapsed = 0f;
		Vector3 finalPosition = p_camera.transform.position;
		Vector3 finalEulerAngles = p_camera.transform.eulerAngles;

		p_camera.transform.position = p_firstPosition;
		p_camera.transform.eulerAngles = p_firstEulerAngles;

		_gif.gameObject.SetActive (true);
		_gif.LaunchGif ();

		yield return new WaitForSeconds (2.5f);

		_gif.ReverseGIF ();

		while (timeElapsed < cinematicDuration) {
			Vector3 position = Vector3.Lerp (p_firstPosition, finalPosition, timeElapsed / cinematicDuration);
			Vector3 eulerAngles = Vector3.Lerp (p_firstEulerAngles, finalEulerAngles, timeElapsed / cinematicDuration);

			p_camera.transform.position = position;
			p_camera.transform.eulerAngles = eulerAngles;

			timeElapsed += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		_gif.gameObject.SetActive (false);

		p_camera.transform.position = finalPosition;
		p_camera.transform.eulerAngles = finalEulerAngles;

		GameManager.Instance.ChangeGameState (ENUM_GAMESTATE.COUNTDOWN);
	}
}
