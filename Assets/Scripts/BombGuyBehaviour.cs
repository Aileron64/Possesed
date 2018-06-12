using UnityEngine;
using System.Collections;

public class BombGuyBehaviour : CharacterBehaviour {
	override protected void Attack(){
		GameObject cloneItem = Instantiate(item, transform.position, Quaternion.identity) as GameObject;
		cloneItem.rigidbody2D.velocity = new Vector2(4 * transform.localScale.x, 4);
	}
}