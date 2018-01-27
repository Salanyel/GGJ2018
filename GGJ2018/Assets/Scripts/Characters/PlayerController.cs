using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f;

    public float speedMultiplicator = 1f;

    private string _HorizontalAxis;
    private string _VerticalAxis;

    private Player _player;
    private Rigidbody _rigidbody;

    Dictionary<string, Vector3> elevatorTranslation;

    void Awake()
    {
        _player = GetComponent<Player>();

        int _playerNumber = _player.PlayerNumber;
        _HorizontalAxis = InputData._Horizontal + _playerNumber.ToString();
        _VerticalAxis = InputData._Vertical + _playerNumber.ToString();

        _rigidbody = GetComponent<Rigidbody>();
        elevatorTranslation = new Dictionary<string, Vector3>();
    }

    public void AddElevator(string _name, Vector3 force){
        if(!elevatorTranslation.ContainsKey(_name)){
            elevatorTranslation.Add(_name, force);
        }
    }

    public void RemoveElevator(string _name){
        if(elevatorTranslation.ContainsKey(_name)){
            elevatorTranslation.Remove(_name);
        }
    }

    void Update()
    {
        if (_player._allowInput){
            var x = Input.GetAxis(_HorizontalAxis) * Time.deltaTime * _playerSpeed * speedMultiplicator;
            var z = Input.GetAxis(_VerticalAxis) * Time.deltaTime * _playerSpeed * speedMultiplicator;

            Vector3 direction = new Vector3(x, 0.0f, z);

            transform.Translate(direction, Space.World);
            if(direction != Vector3.zero){
                transform.rotation = Quaternion.LookRotation(direction);
            }

            foreach(KeyValuePair<string, Vector3> el in elevatorTranslation){
                transform.Translate(el.Value * Time.deltaTime, Space.World);
            }
        }

		Debug.DrawRay(transform.position, transform.forward * 2f);
    }


}