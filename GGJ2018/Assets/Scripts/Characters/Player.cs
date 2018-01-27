using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public bool _isContamined;
	public int _playerNumber;
	public bool _allowInput;

	public int PlayerNumber {
		get { return _playerNumber;}
		set { _playerNumber = value;}
	}

	public void SetIsContamined(bool p_isSick, string p_newMaterial = ResourcesData._notContaminedMaterial) {
		_isContamined = p_isSick;
		gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>(p_newMaterial);
	}
}
