using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public SkinnedMeshRenderer meshRenderer;
    public NavMeshAgent agent;
    public TMP_Text powerText;
    
    [Header("Parameters")]
    public int enemyPowerLevel = 1;
    public Color[] enemyColors;

    private int _currentPower;
    private Spawner _spawner;
    
    private void Start()
    {
        _currentPower = (int) Mathf.Pow(2f,enemyPowerLevel);
        powerText.text = _currentPower.ToString();
        meshRenderer.material.SetColor("_Color", enemyColors[enemyPowerLevel - 1]);
        transform.localScale = new Vector3(
            1f + 0.5f * enemyPowerLevel, 1f + 0.5f * enemyPowerLevel, 1f + 0.5f * enemyPowerLevel);
    }

    public void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }

    public void SetSpawner(Spawner spawner)
    {
        _spawner = spawner;
    }

    public void OnHitBullet()
    {
        _currentPower--;
        powerText.text = _currentPower.ToString();
        
        if (_currentPower <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (enemyPowerLevel > 1)
        {
            enemyPowerLevel--;
            Enemy right = Instantiate(
                this,
                transform.position + 0.5f * Vector3.right,
                transform.rotation);
            right.enemyPowerLevel = enemyPowerLevel;
            right.SetTarget(agent.destination);
            right.SetSpawner(_spawner);
            _spawner.OnEnemySpawned();
            
            Enemy left = Instantiate(
                this,
                transform.position + 0.5f * Vector3.left,
                transform.rotation);
            left.enemyPowerLevel = enemyPowerLevel;
            left.SetTarget(agent.destination);
            left.SetSpawner(_spawner);
            _spawner.OnEnemySpawned();
        }
        _spawner.OnEnemyDeath();
        Destroy(gameObject);
    }
}
