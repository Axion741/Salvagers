using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLights : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private Light2D _suitLight;
    private Light2D _flashlight;

    private void Awake()
    {
        _suitLight = this.transform.Find("SuitLight").GetComponent<Light2D>();
        _flashlight = this.transform.Find("FlashLight").GetComponent<Light2D>();
    }

    private void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _playerMovement.playerMovementEnabled)
        {
            ToggleLights();
        }
    }

    private void ToggleLights()
    {
        _suitLight.enabled = !_suitLight.enabled;
        _flashlight.enabled = !_flashlight.enabled;
    }
}
