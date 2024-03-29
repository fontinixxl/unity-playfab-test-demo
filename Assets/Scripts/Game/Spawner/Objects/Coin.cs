﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 200f;

    private void Update()
    {
        transform.Translate(-Spawner.Speed * Time.deltaTime, 0, 0, Space.World);

        // infinitely rotate this coin about the Y axis in world space
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

}
