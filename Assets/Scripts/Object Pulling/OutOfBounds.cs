using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Deactivate any gameobject that goes out of bounds
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
