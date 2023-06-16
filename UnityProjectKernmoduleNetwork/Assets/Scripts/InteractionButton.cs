using System;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    private Action onButtonPress;

    public void SubScribeAction(Action callback)
    {
        onButtonPress += callback;
    }
    public void UnsubScribeAction(Action callback)
    {
        onButtonPress -= callback;
    }

    private void OnPress()
    {
        onButtonPress?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            inputHandler.pressInteractFirst += OnPress;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            inputHandler.pressInteractFirst -= OnPress;
        }
    }
}