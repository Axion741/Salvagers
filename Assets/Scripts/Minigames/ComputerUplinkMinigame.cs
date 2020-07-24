using Assets.Scripts.Minigames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Text _usbSuccessText;
    private Image[] _usbLights;
    private Sprite[] _lightSprites;

    private List<Connection> _connections = new List<Connection>();
    private string _ownIP;
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
        _usbSuccessText = GameObject.Find("SuccessText").GetComponent<Text>();
        _usbLights = GameObject.Find("Lights").GetComponentsInChildren<Image>();
        _lightSprites = Resources.LoadAll<Sprite>("Sprites/Environment/lights");

        _uiElements.SetActive(false);
        _usbUIElements.SetActive(false);
        _usb.SetActive(false);
        _maxConnectionText.text = _maxConnections.ToString();

        for(var x = 0; x < 4; x++)
        {
            _ipTexts[x].enabled = false;
            _connectionTexts[x].enabled = false;
        }

        CreateConnections();
        _noteText.text = _ownIP;

        _input.Select();
        _input.ActivateInputField();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
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

        var x = 0;

        foreach (var ip in ipList)
        {
            _connections.Add(new Connection()
            {
                ip = ip,
                status = _connectionTypes[Random.Range(0, _connectionTypes.Count)],
                ipText = _ipTexts[x],
                statusText = _connectionTexts[x]
            });
            x++;
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
        _input.enabled = false;

        if (string.IsNullOrWhiteSpace(_input.text))
        {
            DisplayError("NO INPUT");

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();

            return;
        }

        var inputCommands = _input.text.Split(' ');

        switch (inputCommands.Length) 
        {
            case 1:
                if (_commands.Contains(inputCommands[0]) && inputCommands[0] != _commands[0])
                {
                    DisplayError("IP REQUIRED");

                    _input.enabled = true;
                    _input.Select();
                    _input.ActivateInputField();

                    return;
                }
                else if (inputCommands[0] == _commands[0])
                {
                    StartCoroutine(Scan());
                }
                else if (!_commands.Contains(inputCommands[0]))
                {
                    DisplayError("INVALID COMMAND");

                    _input.enabled = true;
                    _input.Select();
                    _input.ActivateInputField();

                    return;
                }
                break;

            case 2:
                if (inputCommands[0] == _commands[0] || !_commands.Contains(inputCommands[0]))
                {
                    DisplayError("INVALID COMMAND");

                    _input.enabled = true;
                    _input.Select();
                    _input.ActivateInputField();

                    return;
                }
                else if (_connections.Where(w => w.ip == inputCommands[1]).Count() == 0)
                {
                    DisplayError("IP NOT FOUND");

                    _input.enabled = true;
                    _input.Select();
                    _input.ActivateInputField();

                    return;
                }
                else if (inputCommands[0] == _commands[1])
                {
                    StartCoroutine(Connect(inputCommands[1]));
                }
                else if (inputCommands[0] == _commands[2])
                {
                    StartCoroutine(Disconnect(inputCommands[1]));
                }
                else if (inputCommands[0] == _commands[3])
                {
                    StartCoroutine(Repair(inputCommands[1]));
                }
                break;

            default:
                DisplayError("INVALID COMMAND");

                _input.enabled = true;
                _input.Select();
                _input.ActivateInputField();

                return;
        }
    }

    private IEnumerator Scan()
    {
        _usb.SetActive(true);

        _uiElements.SetActive(true);

        foreach (var connection in _connections)
        {
            connection.ipText.text = connection.ip;
            connection.ipText.enabled = true;

            var random = Random.Range(0.5f, 3f);

            var conStatusCor = SetConnectionStatus(connection, "SCANNING", connection.status, random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(0.5f);
        }

        UpdateUSBConditions();
        _usbUIElements.SetActive(true);

        _input.enabled = true;
        _input.Select();
        _input.ActivateInputField();
    }

    private IEnumerator Connect(string ip)
    {
        var connection = _connections.Where(w => w.ip == ip).Single();
        var random = Random.Range(0.5f, 3f);

        if (connection.status == _connectionTypes[0])
        {
            var conStatusCor = SetConnectionStatus(connection, "CONNECTING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[1])
        {
            var conStatusCor = SetConnectionStatus(connection, "CONNECTING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            DisplayError("CONNECTION ERROR");
            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[2])
        {
            var conStatusCor = SetConnectionStatus(connection, "CONNECTING", _connectionTypes[0], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
    }

    private IEnumerator Disconnect(string ip)
    {
        var connection = _connections.Where(w => w.ip == ip).Single();
        var random = Random.Range(0.5f, 3f);

        if (connection.status == _connectionTypes[0])
        {
            var conStatusCor = SetConnectionStatus(connection, "DISCONNECTING", _connectionTypes[2], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[1])
        {
            var conStatusCor = SetConnectionStatus(connection, "DISCONNECTING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            DisplayError("UNABLE TO DISCONNECT");
            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[2])
        {
            var conStatusCor = SetConnectionStatus(connection, "DISCONNECTING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            DisplayError("ERROR DISCONNECTING");
            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
    }

    private IEnumerator Repair(string ip)
    {
        var connection = _connections.Where(w => w.ip == ip).Single();
        var random = Random.Range(0.5f, 3f);

        if (connection.status == _connectionTypes[0])
        {
            var conStatusCor = SetConnectionStatus(connection, "REPAIRING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            DisplayError("REPAIR FAILED");
            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[1])
        {
            var randomSuccess = Random.Range(1, 3);

            var conStatusCor = SetConnectionStatus(connection, "REPAIRING", _connectionTypes[randomSuccess], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            if (randomSuccess == 1)
            {
                DisplayError("REPAIR INCOMPLETE");
            }
            else
            {
                DisplayError("REPAIR COMPLETE");
            }

            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
        else if (connection.status == _connectionTypes[2])
        {
            var conStatusCor = SetConnectionStatus(connection, "REPAIRING", _connectionTypes[1], random);
            StartCoroutine(conStatusCor);

            yield return new WaitForSecondsRealtime(random);

            DisplayError("REPAIR FAILED");
            UpdateUSBConditions();

            _input.enabled = true;
            _input.Select();
            _input.ActivateInputField();
        }
    }

    private IEnumerator SetConnectionStatus(Connection connection, string transitionText, string newStatus, float timescale)
    {
        connection.statusText.text = transitionText;
        connection.statusText.enabled = true;

        yield return new WaitForSecondsRealtime(timescale);

        connection.statusText.text = newStatus;
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

        if (_connections.Where(w => w.ownIP == true).Single().status != _connectionTypes[0])
        {
            _usbTexts[2].text = "No Uplink";
            _usbTexts[2].color = Color.red;
            _usbLights[2].sprite = _lightSprites[0];

            uplinkPass = false;
        }
        else if (_connections.Where(w => w.ownIP == true).Single().status == _connectionTypes[0] && (bandwidthPass != true || errorsPass != true))
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

        if (bandwidthPass && errorsPass && uplinkPass)
        {
            StartCoroutine(SuccessfulUplink());
        }
    }

    private IEnumerator SuccessfulUplink()
    {
        _input.enabled = false;

        yield return new WaitForSecondsRealtime(1f);

        foreach (var text in _usbTexts)
        {
            text.enabled = false;
        }

        _usbSuccessText.color = Color.green;
        _usbSuccessText.enabled = true;

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSecondsRealtime(0.4f);

            _usbSuccessText.enabled = false;

            yield return new WaitForSecondsRealtime(0.4f);

            _usbSuccessText.enabled = true;
        }

        yield return new WaitForSecondsRealtime(1f);

        _success = true;
        CloseWindow();
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

    public void CloseWindow()
    {
        Time.timeScale = 1;

        _parent.MinigameResult(_success);
        Destroy(gameObject);
    }

    private class Connection : MonoBehaviour
    {
        public string ip;
        public string status;
        public bool ownIP;
        public Text ipText;
        public Text statusText;
    }
}