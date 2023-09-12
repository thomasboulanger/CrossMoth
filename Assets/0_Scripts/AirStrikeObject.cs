using System;
using System.Data.Common;
using UnityEngine;

public class AirStrikeObject : MonoBehaviour
{
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private float _journeyLength;
    private float _startTime;
    private float _duration;
    private AnimationCurve _speedModifierCurve;
    private int _damageValue;
    private float _explosionRadius;
    
    public void Init(Vector3 endPoint, float duration, AnimationCurve speedModifierCurve,int damageValue,float explosionRadius)
    {
        _startPoint = transform.position;
        _endPoint = endPoint;
        _duration = duration;
        _speedModifierCurve = speedModifierCurve;
        _damageValue = damageValue;
        _explosionRadius = explosionRadius;
    }
    
    private void Update()
    {
        // Calculate the elapsed time since the start
        float currentTime = Time.time - _startTime;

        // Ensure we don't go beyond the duration
        if (currentTime < _duration)
        {
            // Calculate the interpolation factor (t) based on time
            float t = currentTime / _duration;

            // Use a curve to modify the speed as time progresses
            float speedFactor = _speedModifierCurve.Evaluate(t);

            // Interpolate between start and end points using the adjusted speed
            transform.position = Vector3.Lerp(_startPoint, _endPoint, speedFactor);
        }
        else
        {
            // Ensure we reach the end point precisely
            transform.position = _endPoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        
        foreach (Collider element in colliders)
        {
            if (element.transform.CompareTag("Player"))
            {
                //apply damages here 
                break;
            }
        }
        Destroy(gameObject);
    }
}
