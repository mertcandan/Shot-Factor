using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    
    void Start()
    {
        StartCoroutine(SpawningEnemies());
    }

    void Update()
    {
        
    }

    IEnumerator SpawningEnemies()
    {
        foreach (EnemyData enemyData in enemies)
        {
            var rand = Random.insideUnitCircle;
            var position = transform.position +
                           transform.forward * rand.x + transform.right * rand.y;
            Enemy enemy = Instantiate(
                enemyPrefab,
                position,
                Quaternion.LookRotation(transform.forward));
            enemy.enemyPower = enemyData.power;
            enemy.SetTarget(enemyTarget.position);
            
            yield return new WaitForSeconds(enemyData.spawnDelay);
        }
    }
}
