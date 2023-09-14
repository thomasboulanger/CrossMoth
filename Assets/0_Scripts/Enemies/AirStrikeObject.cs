using System;
using System.Collections;
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
    private GameObject _player;

    private void Start() => _player = GameObject.FindWithTag("Player");

    public void Init(Vector3 endPoint, float duration, AnimationCurve speedModifierCurve, int damageValue,
        float explosionRadius)
    {
        _startPoint = transform.position;
        _endPoint = endPoint;
        _duration = duration;
        _speedModifierCurve = speedModifierCurve;
        _damageValue = damageValue;
        _explosionRadius = explosionRadius;
        
        StartCoroutine(DeadEnd());
    }

    IEnumerator DeadEnd()
    {
        yield return new WaitForSeconds(_duration);
        if(Vector3.Distance(transform.position,_player.transform.position) < _explosionRadius) _player.GetComponent<MothHealth>().TakeDamage(_damageValue);
        Destroy(gameObject);
    }

    private void Update()
    {
        _startTime += Time.deltaTime;
        // Calculate the interpolation factor (t) based on time
        float t = _startTime / _duration;

        // Use a curve to modify the speed as time progresses
        float speedFactor = _speedModifierCurve.Evaluate(t);

        // Interpolate between start and end points using the adjusted speed
        transform.position = Vector3.Lerp(_startPoint, _endPoint, speedFactor);
    }
}