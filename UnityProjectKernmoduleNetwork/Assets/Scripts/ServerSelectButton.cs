using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServerSelectButton : MonoBehaviour, IPointerClickHandler
{
    public Action OnClickButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickButton?.Invoke();
    }
}
