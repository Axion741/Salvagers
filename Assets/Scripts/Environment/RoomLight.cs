using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RoomLight : MonoBehaviour
{
    public Light2D lightSource;
    public bool undamageable;
    public float damagedChance = 0.2f;
    public float destroyedChance = 0.05f;

    private bool _powered;
    private bool _damaged;
    private bool _destroyed;

    private float _timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        lightSource.enabled = false;

        if (!undamageable)
            RandomizeDamageState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_destroyed)
            return;

        if (_powered && _damaged)
        {
            Flicker();
        }    
    }

    private void RandomizeDamageState()
    {
        var random = Random.value; 

        if (random <= destroyedChance)
        {
            _destroyed = true;
            return;
        }

        if (random <= damagedChance)
        {
            _damaged = true;
            lightSource.intensity = 0.25f;
        }
    }

    private void Flicker()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            lightSource.enabled = !lightSource.enabled;
            _timer = Random.Range(0.2f, 0.8f);
        }
    }

    public void TogglePower(bool power, float intensity = 1)
    {
        _powered = power;
        lightSource.intensity = intensity;

        if (_destroyed)
            return;

        if (!power)
            lightSource.enabled = false;

        if (power && !_damaged)
            lightSource.enabled = true;
    }
}
