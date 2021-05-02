using System;
using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{
	// Update is called once per frame
	void Update()
    {
		// despawn when past left edge of the screen (camera)
		if (transform.position.x < -25)
		{
			gameObject.SetActive(false);
		}
		else
		{
			PerformPerFrameOperations();
		}
	}

    protected abstract void PerformPerFrameOperations();

}
