using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class Question
{
    /*
     question = {
        'text': 'Text question',
        'answers': ['Answer1','Answer2','Answer3','Answer4'],
        'correct': 0
    }
    */

    public string QuestionText;
    public string[] Answers;
    [Range(0, 3)]
    public byte CorrectIndex;
}

public class GameController : MonoBehaviour
{
    [Header("Init value")]
    [SerializeField]
    private TMP_Text _questionText;

    [Header("Action buttons")]
    [SerializeField]
    private Button[] _actionButtons;
    [SerializeField]
    private TMP_Text[] _actionText;

    [Header("Tip buttons")]
    [SerializeField]
    private Button _tip50;
    [SerializeField]
    private Button _tipCall;
    [SerializeField]
    private Button _tipPeople;

    [Header("Questions")]
    [SerializeField]
    private Question[] _qustions;

    [Header("Test")]
    [SerializeField]
    private byte _currentIndex = 0;

    private void Start()
    {
        InitListeners();
        SetQuestion();
    }

    private int GetRandomInt(int left, int right)
    {
        var rand = new System.Random();
        var result = rand.Next(left, right);
        return result;
    }

    private void InitListeners()
    {

        for (byte i = 0; i < _actionButtons.Length; i++)
        {
            var copyI = i;
            _actionButtons[i].onClick.AddListener(() => OnActionClick(copyI));
        }

        _tip50.onClick.AddListener(() =>
        {
            var tempList = new List<Button> { _actionButtons[0], _actionButtons[1], _actionButtons[2], _actionButtons[3] };

            var currentQuestionAnswer = _qustions[_currentIndex].CorrectIndex;
            tempList.Remove(tempList[currentQuestionAnswer]);
            tempList.Remove(tempList[GetRandomInt(0, tempList.Count - 1)]);

            for (byte i = 0; i < tempList.Count; i++)
            {
                tempList[i].gameObject.SetActive(false);
            }

            _tip50.enabled = false;
        });

        _tipCall.onClick.AddListener(() =>
        {
            byte intel = 20;

            var randNumber = GetRandomInt(1, 100) < intel ? _qustions[_currentIndex].CorrectIndex + 1 : GetRandomInt(1, 4);

            _questionText.text = $"Думаю это {randNumber}";

            _tipCall.enabled = false;
        });
        _tipPeople.onClick.AddListener(() => { });

    }

    private void EndGame()
    {
        _questionText.text = "Вы победили. начать игру с начала?";

        for (byte i = 0; i < _actionButtons.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(false);
        }

        _tip50.gameObject.SetActive(false);
        _tipPeople.gameObject.SetActive(false);


        _tipCall.gameObject.SetActive(true);
        _tipCall.GetComponentInChildren<TMP_Text>().text = "Начать заново";
        _tipCall.onClick.RemoveAllListeners();
        _tipCall.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    private void OnActionClick(byte index)
    {
        var currentQuestion = _qustions[_currentIndex];
        if (currentQuestion.CorrectIndex != index)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log("Правильно");
        _currentIndex++;

        if (_currentIndex >= _qustions.Length)
            EndGame();
        else
            SetQuestion();
    }

    private void SetQuestion()
    {
        for (byte i = 0; i < _actionButtons.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(true);
        }

        var currentQuestion = _qustions[_currentIndex];

        _questionText.text = currentQuestion.QuestionText;

        for (var i = 0; i < _actionText.Length; i++)
        {
            _actionText[i].text = currentQuestion.Answers[i];
        }
    }
}
