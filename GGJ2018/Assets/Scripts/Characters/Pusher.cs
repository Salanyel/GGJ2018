using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {

	public float pushDistance;
	public float pushTimeLength;

	Vector3 _pushVector;
	bool _doPush = false;
	float stopPushTime;

	Player _player;

	void Awake(){
		_player = GetComponent<Player>();
	}

	public void Push(Vector3 pushVector){
		//block le joueur
		_player._allowInput = false;
		//deplace le joueur
		_pushVector = pushVector * pushDistance;
		stopPushTime = Time.time + pushTimeLength;

	}

	void Update(){
		if(_doPush && Time.time < stopPushTime){
			//deplace le joueur
			
		}else{
			_doPush = false;
		}
	}

}
