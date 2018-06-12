using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour 
{
	public float speed = -0.2f;
	float originalCloudPositionY;
	float leftCutoff;

	void Start(){
		leftCutoff = (transform.parent.position != null) ? transform.parent.position.x - 11 : -11; //WILL FIX LATA
		originalCloudPositionY = transform.position.y;
		transform.position = new Vector3(transform.position.x + Random.Range(-2,2), originalCloudPositionY + Random.Range(-2,2), transform.position.z);
	}

	void Update(){		
		transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
		//if (transform.position.x <= -15)transform.position = new Vector3(15, originalCloudPositionY + Random.Range(-2,2), transform.position.z);
		if (transform.position.x <= leftCutoff){
			transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
		}
	}
}