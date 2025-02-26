using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    // shake intensity
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private float currentShakeDuration;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            //change camera position randamly
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // decrease shake duration by time
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            // reset camera position
            currentShakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    // Shake Method
    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
    }
}
