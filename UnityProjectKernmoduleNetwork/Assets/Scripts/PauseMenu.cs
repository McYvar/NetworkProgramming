using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private ButtonAction buttonAction;

    public static PauseMenu instance;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(instance);
                instance = null;
            }
        }

        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputField.text = Math.Round(slider.value, 2).ToString();
    }

    public void OnSliderValueChanged(float value)
    {
        GlobalSettings.SetSensitivity(value);
        inputField.text = Math.Round(value, 2).ToString();
    }

    public void OnDirectValueChanged(string text)
    {
        float value = float.Parse(text);
        slider.value = value;
        GlobalSettings.SetSensitivity(value);
    }

    public void SubscribeToButton(Action callback)
    {
        buttonAction.OnClickButton += callback;
    }

    public void UnsubscribeFromButton(Action callback)
    {
        buttonAction.OnClickButton -= callback;
    }

    public void OnClickDisconnect(int scene)
    {
        StartCoroutine(webRequest.Request<Server>("https://studenthome.hku.nl/~yvar.toorop/php/server_logout", (request) =>
        {
            if (request != null)
            {
                StartCoroutine(webRequest.Request<Results>("studenthome.hku.nl/~yvar.toorop/php/user_logout", (request2) =>
                {
                    if (request2 != null)
                    {
                        if (SessionVariables.instance.server != null)
                        {
                            SessionVariables.instance.myGameClient.SendToServer(new Net_Disconnect(SessionVariables.instance.myPlayerId));
                            Destroy(SessionVariables.instance.server.gameObject);
                            SessionVariables.instance.server = null;
                        }
                        if (SessionVariables.instance.myGameClient != null)
                        {
                            Destroy(SessionVariables.instance.myGameClient.gameObject);
                            SessionVariables.instance.myGameClient = null;
                        }
                        SessionVariables.instance.playerDictionary.Clear();
                        SessionVariables.instance.myGameClient = null;
                        SessionVariables.instance.connected = false;
                        SessionVariables.instance.serverId = -1;
                        SessionVariables.instance.myPlayerId = -1;
                        SessionVariables.instance.myPlayerName = null;
                        SceneManager.LoadScene(scene);
                    }
                }));
            }
        }));
    }
}
