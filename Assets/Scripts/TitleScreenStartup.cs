using UnityEngine;
using System.Collections;

public class TitleScreenStartup : MonoBehaviour {
	
	// Use this for initialization
	void Start(){
		WorldManager.Instance.ChangeTheWorld(1);
	}
}