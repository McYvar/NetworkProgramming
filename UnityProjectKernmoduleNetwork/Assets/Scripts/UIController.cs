using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private WebRequest webRequest;
    // login
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private ValidationMessage usernameValidation;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private ValidationMessage passwordValidation;
    [SerializeField] private ButtonAction onClickLoginButton;
    // sign up
    [SerializeField] private TMP_InputField signUpUsernameInputField;
    [SerializeField] private ValidationMessage signUpUsernameValidation;
    [SerializeField] private TMP_InputField signUpPasswordInputField;
    [SerializeField] private ValidationMessage signUpPasswordValidation;
    [SerializeField] private TMP_InputField signUpEmailInputField;
    [SerializeField] private ValidationMessage signUpEmailValidation;
    [SerializeField] private ButtonAction onClickSignUpButton;


    private void Start()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        signUpPasswordInputField.contentType = TMP_InputField.ContentType.Password;

        onClickLoginButton.OnClickButton += OnClickLogin;
        onClickSignUpButton.OnClickButton += OnClickSignUp;
    }

    public void OnClickLogin()
    {
        bool valid = true;
        if (usernameInputField.text.Length == 0)
        {
            usernameValidation.ActivateMessage();
            valid = false;
        }
        if (passwordInputField.text.Length == 0)
        {
            passwordValidation.ActivateMessage();
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
                        bool predicate = Convert.ToBoolean(request.results[0].code);
                        if (predicate)
                        {
                            SessionVariables.instance.sessionId = request.results[0].session_id;
                            SessionVariables.instance.myPlayerId = request.results[0].user_id;
                            SessionVariables.instance.myPlayerName = request.results[0].username;
                        }


                        onClickLoginButton.PredicateAction(predicate);
                    }
                }
            }
        ));
    }

    public void OnClickSignUp()
    {
        bool valid = true;
        if (signUpUsernameInputField.text.Length == 0)
        {
            signUpUsernameValidation.ActivateMessage();
            valid = false;
        }
        if (signUpPasswordInputField.text.Length == 0)
        {
            signUpPasswordValidation.ActivateMessage();
            valid = false;
        }
        if (signUpEmailInputField.text.Length == 0)
        {
            signUpEmailValidation.ActivateMessage();
            valid = false;
        }
        if (!valid) return;

        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/user_add_user?email={signUpEmailInputField.text}&username={signUpUsernameInputField.text}&password={signUpPasswordInputField.text}",
            (request) =>
            {
                if (request != null)
                {
                    if (request.results.Count > 0)
                    {
                        bool predicate = Convert.ToBoolean(request.results[0].code);
                        if (predicate)
                        {
                            SessionVariables.instance.sessionId = request.results[0].session_id;
                            SessionVariables.instance.myPlayerId = request.results[0].user_id;
                            SessionVariables.instance.myPlayerName = request.results[0].username;
                        }


                        onClickSignUpButton.PredicateAction(predicate);
                    }
                }
            }
        ));
    }

    public void OnClickUserLogout()
    {
        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/user_logout",
            (request) =>
            {
                if (request != null)
                {
                    if (request.results.Count > 0)
                    {
                        //onClickSignUpButton.PredicateAction(predicate);
                    }
                }
            }
        ));
    }

    public void OnClickServerLogout()
    {
        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/server_logout",
            (request) =>
            {
                if (request != null)
                {
                    if (request.results.Count > 0)
                    {
                        //onClickSignUpButton.PredicateAction(predicate);
                    }
                }
            }
        ));
    }

    public void OnClickCompleteLogout()
    {
        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/server_logout",
            (request) =>
            {
                StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/user_logout",
                    (request) =>
                    {
                        if (request != null)
                        {
                            if (request.results.Count > 0)
                            {
                                //onClickSignUpButton.PredicateAction(predicate);
                            }
                        }
                    }
                ));
            }
        ));
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