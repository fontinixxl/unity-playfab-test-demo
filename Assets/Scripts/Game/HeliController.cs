using System;
using System.Collections;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    public static event Action OnCollision;

    public float speed = 10.0f;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private Rigidbody rb;
	private Vector3 velocity;
    private bool isGameOver;

    void Start()
    {
        isGameOver = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isGameOver) return;

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

    private void ConstrainMovementWithinBounds()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.08f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.08f, 0.93f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Collidable"))
        {
            isGameOver = true;
            StartCoroutine(Explode());
        }
        else if (other.CompareTag("Coin"))
        {
            fireworksParticle.Play();
            SoundManager.Instance.PlaySoundEffect(SoundEffect.PickUpCoin);
            GameManager.Instance.PickUpCoins();
            other.gameObject.SetActive(false);
        }
    }

    private IEnumerator Explode()
    {
        // Play particles and SoundFX
        explosionParticle.Play();
        SoundManager.Instance.PlaySoundEffect(SoundEffect.Explosion);
        yield return new WaitForSeconds(0.3f);

        // Hide the helicopter after 0.3 sec
        foreach (var childMesh in GetComponentsInChildren<MeshRenderer>())
            childMesh.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        // Invoke Game Over and destroy gameobject after 2 sec
        OnCollision?.Invoke();
        Destroy(gameObject);
    }
}
