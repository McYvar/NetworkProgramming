using System;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    [SerializeField] private int trapId;
    [SerializeField] private float cooldown;
    [SerializeField] TMPro.TMP_Text cooldownText;
    private float timer;

    public void ActivateTrapButton()
    {
        if (timer > 0) return;
        timer = cooldown;
        SessionVariables.instance.myGameClient.SendToServer(new Net_ActivateTrap(trapId));
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownText.text = $"COOLDOWN: {Math.Round(timer, 2)}";
        }
        else cooldownText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            inputHandler.pressInteractFirst += ActivateTrapButton;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            inputHandler.pressInteractFirst -= ActivateTrapButton;
        }
    }
}
