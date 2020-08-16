using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HardDrive : MonoBehaviour, IPointerClickHandler
{
    private GameObject _topKeeper;
    private GameObject _bottomKeeper;
    private Slider _slider;

    private bool _keepersPopped;

    // Start is called before the first frame update
    void Start()
    {
        _topKeeper = gameObject.transform.Find("Keepers/KeeperTop").gameObject;
        _bottomKeeper = gameObject.transform.Find("Keepers/KeeperBottom").gameObject;
        _slider = gameObject.transform.parent.Find("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_slider.value >= 1 && !_keepersPopped)
        {
            PopKeepers();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_keepersPopped)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }

    private void PopKeepers()
    {
        _topKeeper.transform.localPosition = new Vector3(_topKeeper.transform.localPosition.x, _topKeeper.transform.localPosition.y + 25f, _topKeeper.transform.localPosition.z);
        _bottomKeeper.transform.localPosition = new Vector3(_bottomKeeper.transform.localPosition.x, _bottomKeeper.transform.localPosition.y - 25f, _bottomKeeper.transform.localPosition.z);
        _keepersPopped = true;
    }


}
