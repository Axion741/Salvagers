using Assets.Scripts.Minigames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUplinkMinigame : MonoBehaviour, IMinigame
{
    private Computer _parent;

    private GameObject _usb;
    private GameObject _uiElements;
    private Text[] _ipTexts;
    private Text[] _connectionTexts;
    private InputField _input;
    private Text _errorText;
    private Text _maxConnectionText;
    private Text _noteText;
    private GameObject _usbUIElements;
    private Text[] _usbTexts;
    private Image[] _usbLights;
    private Sprite[] _lightSprites;

    private List<Connection> _connections = new List<Connection>();
    private string _ownIP;
    private bool _scanned = false;
    private bool _inputEnabled = true;
    private int _maxConnections;
    private bool _success = false;

    private List<string> _possibleIPs = new List<string>
    {
        "60.25.235.103",
        "234.207.235.210",
        "185.142.162.39",
        "97.154.112.178",
        "87.43.158.73",
        "171.118.79.201",
        "177.173.76.194",
        "103.11.241.150",
        "1.85.169.30",
        "229.19.51.99",
        "21.176.195.221",
        "246.195.93.236",
        "17.218.69.107",
        "3.103.212.217",
        "149.193.219.237",
        "243.121.152.106",
        "195.116.233.51",
        "45.71.102.186",
        "127.112.30.106",
        "146.174.247.171",
        "246.244.93.131",
        "254.60.79.77",
        "186.3.139.83",
        "105.96.95.3",
        "54.200.181.51"
    };

    private List<string> _connectionTypes = new List<string>
    {
        "CONNECTED",
        "ERROR",
        "DISCONNECTED"
    };

    private List<string> _commands = new List<string>
    {
        "IPSCAN",
        "IPCONNECT",
        "IPDISCONNECT",
        "IPREPAIR"
    };

    private void Awake()
    {
        Time.timeScale = 0;
        _maxConnections = Random.Range(1, 5);

        CreateConnections();
    }

    private void Start()
    {
        _usb = GameObject.Find("USB");
        _uiElements = GameObject.Find("UIElements");
        _ipTexts = GameObject.Find("TextIPs").GetComponentsInChildren<Text>();
        _connectionTexts = GameObject.Find("TextConnections").GetComponentsInChildren<Text>();
        _input = GameObject.Find("InputField").GetComponent<InputField>();
        _errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        _maxConnectionText = GameObject.Find("MaxConnections").GetComponent<Text>();
        _noteText = GameObject.Find("NoteText").GetComponent<Text>();
        _usbUIElements = GameObject.Find("USBUI");
        _usbTexts = GameObject.Find("USBUI").GetComponentsInChildren<Text>();
        _usbLights = GameObject.Find("Lights").GetComponentsInChildren<Image>();
        _lightSprites = Resources.LoadAll<Sprite>("Sprites/Environment/lights");

        _uiElements.SetActive(false);
        _usbUIElements.SetActive(false);
        _usb.SetActive(false);
        _maxConnectionText.text = _maxConnections.ToString();
        _noteText.text = _ownIP;

        for(var x = 0; x < 4; x++)
        {
            _ipTexts[x].enabled = false;
            _connectionTexts[x].enabled = false;
        }

        _input.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && _inputEnabled)
        {
            ProcessInput();
        }
    }

    public void SetParent(Computer parent)
    {
        _parent = parent;
    }

    private void CreateConnections()
    {
        var ipList = new List<string>();

        while (ipList.Count < 4)
        {
            var random = Random.Range(0, _possibleIPs.Count());
            ipList.Add(_possibleIPs[random]);
            _possibleIPs.RemoveAt(random);
        }

        foreach (var ip in ipList)
        {
            _connections.Add(new Connection()
            {
                ip = ip,
                status = _connectionTypes[Random.Range(0, _connectionTypes.Count)]
            }); 
        }

        var ownConnection = Random.Range(0, _connections.Count);
        _connections[ownConnection].ownIP = true;
        _ownIP = _connections[ownConnection].ip;

        if (_connections[ownConnection].status == _connectionTypes[0])
        {
            _connections[ownConnection].status = _connectionTypes[Random.Range(1, _connectionTypes.Count)];
        }
    }

    private void ProcessInput()
    {
        _inputEnabled = false;

        if (string.IsNullOrWhiteSpace(_input.text))
        {
            DisplayError("NO INPUT");

            _inputEnabled = true;
            _input.Select();

            return;
        }

        var inputCommands = _input.text.Split(' ');

        if (!_scanned || inputCommands.Length > 2)
        {
            if(inputCommands.Length > 1 || inputCommands[0] != _commands[0])
            {
                DisplayError("INVALID COMMAND");

                _inputEnabled = true;
                _input.Select();

                return;
            }
        }
        else if (_connections.Select(s => s.ip == inputCommands[1]).Count() > 0)
        {
            DisplayError("IP NOT FOUND");

            _inputEnabled = true;
            _input.Select();

            return;
        }

        if (inputCommands[0] == _commands[0])
        {
            StartCoroutine(Scan());
        }
        else if (inputCommands[0] == _commands[1])
        {
            //Connect
        }
        else if (inputCommands[0] == _commands[2])
        {
            //Disconnect
        }
        else if (inputCommands[0] == _commands[3])
        {
            //Repair
        }
    }

    private IEnumerator Scan()
    {
        _usb.SetActive(true);

        _uiElements.SetActive(true);

        for (var x = 0; x < 4; x++)
        {
            _ipTexts[x].text = _connections[x].ip;
            _ipTexts[x].enabled = true;

            var conStatusCor = SetConnectionStatus(_connections[x], _connectionTexts[x], "SCANNING", _connections[x].status, Random.Range(0.5f, 3f));
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(Random.Range(0.2f, 1f));
        }

        UpdateUSBConditions();
        _usbUIElements.SetActive(true);

        _inputEnabled = true;
    }

    private IEnumerator SetConnectionStatus(Connection connection, Text connectionText, string transitionText, string newStatus, float timescale)
    {
        connectionText.text = transitionText;
        connectionText.enabled = true;

        yield return new WaitForSecondsRealtime(timescale);

        connectionText.text = newStatus;
        connection.status = newStatus;
    }

    private void UpdateUSBConditions()
    {
        bool bandwidthPass;
        bool errorsPass;
        bool uplinkPass;

        var connected = _connections.Where(w => w.status == _connectionTypes[0]).Count();
        var errored = _connections.Where(w => w.status == _connectionTypes[1]).Count();

        if (connected > _maxConnections)
        {
            _usbTexts[0].text = "Insufficient Bandwidth";
            _usbTexts[0].color = Color.red;
            _usbLights[0].sprite = _lightSprites[0];

            bandwidthPass = false;
        }
        else
        {
            _usbTexts[0].text = "Connections Stable";
            _usbTexts[0].color = Color.green;
            _usbLights[0].sprite = _lightSprites[1];

            bandwidthPass = true;
        }

        if (errored > 0)
        {
            _usbTexts[1].text = "Errors Detected!";
            _usbTexts[1].color = Color.red;
            _usbLights[1].sprite = _lightSprites[0];

            errorsPass = false;
        }
        else
        {
            _usbTexts[1].text = "No Errors Detected";
            _usbTexts[1].color = Color.green;
            _usbLights[1].sprite = _lightSprites[1];

            errorsPass = true;
        }

        if (_connections.Where(w => w.ownIP == true).Single().status != _connectionTypes[1])
        {
            _usbTexts[2].text = "No Uplink";
            _usbTexts[2].color = Color.red;
            _usbLights[2].sprite = _lightSprites[0];

            uplinkPass = false;
        }
        else if (_connections.Where(w => w.ownIP == true).Single().status == _connectionTypes[1] && (bandwidthPass != true || errorsPass != true))
        {
            _usbTexts[2].text = "Uplink Unstable";
            _usbTexts[2].color = Color.red;
            _usbLights[2].sprite = _lightSprites[2];

            uplinkPass = false;
        }
        else
        {
            _usbTexts[2].text = "Uplink Stable";
            _usbTexts[2].color = Color.green;
            _usbLights[2].sprite = _lightSprites[1];

            uplinkPass = true;
        }

        _success = bandwidthPass && errorsPass && uplinkPass;
    }

    private void DisplayError(string error)
    {
        _errorText.text = error;
        _errorText.enabled = true;

        var hideErrorCor = HideError(2f);
        StartCoroutine(hideErrorCor);
    }

    private IEnumerator HideError(float timescale)
    {
        yield return new WaitForSecondsRealtime(timescale);

        _errorText.enabled = false;
    }

    //private void InitializeFields()
    //{
    //    _ownIP = _possibleOwnIPs[Random.Range(0, _possibleOwnIPs.Count)];

    //    var ipStrings = new List<string> {_ownIP};

    //    while (ipStrings.Count < 4)
    //    {
    //        var random = Random.Range(0, _possibleIPs.Count);
    //        ipStrings.Add(_possibleIPs[random]);
    //        _possibleIPs.RemoveAt(random);
    //    }

    //    ipStrings = ipStrings.OrderBy(i => System.Guid.NewGuid()).ToList();

    //    for (var i = 0; i < _ips.Length; i++)
    //    {
    //        _ips[i].text = ipStrings[i];

    //        if(ipStrings[i] == _ownIP)
    //        {
    //            _ownIPObject = _connections[i];
    //            _noteText.text = _ownIP;
    //            _connections[i].text = "DISCONNECTED";
    //            _connections[i].color = Color.red;
    //        }
    //        else
    //        {
    //            switch (Random.Range(0, 3)) 
    //            {
    //                case 0:
    //                    _connections[i].text = "DISCONNECTED";
    //                    _connections[i].color = Color.red;
    //                    break;

    //                case 1:
    //                    _connections[i].text = "CONNECTED";
    //                    _connections[i].color = Color.green;
    //                    break;

    //                case 2:
    //                    _connections[i].text = "ERROR";
    //                    _connections[i].color = Color.red;
    //                    break;
    //            }
    //        }
    //    }

    //    _input.Select();
    //}

    public void CloseWindow()
    {
        Time.timeScale = 1;

        _parent.MinigameResult(_success);
        Destroy(gameObject);
    }
}

public class Connection : MonoBehaviour
{
    public string ip;
    public string status;
    public bool ownIP;
}
