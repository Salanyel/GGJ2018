using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MetroController : MonoBehaviour {

	[SerializeField]
	private float _metroSpeed;

	public bool _isMoving = false;
	public bool _isDoorOpened = false;
	public bool _isSomeoneInside = false;

	public GameObject _DoorBlocker;

	private float _timerForTrainDepart = 0.0f;
	private float _timerLimit;
	public float _TimerBeforeTrainLeftStation = 10.0f;

	private Animator _animator;

	public Animator animator {
		get {return _animator;}
	}

	private Vector3 _initPosition;
	public Vector3 _stationPosition;
	public Vector3 _exitPosition;

	public float _lerpPosition = 0f;

	public bool _toStation = false;
	public bool _toExit = false;

	public MetroManager _metroManager;

	void Start () {
		
		_animator = GetComponent<Animator>();
		_initPosition = transform.position;

		_timerLimit = Time.time + 10.0f;

		_DoorBlocker.SetActive(false);

	}
	
	void Update () {

		if (_toStation) {
			transform.position = Vector3.Lerp(_initPosition, _stationPosition, _lerpPosition);
		}

		if (_toExit) {
			transform.position = Vector3.Lerp(_stationPosition, _exitPosition, _lerpPosition);
		}

		if (_isDoorOpened) {

			 _timerForTrainDepart += Time.deltaTime;
			 _animator.SetFloat("DepartureTime", _timerForTrainDepart);

			 if (_timerForTrainDepart >= _timerLimit) {
				_isDoorOpened = false;
			 }
		}

	}

	public void ResetPosition () {
		transform.position = _initPosition;
		_toExit = false;
		_toStation = false;
		_lerpPosition = 0f;
		ResetTimer();
	}

	public void OnTriggerEnter (Collider _collider) {

		if (_collider.GetComponent<Player>()) {
			_animator.SetBool("IsSomeoneInside", true);
			_isSomeoneInside = true;
			_metroManager.IsOccupied(true);
		}
	}

	public void OnTriggerExit () {
		_animator.SetBool("IsSomeoneInside", false);
		_isSomeoneInside = false;
		_metroManager.IsOccupied(false);
	}

	public void StartWaitTimer () {
		ResetTimer();
		_isDoorOpened = true;
	}

	public void ResetTimer () {
		_timerForTrainDepart = 0.0f;
		_animator.SetFloat("DepartureTime", _timerForTrainDepart);
		_timerLimit = Time.time + 10.0f;
	}

	public void CloseDoors () {

		Debug.Log("DoorBlocker Active");
		_DoorBlocker.SetActive(true);
		_metroManager.InStation(false);
	}

	public void OpenDoors () {
		_DoorBlocker.SetActive(false);
		_metroManager.InStation(true);
	}

	public void ToStation() {
		ResetTimer();
		_toExit = false;
		_toStation = true;
	}

	public void ToExit() {
		ResetTimer();
		_toStation = false;
		_toExit = true;
	}
}
