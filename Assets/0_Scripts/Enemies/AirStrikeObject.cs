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

    private bool _inAnim;
    [SerializeField] private GameObject explosion;

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
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("destroy");
        Destroy(Instantiate(explosion, transform.position, Quaternion.identity),2f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_inAnim) return;
        _startTime += Time.deltaTime;
        
        // Calculate the interpolation factor (t) based on time
        float t = _startTime / _duration;

        // Use a curve to modify the speed as time progresses
        float speedFactor = _speedModifierCurve.Evaluate(t);

        // Interpolate between start and end points using the adjusted speed
        transform.position = Vector3.LerpUnclamped(_startPoint, _endPoint, speedFactor);
    }
}