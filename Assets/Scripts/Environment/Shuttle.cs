using UnityEngine;

public class Shuttle : MonoBehaviour
{
    private AirlockDoor _shuttleDoor;

    private Vector3 _lastPosition;

    void Start()
    {
        _lastPosition = this.transform.position;
        _shuttleDoor = this.gameObject.transform.Find("ShuttleDoor").GetComponent<AirlockDoor>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.transform.position = this.transform.position + new Vector3(-1f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            this.transform.position = this.transform.position + new Vector3(1f, 0, 0);
        }

        var currentPos = this.transform.position;

        if (currentPos != _lastPosition)
        {
            _shuttleDoor.doorSeal = false;
        }

        _lastPosition = this.transform.position;
    }
}
