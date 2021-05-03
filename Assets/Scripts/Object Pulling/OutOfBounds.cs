using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Despawn game objects that collide with the sensor
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
