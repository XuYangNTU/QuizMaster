using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Question
{
    public string id;
    public string primary;
    public string content;
    public QuestionType questionType;
    public string[] answers;
    public string[] correctAnswers;
    public float difficulty;
    public int marks;
    public string subject;
    public string catergory;
    public string[] attachments;

    public static Question Default
    {
        get
        {
            return new Question()
            {
                id = "",
                content = "",
                questionType = QuestionType.MCQ,
                answers = new string[] { "Answer_A", "Answer_B", "Answer_C", "Answer_D", },
                correctAnswers = new string[] { "A" },
                difficulty = 1,
                marks = 1,
                subject = "Any",
                catergory = "NA",
                attachments = null,
            };
        }
    }
}
