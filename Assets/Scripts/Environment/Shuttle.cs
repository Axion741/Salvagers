using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shuttle : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private AirlockDoor _shuttleDoor;
    private Rigidbody2D _rb;
    private List<Rigidbody2D> _childRbs = new List<Rigidbody2D>();
    private List<GameObject> _walls = new List<GameObject>();

    private float _xAxis;
    private float _yAxis;

    public int shuttlePowerDepth;
    public bool shuttleMovementEnabled = false;
    public float maxVelocity = 3;
    public float rotationSpeed = 0.5f;
    public float brakingForce = 3;

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _shuttleDoor = this.gameObject.transform.GetComponentInChildren<AirlockDoor>();
        _rb = this.gameObject.transform.GetComponent<Rigidbody2D>();
        _childRbs = GetComponentsInChildren<Rigidbody2D>().ToList();
        _childRbs.Add(GameObject.Find("Player").GetComponent<Rigidbody2D>());
        var wallParent = this.gameObject.transform.Find("Structure/Walls");
        foreach (Transform wall in wallParent.transform)
        {
            _walls.Add(wall.gameObject);
        }
    }

    void Update()
    {
        if (shuttleMovementEnabled)
        {
            _yAxis = Input.GetAxisRaw("Vertical");
            _xAxis = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.R))
            {
                RelinquishControl();
            }

            if (Input.GetKey(KeyCode.X))
            {
                Brake();
            }
            else
            {
                _rb.drag = 0;
                _rb.angularDrag = 0;
            }
        }

        //var currentPos = this.transform.position;

        //if (currentPos != _lastPosition)
        //{
        //    _shuttleDoor.doorSeal = false;
        //}

        //_lastPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (shuttleMovementEnabled)
        {
            ThrustForward(_yAxis * 5);
            Rotate(_xAxis * -rotationSpeed);
            ClampVelocity(-maxVelocity);
        }
    }

    private void RelinquishControl()
    {
        _playerMovement.playerMovementEnabled = true;
        shuttleMovementEnabled = false;
        _playerMovement.ChildPlayerToShuttle(false);
        ToggleLocks(true);
    }

    private void ThrustForward(float amount)
    {
        Vector2 force = transform.right * -amount;
        Debug.Log(force);
        _rb.AddForce(force);
    }

    private void ClampVelocity(float minVelocity = 0)
    {
        float x = Mathf.Clamp(_rb.velocity.x, minVelocity, maxVelocity);
        float y = Mathf.Clamp(_rb.velocity.y, minVelocity, maxVelocity);
        var vel = new Vector2(x, y);

        _rb.velocity = vel;
    }

    private void Rotate(float amount)
    {
        transform.Rotate(0, 0, amount);
    }

    public void ToggleLocks(bool value)
    {
        if (value)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            foreach(var wall in _walls)
            {
                wall.layer = 9;
            }
        }

        else
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            foreach (var wall in _walls)
            {
                wall.layer = 8;
            }
        }
    }

    private void Brake()
    {
        _rb.drag = brakingForce;
        _rb.angularDrag = brakingForce;
    }
}