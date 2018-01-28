using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}
}
