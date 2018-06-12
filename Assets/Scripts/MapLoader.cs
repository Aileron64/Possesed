using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public struct Map{
	public string name;
	public int gridWidth, gridHeight, tileWidth, tileHeight, worldIndex;
	public List<string> tileValueList;
}

public class MapLoader : MonoBehaviour {

	enum LevelReadState { None, Info, Map }

	public static Map LoadMap(int _levelNumber){
		Map mapToBeReturned = new Map();
		mapToBeReturned.tileValueList = new List<string>();
		
		LevelReadState readState = LevelReadState.None;
		try{
			using (StreamReader streamReader = new StreamReader(@"Assets/Levels/" + _levelNumber + ".poslevel")){
				string line;
				//First line will be the map name so let's just read it here before doing loop
				mapToBeReturned.name = streamReader.ReadLine();
				do{
					line = streamReader.ReadLine();
					switch (readState){
						case LevelReadState.None:
							if(line == "--INFO--"){
								readState = LevelReadState.Info;
							}
							break;
						case LevelReadState.Info:
							if (line == "--MAP--"){
								readState = LevelReadState.Map;
							}else{
								//Grabbing values for grid size and tile size
								string[] lineArr = line.Split(' ');
								mapToBeReturned.gridWidth = MapHelper.ConvertToInt(lineArr[0]);
								mapToBeReturned.gridHeight = MapHelper.ConvertToInt(lineArr[1]);
								mapToBeReturned.tileWidth = MapHelper.ConvertToInt(lineArr[2]);
								mapToBeReturned.tileHeight = MapHelper.ConvertToInt(lineArr[3]);
								mapToBeReturned.worldIndex = MapHelper.ConvertToInt(lineArr[4]);
							}
							break;
						case LevelReadState.Map:
							//only reads and adds tiles until the maximum tile count grid size allows
							if (mapToBeReturned.tileValueList.Count < (mapToBeReturned.gridWidth * mapToBeReturned.gridHeight)){
								string[] lineArr = line.Split(' ');
								for (int i = 0; i < lineArr.Length; i++){
									mapToBeReturned.tileValueList.Add(lineArr[i]);			
								}
							}else{
								//once tile reading exceeds max allotted tile count, set read state back to None
								readState = LevelReadState.None;
							}
							break;
					}
				}while(!streamReader.EndOfStream);
			}
		}catch{
			
		}

		return mapToBeReturned;
	}
}