using System.Collections;
using UnityEngine;

public enum OperationType
{
    Add,
    Multiply
}

public class GunExtension : MonoBehaviour
{
    [Header("Parameters")]
    public float movementDuration = 1f;
    public OperationType operationType;
    public int operand;
    public int shotArea = 1;
    
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
