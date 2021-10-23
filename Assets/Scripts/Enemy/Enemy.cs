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
    public int enemyPower = 1;
    public Color[] enemyColors;

    private int _currentPower;
    
    private void Start()
    {
        _currentPower = enemyPower;
        meshRenderer.material.SetColor("_Color", enemyColors[enemyPower - 1]);
        transform.localScale = new Vector3(
            0.5f + 0.5f * enemyPower, 0.5f + 0.5f * enemyPower, 0.5f + 0.5f * enemyPower);
        powerText.text = enemyPower.ToString();
    }

    public void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }

    public void Hit()
    {
        _currentPower--;

        if (_currentPower <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // TODO: destroy
        // if power > 1 instantiate new ones with half power
    }
}
