using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public TMP_Text bulletCounterText;
    public Transform tip;
    public Bullet bulletPrefab;
    public Camera mainCamera;
    public Image crosshair;
    
    [Header("Parameters")]
    public float aimSensitivity = 1f; 
    
    private int _bulletCounter = 3;
    private int _shotArea;
    private float _fireInterval;
    private float _elapsed;
    
    private bool _canAim;
    private Vector3 _lastMousePosition;
    
    void Start()
    {
        SetBulletCounterText();
        UpdateShotArea(1);
        _elapsed = 0;
        _canAim = false;
    }

    void Update()
    {
        Aim();
        _elapsed += Time.deltaTime;
        if (_elapsed > _fireInterval)
        {
            _elapsed = 0;
            Fire();
        }
    }

    #region Fire
    
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

    public void UpdateShotArea(int area)
    {
        _shotArea = area;
        CalculateFireInterval();
    }
    
    void CalculateFireInterval()
    {
        _fireInterval = 1f / _bulletCounter * _shotArea;
    }
    
    #endregion
    
    #region Bullet

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
    
    #endregion

    #region Aim
    
    public void ActivateAim()
    {
        _canAim = true;
    }

    void Aim()
    {
        if (!_canAim)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var diff = Input.mousePosition - _lastMousePosition;
            Debug.Log("Touch diff: " + diff);
            MoveCrosshair(diff);
            _lastMousePosition = Input.mousePosition;
            //  Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        }
    }

    void MoveCrosshair(Vector3 diff)
    {
        crosshair.transform.position += diff;
        Debug.Log(crosshair.transform.position);
        
        // TODO: clamp position
    }
    
    #endregion
}
