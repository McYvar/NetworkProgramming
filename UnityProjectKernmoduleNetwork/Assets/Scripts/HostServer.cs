using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HostServer : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private ServerList serverList;
    [SerializeField] private TMP_InputField serverNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Toggle hostLocalToggle;
    private ushort hostLocal = 1; // 0 false, 1 true

    [SerializeField] private GameObject BaseServerPrefab;

    public void OnHostButton()
    {
        // TO DO: when clicking the button request to host server (create a server) on the given ip
        StartCoroutine(webRequest.Request<Servers>($"https://studenthome.hku.nl/~yvar.toorop/php/server_create_server?ip={ipInputField.text}&local={hostLocal}&server_name={serverNameInputField.text}&password={passwordInputField.text}",
            (request) =>
            {
                if (request != null)
                {
                    Debug.Log(request.ToString());
                    if (request.servers.Count > 0) this.serverList.SetServer(request.servers[0]);
                }
            }));
        string uri = $"https://studenthome.hku.nl/~yvar.toorop/php/server_create_server?ip={ipInputField.text}&local={hostLocal}&server_name={serverNameInputField.text}&password={passwordInputField.text}";
        
        //BaseServer server = Instantiate(BaseServerPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseServer>();
    }

    public void SetHostLocal(bool value)
    {
        if (value) hostLocal = 1;
        else hostLocal = 0;
    }
}
