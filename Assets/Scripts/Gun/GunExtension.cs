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
    
    public bool canAdd;
    public bool isLast;

    private Vector3 _initialPosition;
    private GunBuilder _gunBuilder;
    
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
            _gunBuilder.AddExtension(this);
        }
        else if (isLast)
        {
            _gunBuilder.RemoveLastExtension();
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
        _gunBuilder.MovementDone();
    }
}
