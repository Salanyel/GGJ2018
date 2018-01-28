using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowingText : MonoBehaviour {

	float _speedAnimation = 1f;
	float _startSize = 1f;
	float _finalSize = 1.5f;
	float _currentTime = 0f;
	Text _text;
	bool _isFadeIn = false;

	void Awake() {
		_text = GetComponent<Text> ();
	}

	void Start() {
		_currentTime = 0f;
	}

	void Update() {
		float alpha;
		float scale;

		if (_currentTime <= _speedAnimation) {
			if (_isFadeIn) {
				alpha = Mathf.Lerp (0, 1, _currentTime / _speedAnimation);
				scale = Mathf.Lerp (_startSize, _finalSize, _currentTime / _speedAnimation);
			} else {
				alpha = Mathf.Lerp (1, 0, _currentTime / _speedAnimation);
				scale = Mathf.Lerp (_finalSize, _startSize, _currentTime / _speedAnimation);
			}

			_text.color = new Color (1, 1, 1, alpha);
			_text.transform.localScale = new Vector3 (scale, scale, scale);

			_currentTime += Time.deltaTime;
		} else {
			_isFadeIn = !_isFadeIn;
			_currentTime = 0;
		}
	}
}