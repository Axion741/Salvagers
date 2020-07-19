using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;

    private float _smoothSpeed = 0.125f;
    private Vector3 _offset;

    private void Start()
    {
        _target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        _offset = new Vector3(0f, 0f, -10f);
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
    }
}
