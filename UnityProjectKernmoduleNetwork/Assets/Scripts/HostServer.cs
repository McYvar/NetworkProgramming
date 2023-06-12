using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class HostServer : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private ServerList serverList;
    [SerializeField] private TMP_InputField serverNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField portInputField;
    [SerializeField] private Toggle hostLocalToggle;
    private ushort hostLocal = 1; // 0 false, 1 true

    [SerializeField] private GameObject baseServerPrefab;
    [SerializeField] private GameObject baseClientPrefab;
    [SerializeField] private ButtonAction onHostButtonClick;

    private void Start()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        onHostButtonClick.OnClickButton += OnHostButton;
    }

    public void OnHostButton()
    {
        // TO DO: when clicking the button request to host server (create a server) on the given ip
        StartCoroutine(webRequest.Request<Servers>($"https://studenthome.hku.nl/~yvar.toorop/php/server_create_server?ip={ipInputField.text}&port={portInputField.text}&local={hostLocal}&server_name={serverNameInputField.text}&password={passwordInputField.text}",
            (request) =>
            {
                if (request != null)
                {
                    Debug.Log(request.ToString());
                    if (request.servers.Count > 0)
                    {
                        bool predicate = Convert.ToBoolean(request.servers[0].code);
                        if (predicate)
                        {
                            BaseServer server = Instantiate(baseServerPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseServer>();
                            server.ip = ipInputField.text;
                            server.port = (ushort)Convert.ToInt16(portInputField.text);

                            BaseClient client = Instantiate(baseClientPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseClient>();
                            client.ip = ipInputField.text;
                            client.port = (ushort)Convert.ToInt16(portInputField.text);
                        }

                        onHostButtonClick.PredicateAction(predicate);
                    }
                }
            }));
        
    }

    public void SetHostLocal(bool value)
    {
        if (value) hostLocal = 1;
        else hostLocal = 0;
    }
}
