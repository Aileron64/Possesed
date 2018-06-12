using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 1.0f;
	protected internal float currentTime = 0.0f;
	public bool detachChildren = false;

	void Update(){
		if (currentTime >= timeOut){
			currentTime = 0.0f;
			DestroyNow();
		}else currentTime += Time.deltaTime;
	}

	void DestroyNow(){
		if (detachChildren) {
			transform.DetachChildren ();
		}
		DestroyObject(gameObject);
	}

}