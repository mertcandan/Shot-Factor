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
    public GameObject deathExplosion;
    
    [Header("Parameters")]
    public int enemyPowerLevel = 1;
    public Color[] enemyColors;

    private int _currentPower;
    private Spawner _spawner;
    private static readonly int MaterialColor = Shader.PropertyToID("_Color");
    private bool _alive;
    
    private void Start()
    {
        if (enemyPowerLevel > enemyColors.Length)
        {
            Debug.LogError("Enemy Power Level Exceeds " + enemyColors.Length);
            return;
        }

        _alive = true;
        _currentPower = (int) Mathf.Pow(2f,enemyPowerLevel);
        powerText.text = _currentPower.ToString();
        meshRenderer.material.SetColor(MaterialColor, enemyColors[enemyPowerLevel - 1]);
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
        
        if (_currentPower <= 0 && _alive)
        {
            _alive = false;
            Die();
        }
    }

    void Die()
    {
        if (enemyPowerLevel > 1)
        {
            _spawner.SpawnHalved(GetInstanceID(),enemyPowerLevel - 1, transform.position, transform.rotation);
        }
        _spawner.OnEnemyDeath(GetInstanceID());
        Destroy(Instantiate(deathExplosion, transform.position, Quaternion.identity), 0.5f);
        Destroy(gameObject);
    }
}
