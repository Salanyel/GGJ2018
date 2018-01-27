﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Illness : MonoBehaviour {

	abstract public bool IsGameFinished();
	abstract public void LaunchCinematic(GameObject p_camera);
}
