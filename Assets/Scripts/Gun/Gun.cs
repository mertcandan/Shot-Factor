using System;
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
    public TMP_Text battleBulletCounterText;
    public Transform tip;
    public Bullet bulletPrefab;
    public Camera mainCamera;
    public Image crosshair;

    public LayerMask groundLayer;
    
    [Header("Parameters")]
    public float aimSensitivity = 1f; 
    
    private int _bulletCounter = 3;
    private int _shotArea;
    private float _fireInterval;
    private float _elapsed;

    private bool _firing;
    private bool _canAim;
    private Vector3 _lastMousePosition;
    private Vector3 _aimPosition;
    
    void Start()
    {
        SetBulletCounterText();
        UpdateShotArea(1);
        _elapsed = 0;
        _canAim = false;
        _firing = true;
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
        if (!_firing)
        {
            return;
        }
        
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

    public void Prepare(Transform placement)
    {
        transform.SetPositionAndRotation(
            placement.position,
            placement.rotation);
        ActivateAim();
        ShowBattleBulletCounter();
        ShowCrosshair();
    }
    
    #region Aim
    

    void ShowBattleBulletCounter()
    {
        battleBulletCounterText.text = $"{_bulletCounter}/sec";
        battleBulletCounterText.gameObject.SetActive(true);
        bulletCounterText.gameObject.SetActive(false);
    }
    
    void ActivateAim()
    {
        _canAim = true;
        _firing = false;
    }

    void ShowCrosshair()
    {
        crosshair.gameObject.SetActive(true);
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
            Aim(diff);
            _lastMousePosition = Input.mousePosition;
            _firing = true;
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _firing = false;
        }
    }

    void Aim(Vector3 diff)
    {
        _aimPosition = new Vector3(
            Mathf.Clamp(crosshair.transform.position.x + diff.x, 0, Screen.width),
            Mathf.Clamp(crosshair.transform.position.y + diff.y, 0, Screen.height),
            0);
        
        crosshair.transform.position = _aimPosition;
        AimGun();
    }

    void AimGun()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(_aimPosition.x, _aimPosition.y, 15));
        
        if (Physics.Raycast(ray, out hit, 500f, groundLayer)) {
            Debug.Log(hit.transform.name);
            transform.LookAt(hit.point);
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.blue, 2f);
        }
    }
    
    #endregion
}
