using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {

	public float pushDistance = 4f;
	
	Vector3 _pushVector;
	bool _doPush = false;
	float stopPushTime;

	Player _player;

	void Awake(){
		_player = GetComponent<Player>();
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
			Debug.Log("boooo, im pushed");
			transform.Translate(_pushVector * Time.deltaTime, Space.World);
		}else if (_doPush && Time.time >= stopPushTime) {
			_doPush = false;
			_player._allowInput = true;
		}
	}

}
