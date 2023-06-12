using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Slider usernameValidationSlider;
    private float usernameValidationVelocity = 0;
    private float usernameSliderTarget = 0;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Slider passwordValidationSlider;
    private float passwordValidationVelocity = 0;
    private float passwordSliderTarget = 0;
    [SerializeField] private float smoothTime;

    [SerializeField] private ButtonAction onClickLoginButton;

    private void Start()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        SetScrollViewContent();

        onClickLoginButton.OnClickButton += OnClickLogin;
    }

    private void Update()
    {
        usernameValidationSlider.value = Mathf.SmoothDamp(usernameValidationSlider.value, usernameSliderTarget, ref usernameValidationVelocity, smoothTime);
        passwordValidationSlider.value = Mathf.SmoothDamp(passwordValidationSlider.value, passwordSliderTarget, ref passwordValidationVelocity, smoothTime);
    }

    public void OnClickLogin()
    {
        bool valid = true;
        if (usernameInputField.text.Length == 0)
        {
            SetSliderTarget(out usernameSliderTarget, 1);
            valid = false;
        }
        if (passwordInputField.text.Length == 0)
        {
            SetSliderTarget(out passwordSliderTarget, 1);
            valid = false;
        }

        if (!valid) return;
        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/user_login?username={usernameInputField.text}&password={passwordInputField.text}",
            (request) =>
            {
                if (request != null)
                {
                    if (request.results.Count > 0)
                    {
                        onClickLoginButton.PredicateAction(Convert.ToBoolean(request.results[0].code));
                    }
                }
            }
        ));
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