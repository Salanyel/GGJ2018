using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	private float _actionRange = 8f;
	GameObject _indicator;

	bool _isCharging;
	float _chargeAmount;
	public float chargeSpeed = 2f;
	public float attackAngle = 40f;
	float _startTime;
	PlayerController _playerController;
	GameObject gabaritGO;
	Transform gabaritTransform;

	override protected void Awake() {
		lastAction = Time.time - coolDownTime;
		coolDownTime = 0f;
		
		_playerController = gameObject.GetComponent<PlayerController>();

		GameObject g =  Instantiate(Resources.Load("GabaritColdPrefab",typeof(GameObject)), transform) as GameObject;
		gabaritTransform = g.transform;

		_isCharging = false;
		_chargeAmount = 0f;
	}

	void Fire(){
		StartCoroutine(HideDisplay());

		foreach(GameObject g in SendRay()){
			GameManager.Instance.ContaminedPlayer(g);
			GameManager.Instance.UpdateScoreSickPlayer (GetComponent<Player> ().PlayerNumber - 1);
		}
	}

	//renvoie la liste des players touches
	GameObject[] SendRay(){

		List<GameObject> hitted = new List<GameObject>();
		
		for(int i=0; i < 8;i++){
			RaycastHit hitInfo;
			Vector3 direction = transform.forward;
			direction = Quaternion.AngleAxis(attackAngle * (float)i/8f - (attackAngle/2f) ,Vector3.up) * direction;

			Physics.Raycast(transform.position, direction * _actionRange * _chargeAmount, out hitInfo, _actionRange * _chargeAmount);
			
			Debug.DrawRay(transform.position, direction * _actionRange * _chargeAmount,Color.yellow,4f);
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
		_isCharging = true;
		Debug.Log("isChargin");
	}

	override protected void DoBehaviour(){
		
		if(_isCharging){
			gabaritTransform.localScale = Vector3.one * Mathf.Clamp01((Time.time - _startTime)*chargeSpeed) * (_actionRange -3f);
		}else{
			gabaritTransform.localScale = Vector3.zero;
		}
	}

	override protected void DoReleaseAction(){
		_chargeAmount = Mathf.Clamp01((Time.time - _startTime)*chargeSpeed);
		_playerController.speedMultiplicator = 1f;
		_isCharging = false;
		Fire();
		GetComponent<Pusher>().Push(-transform.forward,.2f);

	}

	IEnumerator HideDisplay() {
		yield return new WaitForSeconds(2f);
	}
}
