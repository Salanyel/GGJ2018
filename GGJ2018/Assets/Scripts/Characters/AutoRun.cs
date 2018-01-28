using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRun : MonoBehaviour {

	public float _moveSpeed = 1;
	float _time = 0f;

	Vector3 _position;

	void Awake() {
		_position = GetComponent<Transform>().position;
	}

	void Start() {
		GetComponentInChildren<Animator> ().SetFloat ("Speed", 0.6f);
	}

	void Update() {
		GetComponent<Transform>().Translate(new Vector3(0f, 0f, 1f) * _moveSpeed * _moveSpeed);
		_time += Time.deltaTime;

		if (_time > 5f) {
			_time = 0;
			GetComponent<Transform>().position = _position;
			GetComponentInChildren<Animator> ().transform.localPosition = Vector3.zero;
		}
	}
}
