using UnityEngine;

public class LockToLocalPosition : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    void Start()
    {
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
    }
}