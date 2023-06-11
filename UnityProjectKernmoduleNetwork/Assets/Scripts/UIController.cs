using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Slider usernameValidationSlider;
    private float usernameValidationVelocity = 0;
    private float usernameSliderTarget = 0;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Slider passwordValidationSlider;
    private float passwordValidationVelocity = 0;
    private float passwordSliderTarget = 0;
    [SerializeField] private float smoothTime;

    private void Start()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        SetScrollViewContent();
    }

    private void Update()
    {
        usernameValidationSlider.value = Mathf.SmoothDamp(usernameValidationSlider.value, usernameSliderTarget, ref usernameValidationVelocity, smoothTime);
        passwordValidationSlider.value = Mathf.SmoothDamp(passwordValidationSlider.value, passwordSliderTarget, ref passwordValidationVelocity, smoothTime);
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
        //StartCoroutine(resultRequest.Request($"https://studenthome.hku.nl/~yvar.toorop/php/user_login?username={usernameInput.text}&password={passwordInput.text}"));
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


    public void OpenNew(GameObject newWindow)
    {
        newWindow.SetActive(true);
    }

    public void CloseOld(GameObject oldWindow)
    {
        oldWindow.SetActive(false);
    }
}