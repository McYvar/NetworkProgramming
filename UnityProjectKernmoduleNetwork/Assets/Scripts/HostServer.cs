using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HostServer : MonoBehaviour
{
    [SerializeField] private ServerRequest serverRequest;
    [SerializeField] private TMP_InputField serverNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Toggle hostLocalToggle;
    private bool hostLocal = true;

    [SerializeField] private GameObject BaseServerPrefab;

    public void OnHostButton()
    {
        // TO DO: when clicking the button request to host server (create a server) on the given ip


        BaseServer server = Instantiate(BaseServerPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseServer>();
    }
}
