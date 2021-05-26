using Assets.Scripts.WorldObjects;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//Mostly from the point of view of the Airlock Door on the Shuttle
public class AirlockDoor : MonoBehaviour, IDoor
{
    private ShipController _shipController;
    private AirlockDoor _dockableDoor;

    private Animator _anim;
    private SpriteRenderer _doorPanel;
    private Sprite _openPanel;
    private Sprite _closedPanel;
    private Light2D _panelLight;

    public Room parentRoom;

    private bool _doorState;
    private Vector3 _dockingOffset = new Vector3();

    public bool doorSeal;
    public bool isShuttleDoor;
    public bool dockingAvailable;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
        parentRoom = gameObject.GetComponentInParent<Room>();
    }

    private void Start()
    {
        _shipController = FindObjectOfType<ShipController>();
        var doorPanelObject = gameObject.transform.Find("DoorPanel");
        _doorPanel = doorPanelObject.GetComponent<SpriteRenderer>();
        _panelLight = doorPanelObject.transform.Find("PanelLight").GetComponent<Light2D>();

        Sprite[] _atlas = Resources.LoadAll<Sprite>("Sprites/Interactables/doorButtons");
        _openPanel = _atlas[0];
        _closedPanel = _atlas[1];
        _panelLight.color = Color.red;
    }

    private void Update()
    {
        if (_doorState == true && doorSeal == false)
        {
            CloseDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Airlock")
        {
            dockingAvailable = true;
            _dockableDoor = collision.gameObject.GetComponent<AirlockDoor>();
        }
    }

    public void OpenDoor()
    {
        _anim.SetBool("Open", true);
        _doorState = true;

        SetPanelSprites();
    }

    public void CloseDoor()
    {
        _anim.SetBool("Open", false);
        _doorState = false;

        SetPanelSprites();
    }

    public void ToggleDoor()
    {
        if (_doorState == false && doorSeal == true)
        {
            _dockableDoor.OpenDoor();
            OpenDoor();
        }
        else if (_doorState == true && doorSeal == true)
        {
            _dockableDoor.CloseDoor();
            CloseDoor();
        }
        else
        {
            Debug.LogError("No Airlock Seal");
        }
    }

    public void SetPanelSprites()
    {
        if (_doorState)
        {
            _doorPanel.sprite = _openPanel;
            _panelLight.color = Color.green;
        }
        else
        {
            _doorPanel.sprite = _closedPanel;
            _panelLight.color = Color.red;
        }
    }

    public void ToggleDoorLights(bool toggle)
    {
        return;
    }

    public void Dock()
    {
        doorSeal = true;
        _dockableDoor.doorSeal = true;
        _dockingOffset = _dockableDoor.parentRoom.transform.Find("DockingOffset").transform.position;
        _dockableDoor.parentRoom.PowerUpRoom("shuttle");
        _dockableDoor.parentRoom.shuttlePowerIndex = 0;        
    }

    public void Undock()
    {
        doorSeal = false;
        _dockableDoor.doorSeal = false;
        CloseDoor();
        _shipController.TurnOffShuttlePowerToAllRooms();
        dockingAvailable = false;
        _dockableDoor = null;
    }

    public Vector3 GetDockingPosition()
    {
        Debug.LogError(_dockingOffset);

        return _dockingOffset;
    }

    public Quaternion GetDockingRotation()
    {
        return _dockableDoor.parentRoom.transform.rotation;
    }

    public Vector3 GetUndockingPosition()
    {
        return _dockingOffset * 1.1f;
    }
}