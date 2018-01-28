using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {

	public float pushDistance = 4f;
	
	Vector3 _pushVector;
	bool _doPush = false;
	float stopPushTime;

	Player _player;

	ParticleSystem _ps;

	void Awake(){
		_player = GetComponent<Player>();
		GameObject particles = Resources.Load("HitBackParticles") as GameObject;
		_ps = Instantiate(particles, transform).GetComponent<ParticleSystem>();
	}

	public void Push(Vector3 pushVector, float _length = .5f){
		//block le joueur
		_player._allowInput = false;
		//deplace le joueur
		_pushVector = pushVector * pushDistance;
		_doPush = true;
		stopPushTime = Time.time + _length;
		_ps.Play();

	}

	void Update(){
		if(_doPush && Time.time < stopPushTime){
			//deplace le joueur
			transform.Translate(_pushVector * Time.deltaTime, Space.World);
		}else if (_doPush && Time.time >= stopPushTime && GameManager.Instance.GameState == ENUM_GAMESTATE.PLAYING) {
			_doPush = false;
			_player._allowInput = true;
		}
	}

}
