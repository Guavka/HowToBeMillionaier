using System;
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
    private Button[] _tipButtons;

    [Header("Questions")]
    [SerializeField]
    private Question[] _qustions;

    [Header("Test")]
    [SerializeField]
    private byte _currentIndex = 0;


    private void Start()
    {
        SetQuestion();
        for (byte i = 0; i < _actionButtons.Length; i++)
        {
            var copyI = i;
            _actionButtons[i].onClick.AddListener(() => OnActionClick(copyI));
        }
    }

    private void EndGame()
    {
        _questionText.text = "Вы победили. начать игру с начала?";

        for (byte i = 0; i < _actionButtons.Length; i++)
        {
            _actionButtons[i].gameObject.SetActive(false);
        }
        for (byte i = 0; i < _tipButtons.Length; i++)
        {
            _tipButtons[i].gameObject.SetActive(false);
        }

        _tipButtons[1].gameObject.SetActive(true);
        _tipButtons[1].GetComponentInChildren<TMP_Text>().text = "Начать заново";
        _tipButtons[1].onClick.RemoveAllListeners();
        _tipButtons[1].onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
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
        var currentQuestion = _qustions[_currentIndex];

        _questionText.text = currentQuestion.QuestionText;

        for (var i = 0; i < _actionText.Length; i++)
        {
            _actionText[i].text = currentQuestion.Answers[i];
        }
    }
}
