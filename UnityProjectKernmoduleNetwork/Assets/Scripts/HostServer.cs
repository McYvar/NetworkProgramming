using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HostServer : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private ServerList serverList;
    [SerializeField] private TMP_InputField serverNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField localIpInputField;
    [SerializeField] private TMP_InputField globalIpInputField;
    [SerializeField] private TMP_InputField portInputField;
    [SerializeField] private Toggle hostLocalToggle;
    private ushort hostLocal = 1; // 0 false, 1 true

    [SerializeField] private ButtonAction onHostButtonClick;
    [SerializeField] private CreateServer createServer;
    [SerializeField] private CreateClient createClient;

    private void Start()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        onHostButtonClick.OnClickButton += OnHostButton;
    }

    public void OnHostButton()
    {
        // TO DO: when clicking the button request to host server (create a server) on the given ip
        string ip = hostLocal == 1 ? localIpInputField.text : globalIpInputField.text;
        StartCoroutine(webRequest.Request<Servers>($"https://studenthome.hku.nl/~yvar.toorop/php/server_create_server?ip={ip}&port={portInputField.text}&local={hostLocal}&server_name={serverNameInputField.text}&password={passwordInputField.text}",
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
                            // create server
                            createServer.CreateServerObject(localIpInputField.text, (ushort)Convert.ToInt16(portInputField.text));
                            // create client
                            createClient.CreateClientObject(localIpInputField.text, (ushort)Convert.ToInt16(portInputField.text));

                            // set server variables
                            SessionVariables.instance.serverId = request.servers[0].server_id;
                        }

                        onHostButtonClick.PredicateAction(predicate);
                    }
                }
            }));
        
    }

    public void SetHostLocal(bool value)
    {
        if (value)
        {
            hostLocal = 1;
            globalIpInputField.gameObject.SetActive(false);
        }
        else
        {
            hostLocal = 0;
            globalIpInputField.gameObject.SetActive(true);
        }
    }
}
