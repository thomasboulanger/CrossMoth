using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMovement : MonoBehaviour {
    private NavMeshAgent _nav;

    [SerializeField] private Transform player;
    [SerializeField] private float maxDistance = 2f;
    private float movementUpdateDelay = 0.5f;
    private float currentMovementUpdateDelay = 0.0f;
    
    protected virtual void Start() {
        _nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update() {
        if (!player) return;
        
        currentMovementUpdateDelay += Time.deltaTime;
        if (currentMovementUpdateDelay > movementUpdateDelay) {
            currentMovementUpdateDelay = 0.0f;
            _nav.SetDestination(player.position);
        }
    }
}
