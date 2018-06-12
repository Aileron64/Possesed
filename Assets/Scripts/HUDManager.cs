using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
	static HUDManager instance;
	GUITexture playerImage;
	GUITexture bottomRect;
	Color bottomRectColor;
	GUIText levelNameText;

	public static HUDManager Instance{
		get{
			if (!instance) instance = (new GameObject("HUDManagerContainer")).AddComponent<HUDManager>();
			return instance;
		}
	}

	public void CreateResources()
    {                                         /////////////////////////////////////////////// PLAYER ICON
		if (!playerImage)
        {
			playerImage = new GameObject("PlayerImage").AddComponent<GUITexture>();
			playerImage.pixelInset = new Rect(-64,-64,32,32);
			playerImage.transform.position = new Vector3(0.14f,1.04f,0f);
			playerImage.transform.localScale = new Vector3(0,0,1);
		}

		if (!bottomRect)
        {
			//Creating bottom rectangle object
			bottomRect = new GameObject("BottomRect").AddComponent<GUITexture>();

			//Setting dimensions
			Rect drawRect = new Rect(0,0,Screen.width,16);
			bottomRect.pixelInset = drawRect;

			//Giving the rectangle a canvas to paint on, and applying it
			Texture2D rectFill = new Texture2D(Screen.width,16);
			rectFill.SetPixel(0,0,Color.white);
			rectFill.Apply();

			//Create the new color for the rectangle
			bottomRectColor = new Color(0.37529411764f, 0.25215686274f, 0.14745098039f);

			//Apply texture, set the color, adjust the scale
			bottomRect.texture = rectFill;
			bottomRect.color = bottomRectColor;
			bottomRect.transform.localScale = new Vector3(0,0,1);
		}
		if (!levelNameText){
			//Creating level name text that will appear on the GUI
			levelNameText = new GameObject("LevelNameText").AddComponent<GUIText>();

			//Setting font for text
			levelNameText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
 
 			//Apply positioning to text
        	levelNameText.pixelOffset = new Vector2(bottomRect.pixelInset.x, bottomRect.pixelInset.y);
        	levelNameText.anchor = TextAnchor.LowerLeft;
        	levelNameText.transform.position = new Vector3(0,0,2);
		}
	}

	public void UpdatePlayerImage(string _characterName){
		playerImage.texture = Resources.Load<Texture2D>("GUI/Player_Icons/Icon_" + _characterName);
	}

	public void UpdateHUD(){
		
	}

	public void SetBottomText(string _text){
		levelNameText.text = _text;
	}
}