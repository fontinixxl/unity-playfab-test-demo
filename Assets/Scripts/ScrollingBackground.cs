﻿using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

	public float scrollSpeed = .1f;
	private Renderer rend;

	void Start()
	{
		rend = GetComponent<Renderer>();
	}

	void Update()
	{
		// Time.time is time since the game began, vs. deltaTime, which is time since last frame
		float offset = Time.time * scrollSpeed;

		// texture offsets shift how the texture is drawn onto the 3D object, skewing its
		// UV coordinates; this results in a scrolling effect when applied on one axis
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}