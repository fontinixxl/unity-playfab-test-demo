using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    private Rigidbody rb;
	public float speed = 10.0f;
	private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        velocity = Vector3.zero;
		velocity.x = Input.GetAxis("Horizontal") * speed;
        velocity.y = Input.GetAxis("Vertical") * speed;

        if (velocity != Vector3.zero)
        {
            // constrain movement within the bounds of the camera
            ConstrainMovementWithinBounds();
        }
	}

    private void FixedUpdate()
    {
        const float Epsilon = 0.005f;

        // Don't perform any work if no movement is required.
        if (velocity.sqrMagnitude <= Epsilon)
            return;

        rb.velocity = velocity;
    }

    private void ConstrainMovementWithinBounds()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.08f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.08f, 0.93f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

}
