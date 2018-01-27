﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f;

    private string _HorizontalAxis;
    private string _VerticalAxis;

    private Player _player;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _player = GetComponent<Player>();

        int _playerNumber = _player.PlayerNumber;
        _HorizontalAxis = InputData._Horizontal + _playerNumber.ToString();
        _VerticalAxis = InputData._Vertical + _playerNumber.ToString();

        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_player._allowInput){
            var x = Input.GetAxis(_HorizontalAxis) * Time.deltaTime * _playerSpeed;
            var z = Input.GetAxis(_VerticalAxis) * Time.deltaTime * _playerSpeed;

            Vector3 direction = new Vector3(x, 0.0f, z);

            transform.Translate(direction, Space.World);
            if(direction != Vector3.zero){
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

		Debug.DrawRay(transform.position, transform.forward * 2f);
    }


}