using UnityEngine;
using System.Collections;

public class GhostBehaviour : CharacterBehaviour {

	protected void Update(){
		base.Update();
	}

	override protected void Attack(){
		if (actionState != ActionState.Shield){
			base.Attack();
			rigidbody2D.AddForce(new Vector3(transform.localScale.x, 0, 0) * (jumpForce / 2) * Time.fixedDeltaTime);
		}
	}

	override protected void Shield(){
		if (actionState != ActionState.Shield){ //if the action state hasn't been set to Shield yet
			base.Shield(); //set it from the base method
		}else{ //otherwise, if the action state is currently Shield
			actionState = ActionState.Idle; //it's no longer shielded
		}
		FadeSprite(2.0f);
		gameObject.layer = (gameObject.layer == frontLayer) ? behindLayer : frontLayer;
	}
}