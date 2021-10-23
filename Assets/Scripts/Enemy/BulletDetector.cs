using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDetector : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        enemy.Hit();
    }
}
