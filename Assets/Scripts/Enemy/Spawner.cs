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
    
    void Start()
    {
        _enemyCount = 0;
        StartCoroutine(SpawningEnemies());
    }

    IEnumerator SpawningEnemies()
    {
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
                yield return new WaitForSeconds(enemyData.spawnDelay);
            }
        }
    }

    public void OnEnemySpawned()
    {
        _enemyCount++;
    }

    public void OnEnemyDeath()
    {
        _enemyCount--;
        if (_enemyCount <= 0)
        {
            GameManager.Instance.LevelWon();
        }
    }
}
