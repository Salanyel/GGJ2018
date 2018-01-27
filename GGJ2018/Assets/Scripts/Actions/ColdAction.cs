using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	private float _actionRange = 4f;
	GameObject _indicator;

	bool _isCharging;
	float _chargeAmount;
	public float chargeSpeed = 2f;
	public float attackAngle = 40f;
	float _startTime;
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

		foreach(GameObject g in SendRay()){
			GameManager.Instance.ContaminedPlayer(g);
		}
	}

	//renvoie la liste des players touches
	GameObject[] SendRay(){

		Debug.Log("Morve distance " + (_actionRange * _chargeAmount).ToString());

		List<GameObject> hitted = new List<GameObject>();
		for(int i=0; i < 8;i++){
			RaycastHit hitInfo;
			Vector3 direction = transform.forward;
			direction = Quaternion.AngleAxis(attackAngle * (float)i/8f - (attackAngle/2f) ,Vector3.up) * direction;

			Physics.Raycast(transform.position, direction, out hitInfo, _actionRange * _chargeAmount + 0.5f);
			
			Debug.DrawRay(transform.position, direction,Color.yellow,4f);
			if(hitInfo.collider != null && hitInfo.collider.gameObject != gameObject){
				if(!hitted.Contains(hitInfo.collider.gameObject)){
					hitted.Add(hitInfo.collider.gameObject);
				}
			}	
		}

		
		return hitted.ToArray();
	} 

	override protected void DoAction() {
		_startTime = Time.time;
		_playerController.speedMultiplicator = .5f;

	}

	override protected void DoReleaseAction(){
		_chargeAmount = Mathf.Clamp01((Time.time - _startTime)*chargeSpeed);
		_playerController.speedMultiplicator = 1f;
		Fire();
		GetComponent<Pusher>().Push(-transform.forward,.2f);

	}

	IEnumerator HideDisplay() {
		yield return new WaitForSeconds(2f);

		_indicator.SetActive(false);
	}
}
