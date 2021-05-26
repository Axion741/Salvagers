using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _cam;
    private Transform _shuttleTransform;

    private float moveSpeed = 5f;
    private float _xMovement;
    private float _yMovement;
    private float _mouseZ = 5.23f;
    private float _turnAngle;
    private Vector3 _mousePosition;
    private Vector3 _currentPosition;

    public bool playerMovementEnabled = true;

    private void Awake()
    {
       _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _shuttleTransform = FindObjectOfType<Shuttle>().transform;
    }

    private void Update()
    {
        _xMovement = Input.GetAxisRaw("Horizontal");
        _yMovement = Input.GetAxisRaw("Vertical");

        RotateToMouse();
    }

    private void FixedUpdate()
    {
        if (playerMovementEnabled)
        {
            var normalizedVelocity = new Vector2(_xMovement, _yMovement).normalized; //.normalized stops diagonal movement being faster
            _rb.velocity = normalizedVelocity * moveSpeed; 
        }
    }

    private void RotateToMouse()
    {
        if (playerMovementEnabled)
        {
            _currentPosition = _cam.WorldToScreenPoint(transform.position);

            _mousePosition = Input.mousePosition;
            _mousePosition.z = _mouseZ;
            _mousePosition.x = _mousePosition.x - _currentPosition.x;
            _mousePosition.y = _mousePosition.y - _currentPosition.y;

            _turnAngle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _turnAngle));
        }
    }

    public void ChildPlayerToShuttle(bool value)
    {
        var transform = this.gameObject.transform;

        if (value == true)
        {
            transform.parent = _shuttleTransform;
            this.gameObject.AddComponent<LockToLocalPosition>();
        }
        else
        {
            transform.parent = null;
            Destroy(this.gameObject.GetComponent<LockToLocalPosition>());
        }
    }
}