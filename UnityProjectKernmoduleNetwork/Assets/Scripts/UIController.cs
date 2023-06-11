using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private ResultRequest resultRequest;
    [SerializeField] private ServerRequest selectServer;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Slider usernameValidationSlider;
    private float usernameValidationVelocity = 0;
    private float usernameSliderTarget = 0;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Slider passwordValidationSlider;
    private float passwordValidationVelocity = 0;
    private float passwordSliderTarget = 0;
    [SerializeField] private float smoothTime;

    [SerializeField] private RectTransform scrollViewContent;
    [SerializeField] private GameObject serverListItemPrefab;
    [SerializeField] private float serverListGap;
    private bool updatingList;

    private List<GameObject> serverPrefabs = new List<GameObject>();

    private void Start()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        StartCoroutine(UpdateServerList());
        SetScrollViewContent();
    }

    private void Update()
    {
        usernameValidationSlider.value = Mathf.SmoothDamp(usernameValidationSlider.value, usernameSliderTarget, ref usernameValidationVelocity, smoothTime);
        passwordValidationSlider.value = Mathf.SmoothDamp(passwordValidationSlider.value, passwordSliderTarget, ref passwordValidationVelocity, smoothTime);

        UpdateListSize();
    }

    public void OnClickLogin()
    {
        bool valid = true;
        if (usernameInput.text.Length == 0)
        {
            SetSliderTarget(out usernameSliderTarget, 1);
            valid = false;
        }
        if (passwordInput.text.Length == 0)
        {
            SetSliderTarget(out passwordSliderTarget, 1);
            valid = false;
        }

        if (!valid) return;
        StartCoroutine(resultRequest.GetResultRequest($"https://studenthome.hku.nl/~yvar.toorop/php/user_login?username={usernameInput.text}&password={passwordInput.text}"));
    }

    public void OnEditUsername()
    {
        SetSliderTarget(out usernameSliderTarget, 0);
    }

    public void OnEditPassword()
    {
        SetSliderTarget(out passwordSliderTarget, 0);
    }

    private void SetSliderTarget(out float target, float newTarget)
    {
        target = newTarget;
    }

    private void SetScrollViewContent()
    {

    }

    public IEnumerator UpdateServerList()
    {
        updatingList = true;
        selectServer.UpdateServerList();
        yield return new WaitForSecondsRealtime(3);

        while (serverPrefabs.Count > 0)
        {
            Destroy(serverPrefabs[0]);
            serverPrefabs.RemoveAt(0);
        }

        for (int i = 0; i < selectServer.servers.Count; i++)
        {
            GameObject serverPrefab = Instantiate(serverListItemPrefab, scrollViewContent.transform);
            serverPrefab.GetComponent<TMP_Text>().text = $"SERVER {selectServer.servers[i].server_id}";
            RectTransform rect = serverPrefab.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0, 1);
            rect.localPosition = new Vector2(0, 0 - i * serverListGap);
            ServerSelectButton serverSelectButton = serverPrefab.GetComponent<ServerSelectButton>();
            int id = selectServer.servers[i].server_id;
            serverSelectButton.OnClickButton += () =>
            {
                Debug.Log(i);
                selectServer.SetServer(id);
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

    public void OpenNew(GameObject newWindow)
    {
        newWindow.SetActive(true);
    }

    public void CloseOld(GameObject oldWindow)
    {
        oldWindow.SetActive(false);
    }
}