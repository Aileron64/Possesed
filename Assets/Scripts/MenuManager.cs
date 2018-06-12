using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	static MenuManager instance;

	public static MenuManager Instance{
		get{
			if (!instance){
				instance = new GameObject("MenuController").AddComponent<MenuManager>();
			}
			return instance;
		}
	}

	public void RunMenuAction(string _menuActionStr){
		switch (_menuActionStr){
			case "loadlevel1":
				LoadLevel(0);
				break;
			case "loadlevel2":
				LoadLevel(1);
				break;
		}
	}

	void LoadLevel(int _levelNum){
		MapHelper.CurrentLevelInt = _levelNum;
		Application.LoadLevel(1);
	}
}