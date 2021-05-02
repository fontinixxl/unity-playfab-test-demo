using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    public static event Action OnCoinPickedUp;
    public static event Action OnCollision;

	public float speed = 10.0f;
    private Rigidbody rb;
	private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        velocity = Vector3.zero;
		velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");

        if (velocity == Vector3.zero)
            return;

        // constrain movement within the bounds of the camera
        ConstrainMovementWithinBounds();
	}

    private void FixedUpdate()
    {
        rb.velocity = velocity * speed;
    }

    // TODO: Unused, but I may use it later
    public void PickupCoin()
    {
        OnCoinPickedUp?.Invoke();
        // TODO: trigger audio playback and emit particles from particle system
    }

    private void ConstrainMovementWithinBounds()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.08f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.08f, 0.93f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collidable"))
        {
            OnCollision?.Invoke();
            // TODO:
            // Trigger Game Over (Subscibing to OnColliderHelicopter)
            // Play SoundFx
            // Play Particles
            Destroy(gameObject);

        }
    }
}
