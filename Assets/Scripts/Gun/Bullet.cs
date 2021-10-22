using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")]
    public float speed = 5f;
    
    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
