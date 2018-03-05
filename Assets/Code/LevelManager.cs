using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    private List<Checkpoint> _checkpoints;
    private int _currentCheckpointIndex;
    private DateTime _started;
    private int _savedPoints;

    public Checkpoint DebugSpawn;

    public void Awake()
    {
        GameManager.Instance.CheckCurrentLevel();
        _savedPoints = GameManager.Instance.Points;
        Instance = this;
        Input.multiTouchEnabled = true;
    }

    public void Start()
    {
        
        GameManager.Instance.Reset();
        _checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
        _currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

        _started = DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
        foreach (var listener in listeners)
        {
            for (var i = _checkpoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;
                if (distance < 0)
                    continue;

                _checkpoints[i].AssignObjectToCheckpoint(listener);
                break;
            }
        }

#if UNITY_EDITOR
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(Player);
        else if(_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
    #else
        if(_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
    #endif 
               
    }

    public void Update()
    {
        var isAtLastCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;
        if (isAtLastCheckpoint)
            return;

        var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        _checkpoints[_currentCheckpointIndex].PlayerLeftCheckPoint();
        _currentCheckpointIndex++;
        _checkpoints[_currentCheckpointIndex].PlayerHitCheckpoint();

        _savedPoints = GameManager.Instance.Points;
        _started = DateTime.UtcNow;

    }

    public void KillPlayer()
    {
        StartCoroutine(KillplayerCo());
    }

    private IEnumerator KillplayerCo()
    {
        Player.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(3f);

        Camera.IsFollowing = true;

        if (_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(_savedPoints);
    }
}
