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
    private float maxVelocity = 4;
    private float maxRotation = 20;
    private float rotationSpeed = 10;
    private float brakingForce = 3;

    public int shuttlePowerDepth;
    public bool shuttleMovementEnabled = false;
    public bool shuttleDocked = false;

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

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_shuttleDoor.dockingAvailable)
                    DockShuttle();
                else
                    Debug.Log("Cannot Dock - No Airlock in range");
            }
        }
    }

    private void FixedUpdate()
    {
        if (shuttleMovementEnabled)
        {
            Thrust(_yAxis * 5);
            Rotate(_xAxis * -rotationSpeed);
            ClampVelocity();
        }
    }

    private void RelinquishControl()
    {
        _playerMovement.playerMovementEnabled = true;
        shuttleMovementEnabled = false;
        _playerMovement.ChildPlayerToShuttle(false);
        ToggleLocks(true);
    }

    private void Thrust(float amount)
    {
        Vector2 force = transform.right * -amount;
        _rb.AddForce(force);
    }

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(_rb.velocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(_rb.velocity.y, -maxVelocity, maxVelocity);
        var vel = new Vector2(x, y);

        _rb.velocity = vel;

        var rot = Mathf.Clamp(_rb.angularVelocity, -maxRotation, maxRotation);
        _rb.angularVelocity = rot;
    }

    private void Rotate(float amount)
    {
        //transform.Rotate(0, 0, amount);
        _rb.AddTorque(amount);
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


    private void DockShuttle()
    {
        _shuttleDoor.Dock();
        this.transform.position = _shuttleDoor.GetDockingPosition();
        this.transform.rotation = _shuttleDoor.GetDockingRotation();
        shuttleDocked = true;
        RelinquishControl();
    }

    public void UndockShuttle()
    {
        this.transform.position = _shuttleDoor.GetUndockingPosition();
        _shuttleDoor.Undock();
        shuttleDocked = false;
    }
}