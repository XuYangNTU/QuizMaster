using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class TextAnswerCard : QuestionAnswerCardBase
{
    [SerializeField] private Text m_optionText;
    [SerializeField] private Text m_answerText;

    private int m_option = -1;

    public int Option
    {
        get { return m_option; }
    }

    private static char[] optionToAlphabetArray =
    {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
    };


    public TextAnswerCard Init(int option,string text)
    {
        m_option = option;
        m_optionText.text = optionToAlphabetArray[option] + ".";
        m_answerText.text = text;
        return this;
    }

    public void SetClickAction(UnityAction click_action)
    {
        m_clickAction = click_action;
    }

}
