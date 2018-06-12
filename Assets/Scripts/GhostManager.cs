using UnityEngine;
using System.Collections;

public class GhostManager : MonoBehaviour {
	public static GameObject[] p = new GameObject[4];

	public static GameObject GetGhost(int _playerNum){
		if (p[_playerNum - 1]){
			return p[_playerNum - 1];
		}else{
			Debug.Log("There is no ghost/player attached to this object. Please attach one then try again.");
		}
		return null;
	}
	public static void SetGhost(ref GameObject _ghost, int _playerNum){
		p[_playerNum - 1] = _ghost;
	}
}