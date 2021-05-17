using UnityEngine;

public class Shuttle : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private AirlockDoor _shuttleDoor;

    private GameObject _player;
    private Vector3 _lastPosition;

    public int shuttlePowerDepth;
    public bool shuttleMovementEnabled = false;

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _lastPosition = this.transform.position;
        _shuttleDoor = this.gameObject.transform.Find("ShuttleDoor").GetComponent<AirlockDoor>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && shuttleMovementEnabled)
        {
            this.transform.position = this.transform.position + new Vector3(-1f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.D) && shuttleMovementEnabled)
        {
            this.transform.position = this.transform.position + new Vector3(1f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.R) && shuttleMovementEnabled)
        {
            RelinquishControl();
        }

        var currentPos = this.transform.position;

        if (currentPos != _lastPosition)
        {
            _shuttleDoor.doorSeal = false;
        }

        _lastPosition = this.transform.position;
    }

    private void RelinquishControl()
    {
        _playerMovement.playerMovementEnabled = true;
        shuttleMovementEnabled = false;
        _playerMovement.ChildPlayerToShuttle(false);
    }
}
