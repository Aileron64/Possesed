using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {
	
	static GoalManager instance;
	enum GoalState { NotInGoal, InGoal }
	GoalState goalState = GoalState.NotInGoal;

	public static GoalManager Instance{
		get{
			if (!instance) instance = (new GameObject("GoalManagerContainer")).AddComponent<GoalManager>();
			return instance;
		}
	}

	public bool CheckForGoal(){
		return (goalState == GoalState.InGoal);
	}

	public void DisplayMessage(){
		goalState = GoalState.InGoal;
		Debug.Log("GOALLLLL!");
	}
}