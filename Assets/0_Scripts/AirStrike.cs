using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirStrike : MonoBehaviour
{
    [SerializeField] private GameObject airStrikeGameObject;

    [Header("Value that can be modified")] 
    [SerializeField] private float spawnRadiusAroundPlayer = 5;
    [SerializeField] private float minDelayBetweenAirStrike = 3.2f;
    [SerializeField] private float maxDelayBetweenAirStrikes = 10;
    [SerializeField] private float delayBeforeAirStrikeHitGround = 3;
    [SerializeField] private AnimationCurve airStrikeSpeedCurve;
    [SerializeField] private int damageValue = 1;
    [SerializeField] private float delayBeforeFirstAirStrike = 10;
    [SerializeField] private float airStrikeObjectHeightAtSpawn = 15;
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private SpawnState airStrikeSpawnOption;

    private float _currentTimeBeforeStart;
    private float _currentDelayBetweenAirStrike;
    private Vector3 _moth;
    private Vector3 _strikePosition;
    private Camera _camera;

    private enum SpawnState
    {
        SpawnAtRandomPosition,
        SpawnInScreen,
        SpawnAroundPlayer
    }

    private void Start()
    {
        _currentTimeBeforeStart = delayBeforeFirstAirStrike;
        _moth = GameObject.FindGameObjectWithTag("Player").transform.position;
        _camera = Camera.main;
    }

    private void Update()
    {
        float deltatime = Time.deltaTime;

        //wait a certain delay before air strike start
        if (_currentTimeBeforeStart > 0)
        {
            _currentTimeBeforeStart -= deltatime;
            return;
        }

        _currentDelayBetweenAirStrike -= deltatime;
        //check if a new air strike can be launch
        if (_currentDelayBetweenAirStrike > 0) return;
        _currentDelayBetweenAirStrike = Random.Range(minDelayBetweenAirStrike, maxDelayBetweenAirStrikes);

        //get the strike position depending on the option selected
        _strikePosition = airStrikeSpawnOption switch
        {
            SpawnState.SpawnAtRandomPosition => GetRandomStrikePosition(),
            SpawnState.SpawnInScreen => GetStrikePositionInScreen(),
            SpawnState.SpawnAroundPlayer => GetStrikePositionAroundPlayer(),
            _ => throw new ArgumentOutOfRangeException()
        };
        GameObject go = Instantiate(airStrikeGameObject,
            _strikePosition + new Vector3(0, airStrikeObjectHeightAtSpawn, 0), Quaternion.identity);
        go.name = "AirStrikeObject";
        go.GetComponent<AirStrikeObject>()
            .Init(_strikePosition, delayBeforeAirStrikeHitGround, airStrikeSpeedCurve, damageValue, explosionRadius);
    }

    private Vector3 GetRandomStrikePosition()
    {
        return _moth + new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
    }

    private Vector3 GetStrikePositionInScreen()
    {
        Vector3 tmp;
        Vector3 pointInScreenSpace;
        bool isInsideScreenBounds;
        do
        {
            tmp = _moth + new Vector3
            (
                Random.Range(-50, 50),
                0,
                Random.Range(-50, 50)
            );

            pointInScreenSpace = _camera.WorldToViewportPoint(tmp);
            isInsideScreenBounds = pointInScreenSpace.x is >= 0 and <= 1 &&
                                   pointInScreenSpace.y is >= 0 and <= 1 &&
                                   pointInScreenSpace.z > 0;
        } while (!isInsideScreenBounds);

        return tmp;
    }

    private Vector3 GetStrikePositionAroundPlayer()
    {
        return _moth + new Vector3
        (
            Random.Range(-spawnRadiusAroundPlayer, spawnRadiusAroundPlayer),
            0,
            Random.Range(-spawnRadiusAroundPlayer, spawnRadiusAroundPlayer)
        );
    }
}