using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidationMessage : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float lifeTime;
    [SerializeField] private float value;
    [SerializeField, Range(0f, 1f)] private float smoothTime;
    [SerializeField] private TMP_Text message;
    private float velocity;
    private float timer;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            value = 0;
        }

        slider.value = Mathf.SmoothDamp(slider.value, value, ref velocity, smoothTime);
    }

    public void ActivateMessage()
    {
        value = 1;
        timer = lifeTime; 
    }

    public void ForceCancelMessage()
    {
        timer = 0;
    }

    public void SetMessage(string message)
    {
        this.message.text = message;
    }
}
