using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnswerFeedbackPanelScript : MonoBehaviour
{
    [SerializeField] private GameObject m_correctImage;
    [SerializeField] private GameObject m_wrongImage;


    private float m_startTime;
    private float m_displayDuration;
    private bool m_startFlag;

    private void Awake()
    {
        m_correctImage.SetActive(false);
        m_wrongImage.SetActive(false);
    }

    public void ShowFeedbackImage(bool correct, float duration)
    {
        GameObject obj = correct ? m_correctImage : m_wrongImage;
        obj.SetActive(true);
        m_displayDuration = duration;
        m_startTime = Time.time;
        m_startFlag = true;
    }

    private void Update()
    {
        if (!m_startFlag)
            return;
        if (Time.time - m_startTime>=m_displayDuration)
        {
            m_correctImage.SetActive(false);
            m_wrongImage.SetActive(false);
            m_startFlag = false;
        }
    }
}
