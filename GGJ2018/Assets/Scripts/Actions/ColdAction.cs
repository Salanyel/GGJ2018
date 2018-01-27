using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	private float _actionRange = 2f;
	GameObject _indicator;

	override protected void Awake() {
		lastAction = Time.time - coolDownTime;
		coolDownTime = 2f;
		
		_indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Destroy(_indicator.GetComponent<BoxCollider>());
		_indicator.transform.SetParent(gameObject.transform);
		_indicator.transform.localPosition = new Vector3(0f, 0f, _actionRange - 0.5f);
		_indicator.transform.localScale = new Vector3(0.1f, 0.1f, _actionRange);
		_indicator.SetActive(false);
	}

	override protected void DoAction() {
		_indicator.SetActive(true);
		StartCoroutine(HideDisplay());

		RaycastHit hitInfo;
		Physics.Raycast(transform.position, transform.forward, out hitInfo, _actionRange + 0.5f);
		
		if(hitInfo.collider != null && hitInfo.collider.gameObject != gameObject){
			GameManager.Instance.ContaminedPlayer(hitInfo.collider.gameObject);
		}
	}

	IEnumerator HideDisplay() {
		yield return new WaitForSeconds(2f);

		_indicator.SetActive(false);
	}
}
