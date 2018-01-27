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

	private float _timerForTrainDepart = 0.0f;
	private float _timerLimit;
	public float _TimerBeforeTrainLeftStation = 10.0f;

	private Animator _animator;

	private Vector3 _initPosition;

	void Start () {
		
		_animator = GetComponent<Animator>();
		_initPosition = transform.position;

		_timerLimit = Time.time + 10.0f;
	}
	
	void Update () {
		
		transform.Translate(Vector3.right *_metroSpeed * Time.deltaTime, Space.World);

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
	}

	public void OnTriggerEnter (Collider _collider) {
		
		if (_collider.GetComponent<Player>()) {
			_animator.SetBool("IsSomeoneInside", true);
			_isSomeoneInside = true;
		}
	}

	public void OnTriggerExit () {
		_animator.SetBool("IsSomeoneInside", false);
		_isSomeoneInside = false;
	}

	public void StartWaitTimer () {
		_timerLimit = Time.time + 10.0f;
		_isDoorOpened = true;
	}

}
