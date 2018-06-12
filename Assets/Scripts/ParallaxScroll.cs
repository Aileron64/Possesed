using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {

	public float speed = 0.002f;
	
	// Update is called once per frame
	void Update () {
		renderer.material.mainTextureOffset = new Vector2 ((Time.time * speed)%1, 0.5f);
	}
}
