using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapGeneration : MonoBehaviour {

	protected internal Map map;

	public int worldNumber = 1;
	public int levelNumber;
	public string levelName;

	public bool isEnabled;

	Camera backgroundCamera;

	Sprite[] tileSpriteArr;
	GameObject[] guyArr;
	string[] guyNameArr;


	// Use this for initialization
	void Start () {
		//LOADING DEPENDICIES
		backgroundCamera = GameObject.Find("BackCamera").camera;
		//LOADING MAP
		if (isEnabled){
			levelNumber = MapHelper.CurrentLevelInt;
			if (CheckIfLevelExists(levelNumber)){
				//Deleting all game objects except camera and MapGenerator
				Camera.main.transform.parent = null; //making main camera parent null if its attached/child to any object
				GameObject[] gameObjectArr = FindObjectsOfType(typeof(GameObject)) as GameObject[];
				for (int i = 0; i < gameObjectArr.Length; i++){
					if (gameObjectArr[i].tag != "camera" && 
						gameObjectArr[i].tag != "MainCamera" && 
						gameObjectArr[i].tag != "background" && 
						gameObjectArr[i].tag != "manager" &&
						gameObjectArr[i].tag != "guiElement"){
						Destroy(gameObjectArr[i]);
					}
				}
				//Getting map ready for loading
				tileSpriteArr = GetTileSprites(worldNumber);
				guyArr = GetGuys();
				guyNameArr = GetGuyNames();
				map = MapLoader.LoadMap(levelNumber);
				GenerateMap(map);
				WorldManager.Instance.ChangeTheWorld(map.worldIndex);
			}else{
				EmulateLevel();
			}
		}else{
			EmulateLevel();
		}
		//Creating HUD resources
		HUDManager.Instance.CreateResources();
		HUDManager.Instance.SetBottomText(map.name);
	}

	bool CheckIfLevelExists(int _levelNumber){
		return System.IO.File.Exists(@"Assets/Levels/" + _levelNumber + ".poslevel");
	}

	void EmulateLevel(){
		//I guess some of us don't like the tile generator. Fine, here's a way to get around it.
		//Emulate map with values given by Unity Editor
		map.name = levelName;
	}

	void GenerateMap(Map _mapToBeGenerated){
		//Now taking tile data and adding appropriate objects to screen
		int numCheck;
		for (int i = 0; i < _mapToBeGenerated.tileValueList.Count; i++){
			Vector2 objectSpawnPoint = MapHelper.ConvertToMapPoint(_mapToBeGenerated, i);
			if (int.TryParse(_mapToBeGenerated.tileValueList[i], out numCheck)){ //if tile value is a number
				if (numCheck != 0){
					//Loading generic block
					GameObject tile;
					tile = Instantiate(Resources.Load("Tiles/Block"), new Vector3(objectSpawnPoint.x, objectSpawnPoint.y, 0), Quaternion.identity) as GameObject;
					//Adjusting collider
					tile.GetComponent<BoxCollider2D>().size = new Vector2((float)_mapToBeGenerated.tileWidth / 100, (float)_mapToBeGenerated.tileHeight / 100);
					//Applying appropriate sprite to it
					tile.GetComponent<SpriteRenderer>().sprite = tileSpriteArr[MapHelper.ConvertToInt(_mapToBeGenerated.tileValueList[i])];
					//Additional settings for each block
					switch (_mapToBeGenerated.tileValueList[i]){
						case "2":
							tile.tag = "bombblock";
							break;
						case "3":
							tile.layer = LayerMask.NameToLayer("Ground-2");
							break;
					}
				}
			}else{ //if a tile value is any other value besides a number
				if (_mapToBeGenerated.tileValueList[i] != "G"){
					GameObject spawnGuy;
					//Suppose we have the value "P1".
					//"characterValue" holds the first character of this string value, and it denotes the type of
					//character it is. In this case, 'P' signifies Ghost.
					//"otherValue" hold the second character of the string value, and it denotes any other info
					//the character needs to have. In this example, the '1' in "P1" signifies this ghost is Player One.
					string characterValue = _mapToBeGenerated.tileValueList[i][0].ToString();
					string otherValue = (_mapToBeGenerated.tileValueList[i].Length > 1) ? _mapToBeGenerated.tileValueList[i][1].ToString() : "";
					spawnGuy = Instantiate(guyArr[System.Array.IndexOf(guyNameArr, characterValue)], new Vector3(objectSpawnPoint.x, objectSpawnPoint.y, 0), Quaternion.identity) as GameObject;
					if (characterValue == "P"){
						GhostManager.SetGhost(ref spawnGuy, MapHelper.ConvertToInt(otherValue));
						Camera.main.transform.parent = spawnGuy.transform;
						Camera.main.transform.position = new Vector3(spawnGuy.transform.position.x,spawnGuy.transform.position.y,-10);
					}
				}else{
					GameObject goalObject;
					goalObject = Instantiate(Resources.Load<GameObject>("Objects/goal/Goal"), new Vector3(objectSpawnPoint.x, objectSpawnPoint.y, 0), Quaternion.identity) as GameObject;
				}
			}
		}
	}

	Sprite[] GetTileSprites(int _worldNumber){
		Sprite[] spriteArr = Resources.LoadAll<Sprite>("Tiles/Images/World_" + _worldNumber);
		return spriteArr;
	}

	GameObject[] GetGuys(){
		GameObject[] guyArr = Resources.LoadAll<GameObject>("Guys");
		return guyArr;
	}

	string[] GetGuyNames(){
		string[] guyNameArr = new string[guyArr.Length];
		for (int i = 0; i < guyNameArr.Length; i++){
			guyNameArr[i] = guyArr[i].name;
		}
		return guyNameArr;
	}
}
