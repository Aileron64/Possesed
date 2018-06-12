using UnityEngine;
using System.Collections;

public class GUIButtonHandler : MonoBehaviour {
	GUIText btnText;
	public string buttonText;
	public string menuAction;
	Color origColor;
	Color origTextColor;

	void Start(){
		//Creating level name text that will appear on the GUI
		btnText = new GameObject("ButtonText").AddComponent<GUIText>();
		btnText.transform.parent = gameObject.transform;

		//Setting font, font size, and text for text
		btnText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		btnText.fontSize = 18;
		btnText.text = buttonText;

		//Apply positioning to text
    	btnText.pixelOffset = new Vector2(0, 0);
    	btnText.anchor = TextAnchor.MiddleCenter;
    	btnText.transform.position = new Vector3(1,1,2);

    	//Setting original colors for button and text
		origColor = guiTexture.color;
		origTextColor = btnText.color;
	}

	void OnMouseEnter(){
		ButtonHighlighted();
	}

	void OnMouseExit(){
		ButtonExit();
	}

	void OnMouseDown(){
		ButtonPressed();
	}

	void OnMouseUp(){
		ButtonReleased();
	}

	void ButtonHighlighted(){
		guiTexture.color = Color.green;
		btnText.color = Color.black;
	}

	void ButtonExit(){
		guiTexture.color = origColor;
		btnText.color = origTextColor;
	}

	void ButtonPressed(){
		guiTexture.color = Color.yellow;
		btnText.color = Color.black;
	}

	void ButtonReleased(){
		MenuManager.Instance.RunMenuAction(menuAction);
	}
}