using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonAction : MonoBehaviour, IPointerClickHandler
{
    public Action OnClickButton;
    [SerializeField] private UnityEvent OnTrueEvent;
    [SerializeField] private UnityEvent OnFalseEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickButton?.Invoke();
    }

    public void PredicateAction(bool predicate)
    {
        if (predicate) OnTrueEvent?.Invoke();
        else OnFalseEvent?.Invoke();
    }
}
