using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class SpiderPasGentil : MonoBehaviour
{
    private NavMeshAgent _nav;

    [SerializeField] private Transform player;
    [SerializeField] private float maxDistance = 2f;
    private float movementUpdateDelay = 0.5f;
    private float currentMovementUpdateDelay = 0.0f;

    void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        currentMovementUpdateDelay += Time.deltaTime;
        if (currentMovementUpdateDelay > movementUpdateDelay)
        {
            currentMovementUpdateDelay = 0.0f;
            _nav.SetDestination(player.position);
        }
    }
}
