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

    private Server myServer = new Server();
    private Result mySession = new Result();

    private void Start()
    {
        // make sure the client is disconnected from everything on startup
        StartCoroutine(webRequest.Request<Results>("https://studenthome.hku.nl/~yvar.toorop/php/user_logout",
            (request) =>
            {
                if (request != null)
                {
                    Debug.Log(request.ToString());
                }

                StartCoroutine(webRequest.Request<Results>("https://studenthome.hku.nl/~yvar.toorop/php/server_logout",
                    (request) =>
                    {
                        if (request != null)
                        {
                            Debug.Log(request.ToString());
                        }
                    }));
            }));

        StartCoroutine(UpdateServerList());
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
            serverPrefab.GetComponent<TMP_Text>().text = $"SERVER {servers.servers[i].id}";
            RectTransform rect = serverPrefab.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0, 1);
            rect.localPosition = new Vector2(0, 0 - i * serverListGap);
            ServerSelectButton serverSelectButton = serverPrefab.GetComponent<ServerSelectButton>();
            int id = servers.servers[i].id;
            serverSelectButton.OnClickButton += () =>
            {
                StartCoroutine(webRequest.Request<Servers>($"https://studenthome.hku.nl/~yvar.toorop/php/server_login?server_id={id}&password=password",
                    (request) =>
                    {
                        if (request != null)
                        {
                            Debug.Log(request.ToString());
                            if (request.servers.Count > 0) this.SetServer(request.servers[0]);
                        }
                    }));
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


    // TO DO: Replace methods with global place to set client session and server connection values
    public void SetSession(Result newSession)
    {
        mySession = newSession;
    }
    public void SetServer(Server newServer)
    {
        myServer = newServer;
    }
}
