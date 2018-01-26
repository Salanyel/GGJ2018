using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public bool _isContamined;
	public int _playerNumber = 1;

	public bool IsContamined {
		get {return _isContamined;}
		set {_isContamined = value;}
	}

	public int PlayerNumber {
		get { return _playerNumber;}
		set { _playerNumber = value;}
	}
}
