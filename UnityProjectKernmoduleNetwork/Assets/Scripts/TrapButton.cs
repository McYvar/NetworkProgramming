using System;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    [SerializeField] private int trapId;
    [SerializeField] private float cooldown;
    [SerializeField] TMPro.TMP_Text cooldownText;
    private float timer;

    private List<InputHandler> inputHandlers = new List<InputHandler>();
    private List<IGravity> gravityObjects = new List<IGravity>();

    [SerializeField] private Collider myCollider;

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


        for (int i = 0; i < inputHandlers.Count; i++)
        {
            if (!myCollider.bounds.Intersects(gravityObjects[i].GetBounds()))
            {
                inputHandlers[i].pressInteractFirst -= ActivateTrapButton;
                inputHandlers.RemoveAt(i);
                gravityObjects.RemoveAt(i);
                --i;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IGravity obj = other.GetComponent<IGravity>();
        if (obj != null)
        {
            InputHandler inputHandler = other.GetComponent<InputHandler>();
            if (inputHandler != null)
            {
                inputHandler.pressInteractFirst += ActivateTrapButton;
                inputHandlers.Add(inputHandler);
                gravityObjects.Add(obj);
            }
        }
    }
}
