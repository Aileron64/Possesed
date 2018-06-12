using UnityEngine;
using System.Collections;

public class MapHelper : MonoBehaviour {

	public static int CurrentLevelInt = 0;

	public static int ConvertToInt(string _value){
		int value;
		if (!int.TryParse(_value, out value)){value = 0;}
		return value;
	}

	public static Vector2 ConvertToMapPoint(Map _map, int _tileIndex){
		Vector2 point = Vector2.zero;
		point.x = (_tileIndex - (_map.gridWidth * Mathf.CeilToInt(_tileIndex / _map.gridWidth)));
		point.y = -Mathf.CeilToInt(_tileIndex / _map.gridWidth);
		point.x = (point.x * _map.tileWidth + (_map.tileWidth / 2)) / 100;
		point.y = (point.y * _map.tileHeight + (_map.tileHeight / 2)) / 100;
		return point;
	}

}