using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAction : PlayerActions {

	private float _actionRange = 8f;

	bool _isCharging;
	float _chargeAmount;
	public float chargeSpeed = 2f;
	public float attackAngle = 40f;
	float _startTime;
	PlayerController _playerController;
	GameObject gabaritGO;
	Transform gabaritTransform;

	ParticleSystem _morveAttack;
	Animator _animator;

	override protected void Awake() {
		lastAction = Time.time - coolDownTime;
		coolDownTime = 0f;
		
		_playerController = gameObject.GetComponent<PlayerController>();

		GameObject g =  Instantiate(Resources.Load("GabaritColdPrefab",typeof(GameObject)), transform) as GameObject;
		gabaritTransform = g.transform;

		_isCharging = false;
		_chargeAmount = 0f;

		GameObject particleObject = Resources.Load("MorveParticle") as GameObject;
		_morveAttack = particleObject.GetComponent<ParticleSystem>();

		_animator = GetComponentInChildren<Animator>();
		coolDownTime = 1f;
		
	}

	void Fire(){
		_morveAttack.Play();
		_animator.SetTrigger("Atchoum");

		GetComponent<Player> ().PlaySound (GetComponent<Player> ()._contamination);

		foreach(GameObject g in SendRay()){
			GameManager.Instance.ContaminedPlayer(g);
			GameManager.Instance.UpdateScoreSickPlayer (GetComponent<Player> ().PlayerNumber - 1);
		}
	}

	//renvoie la liste des players touches
	GameObject[] SendRay(){

		List<GameObject> hitted = new List<GameObject>();

		LayerMask collisionMask = LayerMask.GetMask("Props","Players");

		for(int i=0; i < 8;i++){
			RaycastHit hitInfo;
			Vector3 direction = transform.forward;
			direction = Quaternion.AngleAxis(attackAngle * (float)i/8f - (attackAngle/2f) ,Vector3.up) * direction;

			Vector3 position = transform.position;
			position.y += 0.15f;

			Physics.Raycast(position, direction * _actionRange * _chargeAmount, out hitInfo, _actionRange * _chargeAmount,collisionMask);
			
			Debug.DrawRay(transform.position, direction * _actionRange * _chargeAmount,Color.yellow,4f);

			if(hitInfo.collider != null && hitInfo.collider.gameObject != gameObject){
				Debug.Log(hitInfo.collider.CompareTag(Tags._players));
				if(!hitted.Contains(hitInfo.collider.gameObject)){

					if(hitInfo.collider.gameObject.CompareTag(Tags._players)){
						
						hitted.Add(hitInfo.collider.gameObject);
					}else{
						Rigidbody rigidbody = hitInfo.collider.GetComponent<Rigidbody>();
						if (rigidbody != null) { 
							float range = .2f;
							Vector3 randv = new Vector3(
							Random.Range(-range, range),
							Random.Range(-range, range),
							Random.Range(-range, range)
							);
							rigidbody.AddForce((transform.forward + randv) * 7f, ForceMode.Impulse);
						}
					}
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
			_animator.SetBool("Charging",true);
		}else{
			gabaritTransform.localScale = Vector3.zero;
			_animator.SetBool("Charging",false);
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
