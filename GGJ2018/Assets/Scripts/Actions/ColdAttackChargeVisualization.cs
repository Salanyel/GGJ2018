using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAttackChargeVisualization : MonoBehaviour {

	SpriteRenderer gabaritSprite;
	public float maxScale = 4f;

	void Awake(){
		gabaritSprite = GetComponent<SpriteRenderer>();
	}

	void Update(){
		gabaritSprite.transform.localScale = Vector3.one * Mathf.Abs(Mathf.Cos(Time.time)*2f) * maxScale;
	}
}
