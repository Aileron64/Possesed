using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {
	static WorldManager instance;

	const int AMOUNT_OF_WORLDS = 2;

	Transform backCamTransform;
	GameObject background;
	Material[] worldMats;

	public static WorldManager Instance{
		get{
			if (!instance){
				GameObject gO = new GameObject("WorldManagerContainer");
				DontDestroyOnLoad(gO);
				instance = gO.AddComponent<WorldManager>();
				instance.SetUp();
			}
			return instance;
		}
	}

	public void SetUp(){
		backCamTransform = GameObject.Find("BackCamera").camera.transform;
		background = backCamTransform.Find("BackgroundQuad").gameObject;
		worldMats = new Material[AMOUNT_OF_WORLDS];
		for (int i = 0; i < AMOUNT_OF_WORLDS; i++){
			worldMats[i] = Resources.Load<Material>("Materials/World/" + i);
		}
	}

	public void ChangeTheWorld(int _worldNumber){
		if (!background){
			SetUp();
		}
		background.renderer.material = worldMats[_worldNumber];
		switch (_worldNumber){
			case 0:
				break;
			case 1:
				int cloudSpawnAmount = Random.Range(1,6);
				for (int i = 0; i < cloudSpawnAmount; i++){
					GameObject cloneCloud;
					cloneCloud = Instantiate(Resources.Load<GameObject>("Clouds/Cloud"), Vector2.zero, Quaternion.identity) as GameObject;
					cloneCloud.transform.parent = backCamTransform;
					cloneCloud.transform.localPosition = new Vector3(0, 0, 10);
				}
				GameObject moon = new GameObject("Moon");
				moon.transform.position = background.transform.position + new Vector3(background.renderer.bounds.size.x / 4, background.renderer.bounds.size.y / 4, 0);
				Sprite moonSprite = Resources.Load("Background/World_1/Moon", typeof(Sprite)) as Sprite;
				moon.AddComponent<SpriteRenderer>().sprite = moonSprite;
				moon.transform.parent = backCamTransform;
				break;
		}
	}

}