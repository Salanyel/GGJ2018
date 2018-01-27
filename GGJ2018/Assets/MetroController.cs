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

	private Vector3 _initPosition;

	// Use this for initialization
	void Start () {
		_initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate(Vector3.right *_metroSpeed * Time.deltaTime, Space.World);
	}

	public void ResetPosition () {
		transform.position = _initPosition;
	}

}
