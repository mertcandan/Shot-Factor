using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class EnemyData
{
    public int power;
    public int enemyCount;
    public float spawnDelay;
}

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public Enemy enemyPrefab;
    public Transform enemyTarget;
    
    [Header("Parameters")]
    
    [SerializeField]
    private EnemyData[] enemies;
    private int _enemyCount;
    private bool _spawningDone;
    private List<int> _deadEnemyIds;
    
    void Start()
    {
        _enemyCount = 0;
        _deadEnemyIds = new List<int>();
        
        StartCoroutine(SpawningEnemies());
    }

    IEnumerator SpawningEnemies()
    {
        _spawningDone = false;
        foreach (EnemyData enemyData in enemies)
        {
            for (int i = 0; i < enemyData.enemyCount; i++)
            {
                var rand = Random.insideUnitCircle;
                var position = transform.position +
                               transform.forward * rand.x + transform.right * rand.y;
                Enemy enemy = Instantiate(
                    enemyPrefab,
                    position,
                    Quaternion.LookRotation(transform.forward));
                enemy.enemyPowerLevel = enemyData.power;
                enemy.SetTarget(enemyTarget.position);
                enemy.SetSpawner(this);
                
                _enemyCount++;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(enemyData.spawnDelay);
        }

        _spawningDone = true;
    }

    void OnEnemySpawned()
    {
        _enemyCount++;
    }

    public void OnEnemyDeath(int enemyId)
    {
        if (_deadEnemyIds.Contains(enemyId))
        {
            return;
        }
        _deadEnemyIds.Add(enemyId);
        _enemyCount--;
        if (_spawningDone && _enemyCount <= 0)
        {
            GameManager.Instance.LevelWon();
        }
    }

    public void SpawnHalved(int enemyId, int powerLevel, Vector3 position, Quaternion rotation)
    {
        if (_deadEnemyIds.Contains(enemyId))
        {
            return;
        }
        
        Enemy right = Instantiate(
            enemyPrefab,
            position + powerLevel * Vector3.right,
            rotation);
        right.enemyPowerLevel = powerLevel;
        right.SetTarget(enemyTarget.position);
        right.SetSpawner(this);
        OnEnemySpawned();
            
        Enemy left = Instantiate(
            enemyPrefab,
            position + powerLevel * Vector3.left,
            rotation);
        left.enemyPowerLevel = powerLevel;
        left.SetTarget(enemyTarget.position);
        left.SetSpawner(this);
        OnEnemySpawned();
    }
}
