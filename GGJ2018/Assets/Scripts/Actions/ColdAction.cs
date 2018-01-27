using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	private float _actionRange = 2f;
	GameObject _indicator;

	bool _isCharging;
	float _chargeAmount;
	public float chargeSpeed = 2f;

	PlayerController _playerController;

	override protected void Awake() {
		lastAction = Time.time - coolDownTime;
		coolDownTime = 0f;
		
		_indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Destroy(_indicator.GetComponent<BoxCollider>());
		_indicator.transform.SetParent(gameObject.transform);
		_indicator.transform.localPosition = new Vector3(0f, 0f, _actionRange - 0.5f);
		_indicator.transform.localScale = new Vector3(0.1f, 0.1f, _actionRange);
		_indicator.SetActive(false);

		_playerController = gameObject.GetComponent<PlayerController>();

		_isCharging = false;
		_chargeAmount = 0f;
	}

	void Fire(){
		_indicator.SetActive(true);
		StartCoroutine(HideDisplay());

		RaycastHit hitInfo;
		Physics.Raycast(transform.position, transform.forward, out hitInfo, _actionRange + 0.5f);
		
		if(hitInfo.collider != null && hitInfo.collider.gameObject != gameObject){
			GameManager.Instance.ContaminedPlayer(hitInfo.collider.gameObject);
		}
	}

	override protected void DoAction() {
		_chargeAmount += chargeSpeed * Time.deltaTime;
		_chargeAmount = Mathf.Clamp01(_chargeAmount);
		_playerController.speedMultiplicator = .5f;

	}

	override protected void DoReleaseAction(){
		_chargeAmount = 0f;
		_playerController.speedMultiplicator = 1f;
		Fire();
		GetComponent<Pusher>().Push(-transform.forward,.2f);
	}

	IEnumerator HideDisplay() {
		yield return new WaitForSeconds(2f);

		_indicator.SetActive(false);
	}
}
