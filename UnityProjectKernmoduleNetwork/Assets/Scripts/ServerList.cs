using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerList : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    private bool updatingList;
    private List<GameObject> serverPrefabs = new List<GameObject>();
    [SerializeField] private float serverListGap;
    [SerializeField] private GameObject serverListItemPrefab;
    [SerializeField] private RectTransform scrollViewContent;

    [SerializeField] private GameObject serverLogin;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_Text loginText;
    [SerializeField] private ButtonAction onClickRefreshButton;

    [SerializeField] private CreateClient createClient;

    [SerializeField] private ButtonAction onClickConnectButton;

    private void Start()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;

        // make sure the client is disconnected from everything on startup
        StartCoroutine(webRequest.Request<Results>("https://studenthome.hku.nl/~yvar.toorop/php/server_logout?session_id={SessionVariables.instance.sessionId}",
            (request) =>
            {
                if (request != null)
                {
                    Debug.Log(request.ToString());
                }

                StartCoroutine(webRequest.Request<Results>("https://studenthome.hku.nl/~yvar.toorop/php/user_logout?session_id={SessionVariables.instance.sessionId}",
                    (request) =>
                    {
                        if (request != null)
                        {
                            Debug.Log(request.ToString());
                        }

                    }));
            }));

        StartCoroutine(UpdateServerList());

        onClickRefreshButton.OnClickButton += () => StartCoroutine(UpdateServerList());
    }

    private void OnApplicationQuit()
    {
        // TO DO: APPLICATION ACTUALLY HAS TO EXECUTE THESE ON EXIT
        //InstantRequest("https://studenthome.hku.nl/~yvar.toorop/php/user_logout");
        //InstantRequest("https://studenthome.hku.nl/~yvar.toorop/php/server_logout");
    }

    private void Update()
    {
        UpdateListSize();
    }

    public IEnumerator UpdateServerList()
    {
        updatingList = true;

        Servers servers = new Servers();
        bool isRequesting = true;
        StartCoroutine(webRequest.Request<Servers>("https://studenthome.hku.nl/~yvar.toorop/php/server_get_all_servers",
            (request) =>
            {
                if (request != null)
                {
                    servers = request;
                }

                isRequesting = false;
            }
            ));

        yield return new WaitUntil(() => !isRequesting);

        Debug.Log(servers);

        while (serverPrefabs.Count > 0)
        {
            Destroy(serverPrefabs[0]);
            serverPrefabs.RemoveAt(0);
        }

        for (int i = 0; i < servers.servers.Count; i++)
        {
            GameObject serverPrefab = Instantiate(serverListItemPrefab, scrollViewContent.transform);
            serverPrefab.GetComponent<TMP_Text>().text = $"Server: {servers.servers[i].server_name}";
            RectTransform rect = serverPrefab.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0, 1);
            rect.localPosition = new Vector2(0, 0 - i * serverListGap);
            ButtonAction serverSelectButton = serverPrefab.GetComponent<ButtonAction>();
            int id = servers.servers[i].server_id;
            string serverName = servers.servers[i].server_name;
            serverSelectButton.OnClickButton += () =>
            {
                passwordInputField.text = "";
                serverLogin.SetActive(true);
                loginText.text = $"LOGIN {serverName}";
                onClickConnectButton.OnClickButton = () =>
                {
                    StartCoroutine(webRequest.Request<Servers>($"https://studenthome.hku.nl/~yvar.toorop/php/server_login?session_id={SessionVariables.instance.sessionId}?server_id={id}&password={passwordInputField.text}",
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
                                        SessionVariables.instance.serverId = request.servers[0].server_id;
                                        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/user_get_all_users_from_server?session_id={SessionVariables.instance.sessionId}?server_id={request.servers[0].server_id}", (request2) =>
                                        {
                                            if (request != null)
                                            {
                                                if (request2.results.Count > 0)
                                                {
                                                    bool predicate = Convert.ToBoolean(request2.results[0].code);
                                                    if (predicate)
                                                    {
                                                        foreach (var result in request2.results)
                                                        {
                                                            Player player = new Player(result.user_id, result.username);
                                                            SessionVariables.instance.playerDictionary.Add(result.user_id, player);
                                                        }
                                                        // create client
                                                        createClient.CreateClientObject(request.servers[0].ip, (ushort)Convert.ToInt16(request.servers[0].port));
                                                        SessionVariables.instance.serverId = request.servers[0].server_id;
                                                        onClickConnectButton.PredicateAction(true);
                                                    }
                                                    else
                                                    {
                                                        StartCoroutine(webRequest.Request<Servers>("https://studenthome.hku.nl/~yvar.toorop/php/server_logout?session_id={SessionVariables.instance.sessionId}", null));
                                                    }

                                                }
                                            }
                                        }));
                                    }
                                    else onClickConnectButton.PredicateAction(false);
                                }
                            }
                        }));
                };
            };
            serverPrefabs.Add(serverPrefab);
        }
        updatingList = false;
    }

    private void UpdateListSize()
    {
        if (updatingList) return;

        for (int i = 0; i < serverPrefabs.Count; i++)
        {
            RectTransform rect = serverPrefabs[i].GetComponent<RectTransform>();

            rect.pivot = new Vector2(0, 1);
            rect.localPosition = new Vector2(0, 0 - i * serverListGap);
        }
        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, serverPrefabs.Count * serverListGap);
    }
}
