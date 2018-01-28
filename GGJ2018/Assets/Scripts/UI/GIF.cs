using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour {

	public Sprite[] _sprites;

	float _fps = 30f;
	float _time = 0f;
	public bool _isAnimated = false;
	int _index = 0;
	Image _image;
	bool _isInverted = false;

	void Awake() {
		_image = GetComponent<Image> ();
		_image.sprite = _sprites [_index];
	}

	public void LaunchGif() {
		_index = 0;
		_isAnimated = true;
		_isInverted = false;
		_time = 1f;
	}

	public void ReverseGIF() {
		_index = _sprites.Length - 1;
		_time = 1f;
		_isInverted = true;
	}

	void Update() {
		if (_time > 1 / 30) {
			if (!_isInverted) {
				_index++;
			} else {
				_index--;
			}

			if (_index < _sprites.Length && _index > 0) {
				_image.sprite = _sprites [_index];
			}

			_time -= 1 / 30;
		}

		_time += Time.deltaTime;
		
	}
}
