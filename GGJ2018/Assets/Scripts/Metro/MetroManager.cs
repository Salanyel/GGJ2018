using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class handles a train with multiple cars
public class MetroManager : MonoBehaviour {

	public MetroController[] _carList;
	
	public bool _isOccupied = false;

	// Use this for initialization
	void Awake () {
		
		foreach (MetroController mc in _carList) {
			mc._metroManager = this;	
		}
	}
	
	public void IsOccupied (bool p_occupied) {

		foreach (MetroController mc in _carList) {
			mc._isSomeoneInside	= p_occupied;
			mc.animator.SetBool("IsSomeoneInside", p_occupied);
		}

	}

}
