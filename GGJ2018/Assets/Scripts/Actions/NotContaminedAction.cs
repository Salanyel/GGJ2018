using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotContaminedAction : PlayerActions {

	public float coolDownTime = 1f;
	float lastAction;
	public float actionPushRange = 1f;
	public LayerMask collisionMask;

	override protected void Awake() {
		_actionKey = InputData._Action;
		lastAction = Time.time - coolDownTime;
	}

	void DoPushAction(){
		RaycastHit hitInfo;
		Physics.Raycast(transform.position, transform.forward, out hitInfo, actionPushRange, collisionMask);
		//push that guy
		hitInfo.collider.GetComponent<Pusher>().Push(transform.forward);

	}

	override public void Action() {
		//verifie le cooldown
		if(Time.time >= lastAction + coolDownTime){
			lastAction = Time.time;

			DoPushAction();
		}
	}
}
