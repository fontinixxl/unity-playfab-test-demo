using System.Collections;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

	public float scrollSpeed = .1f;
	private Renderer rend;
    private Coroutine scrollBackground;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        GameManager.StartGameEvent += StartSpawning;
        GameManager.GameOverEvent += StopSpawning;
    }

    private void StartSpawning()
    {
        scrollBackground = StartCoroutine(ScrollBackground());
    }

    private void StopSpawning()
    {
        StopCoroutine(scrollBackground);
    }

    private IEnumerator ScrollBackground()
    {
        while (true)
        {
			// Time.time is time since the game began, vs. deltaTime, which is time since last frame
			float offset = Time.time * scrollSpeed;

			// texture offsets shift how the texture is drawn onto the 3D object, skewing its
			// UV coordinates; this results in a scrolling effect when applied on one axis
			rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
			yield return null;
        }
	}
}
