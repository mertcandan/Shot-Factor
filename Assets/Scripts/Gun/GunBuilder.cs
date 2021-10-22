using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GunBuilder : MonoBehaviour
{
    [Header("References")]
    public GameObject tip;
    public Camera mainCamera;
    public LayerMask gunExtensionLayer;
    
    private List<GunExtension> _extensions;
    private bool _enabled;
    private Gun _gun;
    

    void Start()
    {
        _extensions = new List<GunExtension>();
        _enabled = true;
        _gun = GetComponent<Gun>();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (_enabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit, 100f, gunExtensionLayer)) {
                Debug.Log(hit.transform.gameObject.name);
                hit.transform.GetComponent<GunExtension>().OnSelected();
            }
        }
    }

    public void AddExtension(GunExtension extension)
    {
        _enabled = false;
        int count = _extensions.Count;
        if (count > 0)
        {
            _extensions[count - 1].isLast = false;
        }

        extension.canAdd = false;
        extension.isLast = true;
        _extensions.Add(extension);
        var tipPosition = tip.transform.position;
        var xScale = extension.transform.localScale.x;
        extension.Move(tipPosition + xScale / 2f * Vector3.right);
        tip.transform.position = tipPosition +
                                 new Vector3(
                                     extension.transform.localScale.x,
                                     0,
                                     0);

        _gun.UpdateBulletCounter(extension);
        _gun.UpdateShotArea(extension.shotArea);
    }

    public void RemoveLastExtension()
    {
        _enabled = false;
        int count = _extensions.Count;
        _extensions[count - 1].isLast = false;
        _extensions[count - 1].canAdd = true;
        _extensions[count - 1].Reset();
        var tipPosition = tip.transform.position;
        tip.transform.position = tipPosition -
                                 new Vector3(
                                     _extensions[count - 1].transform.localScale.x,
                                     0,
                                     0);
        
        _gun.UpdateBulletCounter(_extensions[count - 1], true);
        _extensions.RemoveAt(count - 1);
        
        count--;
        if (count > 0)
        {
            _extensions[count - 1].isLast = true;
            _gun.UpdateShotArea(_extensions[count - 1].shotArea);
        }
        else
        {
            _gun.UpdateShotArea(1);
        }
    }

    public void MovementDone()
    {
        _enabled = true;
    }

    public void OnBattlePressed()
    {
        
    }

    void PrepareGun()
    {
        
    }

    void ActivateBattle()
    {
        
    }
}
