using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _anim;

    private bool _doorState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
    }

    public void ToggleDoor()
    {
        if (_doorState == false)
        {
            _anim.SetBool("Open", true);
            _doorState = true;
        }
        else
        {
            _anim.SetBool("Open", false);
            _doorState = false;
        }
    }

}
