using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class AppController : MonoBehaviour
{
    private static AppController m_singleton = null;

    [SerializeField] private int m_maxTimeSec;
    [SerializeField] private int m_questionCount;
    [SerializeField] private string m_questionBankDir;

    private List<Question> m_questions;
    private Player m_player;

    private List<Question> m_remainingQuestions;
    private UnityEvent m_onQuestionsLoaded = new UnityEvent();
    public static UnityEvent OnQuestionsLoaded
    {
        get 
        {
            if (m_singleton.m_onQuestionsLoaded == null)
                m_singleton.m_onQuestionsLoaded = new UnityEvent();
            return m_singleton.m_onQuestionsLoaded;
        }
    }

    public static List<Question> Questions
    {
        get { return m_singleton.m_questions; }
    }

    public static int QuestionCount
    {
        get { return m_singleton.m_questionCount; }
    }

    public static float Score
    {
        get { return (1.0f * m_singleton.m_player.correctCount) / (1.0f * m_singleton.m_questionCount); }
    }

    public static Player Player
    {
        get { return m_singleton.m_player; }
    }

    public static int MaxTimeSec
    {
        get { return m_singleton.m_maxTimeSec; }
    }

    public static string QuestionBankDir
    {
        get { return Application.dataPath + "/" + m_singleton.m_questionBankDir; }
    }

    private void Awake()
    {
        if (!m_singleton)
            m_singleton = this;
        else
            DestroyImmediate(this);

        DontDestroyOnLoad(this);
        m_questions = new List<Question>();
        m_remainingQuestions = new List<Question>();
    }

    public static bool GetNextQuestion(out Question question)
    {
        question = Question.Default;
        if (m_singleton.m_remainingQuestions.Count <0)
            return false;

        int index = Random.Range(0, m_singleton.m_remainingQuestions.Count);
        question = m_singleton.m_remainingQuestions[index];
        m_singleton.m_remainingQuestions.Remove(question);
        return true;
    }


    public static void InitPlayer()
    {
        m_singleton.m_player = new Player()
        {
            correctCount = 0,
            questionCount = 0,
        };
    }


    public static void LoadQuestionBank(string file)
    {
        string path = QuestionBankDir + "/" + file;
        if (!File.Exists(path))
            return;
        m_singleton.m_questions.Clear();
        using (StreamReader stream_reader = new StreamReader(path))
        {
            while (!stream_reader.EndOfStream)
            {
                string[] line = stream_reader.ReadLine().Split('\t');
                if (line[0] == "ID")
                    continue;
                Question question = new Question()
                {
                    id = line[0],
                    primary = line[1],
                    content = line[2],
                    answers = new string[] { line[3], line[4], line[5], line[6] },
                    correctAnswers = new string[] { line[7] },
                    catergory = line[8],
                    questionType = line[9] == "Yes" ? QuestionType.MCQ : QuestionType.BLANK,
                };
                m_singleton.m_questions.Add(question);
                m_singleton.m_remainingQuestions.Add(question);
            }
            m_singleton.m_onQuestionsLoaded.Invoke();
        }
    }

}
