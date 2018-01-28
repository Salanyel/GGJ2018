using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class morveattackTest : MonoBehaviour {

	public ParticleSystem _ps;

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			_ps.Play();
		}
	}
}
