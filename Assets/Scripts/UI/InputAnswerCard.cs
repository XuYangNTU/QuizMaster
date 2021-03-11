using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputAnswerCard : QuestionAnswerCardBase
{
    [SerializeField] private InputField m_inputField;
    [SerializeField] private Button m_submitBtn;



    public InputAnswerCard Init(UnityAction<string> submit_action)
    {
        m_inputField.text = "";
        m_submitBtn.onClick.AddListener(()=> { submit_action.Invoke("a"); });

        m_clickAction =
            () =>
            {
                m_inputField.ActivateInputField();
                m_inputField.MoveTextEnd(true);
            };

        return this;
    }
}
