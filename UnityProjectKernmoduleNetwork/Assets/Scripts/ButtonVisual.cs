using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Slider[] loginButtonSliders;
    [SerializeField] private float smoothTime;
    [SerializeField] private float fillspeed;
    private float[] smoothVelocities;
    private float filling;

    private bool hover = false;

    private void OnEnable()
    {
        filling = 0;
        hover = false;
        for (int i = 0; i < loginButtonSliders.Length; i++)
        {
            loginButtonSliders[i].value = 0;
        }
    }

    private void Start()
    {
        smoothVelocities = new float[loginButtonSliders.Length];
        for (int i = 0; i < loginButtonSliders.Length; i++)
        {
            loginButtonSliders[i].maxValue = 1f / (i + 1f);
        }
    }

    private void Update()
    {
        if (hover)
        {
            filling += fillspeed * Time.deltaTime;
        }
        else
        {
            filling -= fillspeed * Time.deltaTime;
        }
        filling = Mathf.Clamp(filling, 0.0f, 1.0f);

        FillSliders();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }

    private void FillSliders()
    {
        for (int i = 0; i < loginButtonSliders.Length; i++)
        {
            loginButtonSliders[i].value = Mathf.SmoothDamp(loginButtonSliders[i].value, filling, ref smoothVelocities[i], smoothTime);
        }
    }
}
