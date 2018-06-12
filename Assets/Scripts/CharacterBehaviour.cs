using UnityEngine;
using System.Collections;

public enum ControlState { Disabled, AIControlled, PlayerControlled }

public enum CharacterType { Normal, Ghost, Bomb }

public enum ActionState { Idle, Attack, Shield }

public class CharacterBehaviour : MonoBehaviour {

	public ControlState controlState = ControlState.PlayerControlled;
	public CharacterType charaType = CharacterType.Normal;
	protected ActionState actionState = ActionState.Idle;

	public GameObject item;

	protected Vector3 spawnPoint;
	protected Vector3 moveDirection = Vector3.zero;

	public float jumpForce = 10000.0f;
	bool hasJumped = false;

	int groundLayer;
	protected int frontLayer, behindLayer;

	protected bool isFacingRight = true;

	const float MAX_ATTACK_TIME = 1.0f;
	float attackTime = 0.0f;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
		groundLayer = LayerMask.NameToLayer("Ground");
		frontLayer = LayerMask.NameToLayer("Characters");
		behindLayer = LayerMask.NameToLayer("Characters-Behind");
	}
	
	// Update is called once per frame
	protected void Update () {
		//THIS WILL EVENTUALLY NOT BE IN UPDATE
		if (controlState == ControlState.PlayerControlled){
			HUDManager.Instance.UpdatePlayerImage(gameObject.name.Split('(')[0]);
		}
		//Checking if player reached out of bounds
		if (transform.position.y <= -100){
			Respawn();
		}
		//Checking player's direction and flipping accordingly
		if (moveDirection.x < 0 && isFacingRight){
			FlipSprite();
		}else if (moveDirection.x > 0 && !isFacingRight){
			FlipSprite();
		}
		//Checking if character is in attack state
		if (actionState == ActionState.Attack){
			if (attackTime >= MAX_ATTACK_TIME){
				actionState = ActionState.Idle;
				attackTime = 0.0f;
			}else{
				attackTime += Time.deltaTime;
			}
		}
		//Checking for control state and acting accordingly
		switch (controlState){
			case ControlState.PlayerControlled:
				PlayerInput();
				break;
			case ControlState.Disabled:

				break;
			case ControlState.AIControlled:
			//right now AI just jumps. We'll make a whole AI controller class later
				if (IsGrounded() && !hasJumped){
					Jump();
					hasJumped = true;
				}
				break;
		}
		//Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - collider2D.renderer.bounds.extents.y - 0.05f), Color.white, 0.0f, false);
	}

	void FixedUpdate(){
		transform.Translate(moveDirection * Time.deltaTime);
	}

	void PlayerInput(){
		//Checking for player input
		if (Input.GetKey(KeyCode.LeftArrow)){
			MoveHorizontal(-3.0f);
		}else if (Input.GetKey(KeyCode.RightArrow)){
			MoveHorizontal(3.0f);
		}else{
			MoveHorizontal(0.0f);
		}
		if (Input.GetKeyDown(KeyCode.Space) && ((IsGrounded() && !hasJumped) || charaType == CharacterType.Ghost)){
			Jump();
			hasJumped = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			//Checking player state
			if (charaType == CharacterType.Bomb){
				GameObject clone;
				clone = Instantiate(item, transform.position, Quaternion.identity) as GameObject;
			}
		}
		if (Input.GetKeyDown(KeyCode.A)){
			Attack();
		}
		if (Input.GetKeyDown(KeyCode.B)){
			Shield();
		}
		if (Input.GetKeyDown(KeyCode.E)){
			if (charaType != CharacterType.Ghost) LeaveCharacter(false); //ghost leave enemy, but not be disabled
		}
	}

	protected bool IsGrounded(){
		return Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - collider2D.renderer.bounds.extents.y - 0.05f), 1 << groundLayer);
	}

	protected void MoveHorizontal(float _amount){
		moveDirection.x = _amount;
	}

	virtual protected void Jump(){
		rigidbody2D.AddForce(transform.up * jumpForce * Time.fixedDeltaTime);
	}

	virtual protected void Attack(){
		actionState = ActionState.Attack;
	}

	virtual protected void Shield(){
		actionState = ActionState.Shield;
	}

	bool CheckIfBehind(Vector2 _otherForward){
		if (Vector2.Dot(new Vector3(transform.right.x * transform.localScale.x, transform.right.y, transform.right.z), _otherForward) < 0){
			return false;
		}else { return true; }
	}

	void LeaveCharacter(bool _disableAsWell){
		//Later we will check which player activated this command. For now, let's set to player 1
		GameObject ghostInControl = GhostManager.GetGhost(1);
		GhostBehaviour ghostScript = ghostInControl.GetComponent<GhostBehaviour>(); //save a reference to his script
		ghostInControl.transform.position = transform.position; //set ghost to where possessed enemy was
		ghostScript.StopMoving(); //stop character's velocity
		Camera.main.transform.parent = ghostInControl.transform; //set camera's parent back to ghost
		ghostInControl.layer = frontLayer; //send ghost to front layer if he isn't already
		if (ghostInControl.transform.localScale.x > 0 && transform.localScale.x < 0
			|| ghostInControl.transform.localScale.x < 0 && transform.localScale.x > 0){ //if player is facing the other direction from ghost
			ghostScript.FlipSprite(); //flip ghost sprite
		}
		ghostInControl.renderer.enabled = true; //set his renderer back on so he can be seen
		ghostScript.controlState = ControlState.PlayerControlled; //player can control the ghost again
		Destroy(gameObject); //ghost kills the character he possessed when he's done
		if (_disableAsWell){ //if the disable as well boolean is set to true
			ghostScript.controlState = ControlState.Disabled; //disable the ghost. this is ideal for when he reaches the goal
		}
	}

	protected void StopMoving(){
		moveDirection = Vector3.zero;
		rigidbody2D.velocity = Vector2.zero;
	}

	void FlipSprite(){
		isFacingRight = !isFacingRight;
		Vector3 flippedScale = transform.localScale;
		flippedScale.x *= -1;
		transform.localScale = flippedScale;
	}

	protected void FadeSprite(float _fadeSpeed){
		if (renderer.material.color.a == 1){renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0);}
		else{renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1);}
	}

	protected void Respawn(){
		if (controlState != ControlState.Disabled){
			transform.position = spawnPoint;
			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1);
			StopMoving();
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.layer == groundLayer){
			moveDirection.y = 0.0f;
			hasJumped = false;
	    }else if (collision.gameObject.tag == "player"){
	    	if (charaType == CharacterType.Ghost && controlState != ControlState.Disabled && actionState == ActionState.Attack){
	    		if (CheckIfBehind(new Vector3(collision.gameObject.transform.right.x * collision.gameObject.transform.localScale.x, collision.gameObject.transform.right.y, collision.gameObject.transform.right.z))){
	    			Camera.main.transform.parent = collision.gameObject.transform;
	    			Camera.main.transform.position = new Vector3(collision.gameObject.transform.position.x,collision.gameObject.transform.position.y,-10);
	    			controlState = ControlState.Disabled; //make this character unable to move
	    			StopMoving();
	    			gameObject.layer = behindLayer;
	    			renderer.enabled = false;
	    			transform.position = new Vector3(-200,-200,-200);
	    			collision.gameObject.GetComponent<CharacterBehaviour>().controlState = ControlState.PlayerControlled; //make collided character movable by player
	    		}
	    	}
	    }
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "explosion"){
			if (collider.gameObject.GetComponent<TimedObjectDestructor>().currentTime < 0.5f){ //Only the first half second of the explosion the player will be able to jump from it
				rigidbody2D.AddForce(transform.up * (jumpForce * 4) * Time.deltaTime);
			}
	    }else if (collider.gameObject.tag == "goal"){
	    	//Player made it to the goal!
	    	if (!GoalManager.Instance.CheckForGoal()){
		    	GoalManager.Instance.DisplayMessage();
		    	if (charaType != CharacterType.Ghost) LeaveCharacter(true); //ghost leave enemy, but be disabled
		    	controlState = ControlState.Disabled;
		    	StopMoving();
		    }
	    }
	}
}
