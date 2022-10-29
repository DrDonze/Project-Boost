using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementDirection = Vector3.right;
    [SerializeField, Tooltip("Time[s] it takes for a full cycle.")] float oscillationPeriod = 2f;

    Vector3 initialPosition;
    float movementFactor = 0;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (oscillationPeriod <= Mathf.Epsilon) return;

        ProcessOscillation();
    }

    void ProcessOscillation()
    {
        float cycles = Time.time / oscillationPeriod;

        float rawSinWave = Mathf.Sin(cycles * (Mathf.PI * 2));

        movementFactor = rawSinWave; // If you don't want to go in negative values, take this instead => movementFactor = (rawSinWave + 1f) / 2f

        Vector3 offset = movementDirection * movementFactor;
        transform.position = initialPosition + offset;
    }
}