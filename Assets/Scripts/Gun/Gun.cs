using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public TMP_Text bulletCounterText;
    public Transform tip;
    public Bullet bulletPrefab;
    
    private int _bulletCounter = 3;
    private int _shotArea;
    private float _fireInterval;
    private float _elapsed;
    
    void Start()
    {
        SetBulletCounterText();
        UpdateShotArea(1);
        _elapsed = 0;
    }

    void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed > _fireInterval)
        {
            _elapsed = 0;
            Fire();
        }
    }

    void Fire()
    {
        for (int i = 0; i < _shotArea; i++)
        {
            var position = CalculateBulletStartPosition(i);
            // TODO: pool
            Instantiate(
                bulletPrefab,
                position,
                Quaternion.LookRotation(tip.forward, Vector3.up));
        }
    }

    Vector3 CalculateBulletStartPosition(int index)
    {
        if (index == 0)
        {
            return tip.position;
        }

        if (index % 2 == 0)
        {
            return tip.position +
                   new Vector3(0, index / 2f - 0.5f, 0);
        }
        
        return tip.position +
               new Vector3(0, index / -2f, 0);
    }

    public void UpdateShotArea(int area)
    {
        _shotArea = area;
        CalculateFireInterval();
    }

    public void UpdateBulletCounter(GunExtension extension, bool removed = false)
    {
        if (removed)
        {
            switch (extension.operationType)
            {
                case OperationType.Add:
                    _bulletCounter -= extension.operand;
                    break;
                
                case OperationType.Multiply:
                    _bulletCounter /= extension.operand;
                    break;
            }
        }
        else
        {
            switch (extension.operationType)
            {
                case OperationType.Add:
                    _bulletCounter += extension.operand;
                    break;
                
                case OperationType.Multiply:
                    _bulletCounter *= extension.operand;
                    break;
            }
        }
        SetBulletCounterText();
    }

    void SetBulletCounterText()
    {
        bulletCounterText.text = $"{_bulletCounter} / sec";
    }
    
    void CalculateFireInterval()
    {
        _fireInterval = 1f / _bulletCounter * _shotArea;
    }
}
