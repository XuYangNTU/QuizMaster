using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum QuestionType
{
    MCQ,
    BLANK,
}

public class UIController : MonoBehaviour
{
    #region references

    [SerializeField] private TextAnswerCard m_textAnswerCardPref;
    [SerializeField] private ImageAnswerCard m_imageAswerCardPref;
    [SerializeField] private InputAnswerCard m_inputAnswerCardPref;

    [SerializeField] private GameObject m_startPage;
    [SerializeField] private GameObject m_questionPage;
    [SerializeField] private GameObject m_endPage;
    [SerializeField] private GameObject m_blockPage;

    //start page
    [SerializeField] private Button m_startButton;
    [SerializeField] private PaperSelectionPanelScript m_paperSelectionPanel;


    //question page
    [SerializeField] private Transform m_MCQAnswerHolder;
    [SerializeField] private Transform m_blankAnswerHolder;
    [SerializeField] private Button m_endButtonQuestionPage;
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Text m_timeLeftText;
    [SerializeField] private Text m_quizProgressText;
    [SerializeField] private RectTransform m_progressBarFill;
    [SerializeField] private RectTransform m_progressBar;
    [SerializeField] private Text m_questionContentText;

    [SerializeField] private AnswerFeedbackPanelScript m_answerFeedbackPanel; 

    //end page
    [SerializeField] private Text m_scoreText;
    [SerializeField] private Text m_progressText;
    [SerializeField] private Button m_endButtonEndPage;
    [SerializeField] private Button m_retryButton;





    #endregion
    private bool m_timerFlag;
    private float m_startTime;
    private int m_secElapsed;
    private List<GameObject> m_pageRef;


    private void Awake()
    {
        InitUI();
    }

    private void Start()
    {
        m_pageRef = new List<GameObject>();
        m_pageRef.Add(m_startPage);
        m_pageRef.Add(m_questionPage);
        m_pageRef.Add(m_endPage);
        SetActivePage(m_startPage);
        m_paperSelectionPanel.PopulatePanel(AppController.QuestionBankDir);
    }


    private void Update()
    {
        if (m_timerFlag)
        {
            m_secElapsed = Mathf.FloorToInt(Time.time - m_startTime);
            UpdateQuizInfoUI();
        }
    }


    private void InitUI()
    {
        m_startButton.onClick.AddListener(OnStartBtnPressed);

        m_continueButton.onClick.AddListener(() =>
        {
            if (AppController.Player.questionCount < AppController.QuestionCount)
                DisplayNextQuestion();
            else
                EndQuiz();
        });

        m_retryButton.onClick.AddListener(() => { SetActivePage(m_startPage); });

        m_endButtonQuestionPage.onClick.AddListener(EndQuiz);
    }

    private void SetActivePage(GameObject page)
    {
        for (int i = 0; i < m_pageRef.Count; i++)
        {
            m_pageRef[i].SetActive(m_pageRef[i] == page);
        }
    }


    private void DisplayQuestionAnswers(Question question)
    {
        m_MCQAnswerHolder.gameObject.SetActive(false);
        m_blankAnswerHolder.gameObject.SetActive(false);
        switch (question.questionType)
        {
            case QuestionType.MCQ:
                int child_count = m_MCQAnswerHolder.childCount;
                for (int i = 0; i < child_count; i++)
                {
                    Destroy(m_MCQAnswerHolder.GetChild(i).gameObject);
                }
                for (int i = 0; i < question.answers.Length; i++)
                {
                    TextAnswerCard answercard = Instantiate(m_textAnswerCardPref, m_MCQAnswerHolder).Init(i, question.answers[i]);
                    answercard.SetClickAction(
                    () =>
                    {
                        char c = question.correctAnswers[0][0];
                        bool correct = (char.ToUpper(c) - 65) == answercard.Option; 
                        if (correct)
                            AppController.Player.correctCount++;
                        m_answerFeedbackPanel.ShowFeedbackImage(correct, 0.15f);
                        if (AppController.Player.questionCount >= AppController.QuestionCount)
                            EndQuiz();
                        else
                            DisplayNextQuestion();
                    });
                }
                m_MCQAnswerHolder.gameObject.SetActive(true);
                break;
            case QuestionType.BLANK:
                Instantiate(m_inputAnswerCardPref, m_blankAnswerHolder).Init(
                    (string answer)=> {
                        bool correct = answer == question.answers[0];
                        if (correct)
                            AppController.Player.correctCount++;
                        m_answerFeedbackPanel.ShowFeedbackImage(correct,0.15f);
                        if (AppController.Player.questionCount >= AppController.QuestionCount)
                            EndQuiz();
                        else
                            DisplayNextQuestion();
                    });
                m_blankAnswerHolder.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        AppController.Player.questionCount++;
    }

    private void DisplayEndPage()
    {
        m_scoreText.text = AppController.Score.ToString("P0");
        m_progressText.text = AppController.Player.correctCount.ToString() + " of " + AppController.QuestionCount;
        SetActivePage(m_endPage);
    }


    private bool DisplayNextQuestion()
    {
        if (!AppController.GetNextQuestion(out Question next_question))
            return false;

        m_questionContentText.text = next_question.content;
        DisplayQuestionAnswers(next_question);
        return true;
    }


    private void EndQuiz()
    {
        AppController.OnQuestionsLoaded.RemoveListener(OnQuestionsLoaded);
        m_timerFlag = false;
        DisplayEndPage();
    }

    private void OnStartBtnPressed()
    {
        AppController.OnQuestionsLoaded.AddListener(OnQuestionsLoaded);
        AppController.InitPlayer();
        AppController.LoadQuestionBank(m_paperSelectionPanel.SelectedFile);
    }


    private void OnQuestionsLoaded()
    {
        if (!DisplayNextQuestion())
            return;
        SetActivePage(m_questionPage);
        m_timerFlag = true;
        m_secElapsed = 0;
        m_startTime = Time.time;
    }

    private void UpdateQuizInfoUI()
    {
        int secLeft = AppController.MaxTimeSec - m_secElapsed;
        m_timeLeftText.text = (secLeft / 60).ToString() + "m" + (secLeft % 60).ToString() + "s";
        m_quizProgressText.text = AppController.Player.questionCount.ToString() + " of " + AppController.QuestionCount;
        float delta = m_secElapsed * 1.0f / (1.0f*AppController.MaxTimeSec);
        m_progressBarFill.sizeDelta = new Vector2(-delta* m_progressBar.rect.size.x, 0);
        if (secLeft<=0)
            EndQuiz();
    }


}
