using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		// despawn when past left edge of the screen (camera)
		if (transform.position.x < -25)
		{
			Destroy(gameObject);
		}
		else
		{
			Scroll();
		}
	}

    protected abstract void Scroll();

    protected void OnTriggerEnter(Collider other)
    {
		//other.transform.parent.gameObject.GetComponent<HeliController>().Explode();
	}
}
