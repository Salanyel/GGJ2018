using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotContaminedAction : PlayerActions {

	public float actionPushRange = 2f;
	public LayerMask collisionMask;

	override protected void Awake() {
		lastAction = Time.time - coolDownTime;
		coolDownTime = 1f;
	}

	override protected void DoAction(){
		RaycastHit hitInfo;
		Physics.Raycast(transform.position, transform.forward, out hitInfo, actionPushRange);
		//push that guy
		if(hitInfo.collider != null && hitInfo.collider.gameObject != gameObject){
			hitInfo.collider.GetComponent<Pusher>().Push(transform.forward);
			Debug.Log(hitInfo.collider.name);
		}else{
			Debug.Log("Coup dans le vide");
		}
	}
}
