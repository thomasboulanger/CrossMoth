using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject crowGameObject;
    [SerializeField] private NavMeshAgent mothNavmeshAgent;

    [Header("Value that can be modified")]
    [SerializeField] private float minDelayBetweenSpawns = 10f;
    [SerializeField] private float maxDelayBetweenSpawns = 15f;
    [SerializeField] private float delayBeforeFirstSpawn = 10f;
    [SerializeField] private float spawnRadiusAroundPlayer = 10;
    [SerializeField] private float minWalkableSpawnDistanceFromMoth = 8f;
    [SerializeField] private float maxWalkableSpawnDistanceFromMoth = 12f;
    
    [SerializeField] private float crowHeightAtSpawn = 0.5f;

    private float _currentTimeBeforeStart;
    private float _currentDelayBetweenAirStrike;
    private Transform _moth;
    private Vector3 _spawnPosition;
    private NavMeshPath navmeshPath;

    private void Awake() {
        navmeshPath = new NavMeshPath();
    }

    private void Start() {
        _currentTimeBeforeStart = delayBeforeFirstSpawn;
        _moth = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        float deltatime = Time.deltaTime;

        //wait a certain delay before air strike start
        if (_currentTimeBeforeStart > 0) {
            _currentTimeBeforeStart -= deltatime;
            return;
        }

        _currentDelayBetweenAirStrike -= deltatime;
        
        //check if a new crow can be spawned
        if (_currentDelayBetweenAirStrike > 0) return;
        _currentDelayBetweenAirStrike = Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);

        //get the spawn position depending on the option selected
        _spawnPosition = GetRandomCrowPosition();
        
        GameObject go = Instantiate(crowGameObject, _spawnPosition + new Vector3(0, crowHeightAtSpawn, 0), Quaternion.identity);
        go.name = "Crow";
    }
    
    private Vector3 GetRandomCrowPosition() {
        Vector3 returnPos = new Vector3(0,0,0);
        bool validPositionFound = false;
        while (!validPositionFound) {
            returnPos = _moth.position + new Vector3
            (
                Random.Range(-spawnRadiusAroundPlayer, spawnRadiusAroundPlayer),
                0,
                Random.Range(-spawnRadiusAroundPlayer, spawnRadiusAroundPlayer)
            );
            validPositionFound = CheckIfCrowPositionIsValid(returnPos);
        }
        return returnPos;
    }

    // Checks if the crow spawns near the player (ie not behind a long wall that is near but implies a long detour
    private bool CheckIfCrowPositionIsValid(Vector3 tempCrowPos) {
        if (NavMesh.CalculatePath(tempCrowPos, _moth.position, mothNavmeshAgent.areaMask, navmeshPath)) {
            // Checking if the path is complete
            if (navmeshPath.status != NavMeshPathStatus.PathComplete)
                return false;
            
            float distance = Vector3.Distance(_moth.position, navmeshPath.corners[0]);
            
            // Calculating total distance
            for (int i = 1; i < navmeshPath.corners.Length; i++) {
                distance += Vector3.Distance(navmeshPath.corners[i - 1], navmeshPath.corners[i]);
            }

            if (distance < maxWalkableSpawnDistanceFromMoth && distance > minWalkableSpawnDistanceFromMoth)
            return distance < maxWalkableSpawnDistanceFromMoth && distance > minWalkableSpawnDistanceFromMoth;
        }
        return false;
    }
}
