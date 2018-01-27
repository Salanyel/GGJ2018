using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Pusher : MonoBehaviour {

	public float pushDistance = 4f;
	
	Vector3 _pushVector;
	bool _doPush = false;
	bool _vibrate = false;
	float stopPushTime;

	Player _player;
	PlayerIndex _playerIndex;

	void Awake(){
		_player = GetComponent<Player>();

		_playerIndex = (PlayerIndex)_player.PlayerNumber;
	}

	public void Push(Vector3 pushVector, float _length = .5f){
		//block le joueur
		_player._allowInput = false;
		//deplace le joueur
		_pushVector = pushVector * pushDistance;
		_doPush = true;
		stopPushTime = Time.time + _length;

	}

	void Update(){
		if(_doPush && Time.time < stopPushTime){
			//deplace le joueur
			transform.Translate(_pushVector * Time.deltaTime, Space.World);
			_vibrate = true;
		}else if (_doPush && Time.time >= stopPushTime && GameManager.Instance.GameState == ENUM_GAMESTATE.PLAYING) {
			_doPush = false;
			_player._allowInput = true;
			_vibrate = false;
		}
	}

	void FixedUpdate()
    {
		if (_vibrate) {
			 // SetVibration should be sent in a slower rate.
			 // Set vibration according to triggers
			GamePad.SetVibration(_playerIndex, 2.0f, 2.0f);
		} else {
			GamePad.SetVibration(_playerIndex, 0f, 0f);
		}

    }
}
