using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class QuestionAnswerCardBase : MonoBehaviour, IPointerClickHandler
{
    protected UnityAction m_clickAction;

    public void OnPointerClick(PointerEventData eventData)
    {
        m_clickAction.Invoke();
    }

}
