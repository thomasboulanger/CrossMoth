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

    public void Init(Vector3 endPoint, float duration, AnimationCurve speedModifierCurve, int damageValue,
        float explosionRadius)
    {
        _startPoint = transform.position;
        _endPoint = endPoint;
        _duration = duration;
        _speedModifierCurve = speedModifierCurve;
        _damageValue = damageValue;
        _explosionRadius = explosionRadius;
        Destroy(gameObject, duration);
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

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider element in colliders)
        {
            if (!element.transform.CompareTag("Player")) continue;
            element.GetComponent<MothHealth>().TakeDamage(_damageValue);
        }
    }
}