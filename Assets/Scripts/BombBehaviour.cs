using UnityEngine;
using System.Collections;

public class BombBehaviour : MonoBehaviour {

	enum BombState { Normal, Exploded }

	BombState bombState = BombState.Normal;
	public Transform explosion;
	float bombTimer;
	const float MAX_BOMB_TIME = 3.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (bombTimer >= MAX_BOMB_TIME){
			bombTimer = 0.0f;
			CreateExplosion();
			Destroy(gameObject);
		}else{
			bombTimer += Time.deltaTime;
		}
	}

	void CreateExplosion(){
		bombState = BombState.Exploded;
		GameObject clone;
		clone = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		ExplodeAnythingNearBomb();
	}

	void ExplodeAnythingNearBomb(){
		float explodeRadius = 1.0f;
		Collider2D[] collidersinRangeArr = Physics2D.OverlapCircleAll(transform.position, explodeRadius, 1 << LayerMask.NameToLayer("Ground"), -Mathf.Infinity, Mathf.Infinity);
		foreach (Collider2D col in collidersinRangeArr){
			if (col.gameObject.tag == "bombblock")Destroy(col.gameObject, 0.2f);
		}
	}
}
