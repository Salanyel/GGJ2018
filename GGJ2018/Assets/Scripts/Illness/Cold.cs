using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Illness {

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

	override public void LaunchCinematic(GameObject p_camera) {
		Debug.Log ("--- With camera: " + p_camera.name, p_camera);
		//StartCoroutine (Cinematic (p_camera));
	}

	IEnumerator Cinematic(GameObject p_camera) {
		float cinematicDuration = 5;
		float timeElapsed = 0f;
		Vector3 finalPosition = p_camera.transform.position;
		Vector3 finalEulerAngles = p_camera.transform.eulerAngles;

		Debug.Log ("Setup ready");

		while (timeElapsed < cinematicDuration) {
			Vector3 position = Vector3.Lerp (Vector3.zero, finalPosition, timeElapsed / cinematicDuration);
			Vector3 eulerAngles = Vector3.Lerp (Vector3.zero, finalEulerAngles, timeElapsed / cinematicDuration);

			p_camera.transform.position = position;
			p_camera.transform.eulerAngles = eulerAngles;

			timeElapsed += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
			Debug.Log (timeElapsed + " / " + cinematicDuration);
		}

		Debug.Log ("Ended");
		GameManager.Instance.ChangeGameState (ENUM_GAMESTATE.COUNTDOWN);
	}
}
