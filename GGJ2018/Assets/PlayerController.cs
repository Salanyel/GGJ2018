using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {
	
	[SerializeField]
	private float _playerSpeed = 5.0f;

	private string _HorizontalAxis = "Horizontal";
	private string _VerticalAxis = "Vertical";
	
	void Start () {
		
		int _playerNumber = this.GetComponent<Player>().PlayerNumber;
		_HorizontalAxis += _playerNumber.ToString();
		_VerticalAxis += _playerNumber.ToString();
	}

	void Update () {

		var x = Input.GetAxis(_HorizontalAxis) * Time.deltaTime * _playerSpeed;
        var z = Input.GetAxis(_VerticalAxis) * Time.deltaTime * _playerSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
		
	}
}
