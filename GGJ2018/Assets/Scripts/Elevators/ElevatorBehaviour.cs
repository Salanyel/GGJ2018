using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ElevatorBehaviour : MonoBehaviour {

	public float speed;
	public Vector3 direction;

	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + direction);
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag(Tags._players)){
			other.GetComponent<PlayerController>().AddElevator(gameObject.name, direction * speed);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.CompareTag(Tags._players)){
			other.GetComponent<PlayerController>().RemoveElevator(gameObject.name);
		}
	}
}
