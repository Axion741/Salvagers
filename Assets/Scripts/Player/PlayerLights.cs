using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLights : MonoBehaviour
{
    private Light2D _suitLight;
    private Light2D _flashlight;

    private void Awake()
    {
        _suitLight = this.transform.Find("SuitLight").GetComponent<Light2D>();
        _flashlight = this.transform.Find("FlashLight").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.timeScale > 0)
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
