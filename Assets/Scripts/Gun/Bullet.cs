using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletExplosion;
    
    [Header("Parameters")]
    public float speed = 5f;
    public float destroyAfter = 3f;
    
    private float _lifeTime;
    
    void Start()
    {
        _lifeTime = 0;
    }

    void Update()
    {
        Move();
        _lifeTime += Time.deltaTime;
        if (_lifeTime > destroyAfter)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    public void OnHit()
    {
        Destroy(Instantiate(bulletExplosion, transform.position, Quaternion.identity), 0.5f);
        Destroy(gameObject);
    }
}
