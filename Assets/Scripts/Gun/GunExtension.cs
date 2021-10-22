using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunExtension : MonoBehaviour
{
    [Header("Parameters")]
    public float movementDuration = 1f;
    
    [Header("References")]
    public GunBuilder gunBuilder;
    
    public bool canAdd;
    public bool isLast;

    private Vector3 _initialPosition;
    
    void Start()
    {
        canAdd = true;
        isLast = false;

        _initialPosition = transform.position;
    }

    void Update()
    {
        
    }

    public void OnSelected()
    {
        if (canAdd)
        {
            gunBuilder.AddExtension(this);
        }
        else if (isLast)
        {
            gunBuilder.RemoveLastExtension();
        }
    }

    public void Move(Vector3 position)
    {
        StartCoroutine(Moving(position));
    }

    public void Reset()
    {
        StartCoroutine(Moving(_initialPosition));
    }

    IEnumerator Moving(Vector3 position)
    {
        float elapsed = 0;
        var startPosition = transform.position;
        while (elapsed < movementDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(
                startPosition,
                position,
                elapsed / movementDuration);
            yield return new WaitForEndOfFrame();
        }

        transform.position = position;
        gunBuilder.MovementDone();
    }
}
