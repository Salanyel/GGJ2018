using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public bool _isContamined;
	public int _playerNumber;

	public bool _allowInput = true;

	float _score;

	public float Score {
		get { return _score; }
		set { _score = value; }
	}

	public int PlayerNumber {
		get { return _playerNumber;}
		set { _playerNumber = value;}
	}

	public void SetIsContamined(bool p_isSick) {
		_isContamined = p_isSick;

		if (_isContamined) {
			gameObject.GetComponentInChildren<MeshRenderer> ().materials [1].SetFloat (ResourcesData._shaderSickChannel, 1);
			GameObject g =  Resources.Load("IllParticle") as GameObject;
			Instantiate(g, transform);
			g.transform.position = g.transform.position;
		} else {
			gameObject.GetComponentInChildren<MeshRenderer> ().materials [1].SetFloat (ResourcesData._shaderSickChannel, 0);
		}
	}

	public void SetMaterial(string p_newMaterial) {
		Material[] materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
		materials[1] = Resources.Load<Material>(p_newMaterial);
		gameObject.GetComponentInChildren<MeshRenderer> ().materials = materials;
	}
}
