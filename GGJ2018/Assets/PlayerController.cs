using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	[SerializeField]
	private float _playerSpeed = 5.0f;

	private int _playerNumber;

	public int PlayerNumber {
		get { return _playerNumber;}
		set { _playerNumber = value;}
	}

	private string _HorizontalAxis = "Horizontal";
	private string _VerticalAxis = "Vertical";
	
	void InitPlayer () {
		_HorizontalAxis += _playerNumber.ToString();
	}

	void Update () {

		var x = Input.GetAxis(_HorizontalAxis) * Time.deltaTime * _playerSpeed;
        var z = Input.GetAxis(_VerticalAxis) * Time.deltaTime * _playerSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
		
	}
}
