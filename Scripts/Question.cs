using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MillionaireGame : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Button stopButton, fiftyFiftyButton, phoneAFriendButton, peopleButton;
    public Panel panel;
    public GameObject imageMessage;
    public TextMeshProUGUI message;
    public AudioClip audioRight;
    private AudioSource audioSource;
    public GameResult gameResult; 
    public Image[] circles;
    public Color defaultColor = Color.blue;
    public Color activeColor = Color.magenta;

    private List<QuestionData> questions;
    private int currentQuestionIndex, balance, lastSafeLevel;
    private bool[] aidsUsed = new bool[3];
    private int[] prizeMoney = { 100, 200, 300, 500, 1000, 2000, 4000, 8000, 16000, 32000, 64000, 125000, 250000, 500000, 1000000 };
    private int[] safeLevels = { 4, 9, 14 };

    void Start()
    {
        LoadQuestions();
        InitializeGame();
        AttachButtonListeners();
        imageMessage.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void LoadQuestions()
    {
        questions = new List<QuestionData> {
        new QuestionData { questionText = "Яка столиця Франції?", answers = new string[] { "Берлін", "Париж", "Рим", "Лондон" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Який елемент позначається символом 'O'?", answers = new string[] { "Золото", "Кисень", "Олово", "Водень" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Скільки планет у Сонячній системі?", answers = new string[] { "8", "9", "7", "10" }, correctAnswerIndex = 0 },
            new QuestionData { questionText = "Скільки кольорів у веселці?", answers = new string[] { "5", "6", "7", "8" }, correctAnswerIndex = 2 },
            new QuestionData { questionText = "Яке море не має берегів?", answers = new string[] { "Саргасове", "Чорне", "Азовське", "Карибське" }, correctAnswerIndex = 0 },
            new QuestionData { questionText = "Яка країна виготовляє автомобілі Ferrari?", answers = new string[] { "Франція", "Італія", "Німеччина", "США" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Скільки кісток у людському тілі?", answers = new string[] { "180", "195", "250", "206" }, correctAnswerIndex = 3 },
            new QuestionData { questionText = "Яке озеро найбільше у світі за площею?", answers = new string[] { "Байкал", "Вікторія", "Каспійське море", "Онтаріо" }, correctAnswerIndex = 2 },
            new QuestionData { questionText = "Яка найбільша пустеля у світі?", answers = new string[] { "Калахарі", "Гобі", "Антарктична", "Сахара" }, correctAnswerIndex = 3 },
            new QuestionData { questionText = "Яка найвища гора у світі?", answers = new string[] { "Кіліманджаро", "Мак-Кінлі", "Аннапурна", "Еверест" }, correctAnswerIndex = 3 },
            new QuestionData { questionText = "Яка найменша країна у світі?", answers = new string[] { "Монако", "Ватикан", "Ліхтенштейн", "Мальта" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Яка річка найдовша у світі?", answers = new string[] { "Амазонка", "Ніл", "Конго", "Міссісіпі" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Хто написав 'Гамлета'?", answers = new string[] { "Данте", "Шекспір", "Толстой", "Гете" }, correctAnswerIndex = 1 },
            new QuestionData { questionText = "Скільки континентів на Землі?", answers = new string[] { "5", "6", "7", "8" }, correctAnswerIndex = 2 },
            new QuestionData { questionText = "Який рік заснування Києва?", answers = new string[] { "482", "1240", "988", "1010" }, correctAnswerIndex = 0 },
        };
    }

    void InitializeGame()
    {
        currentQuestionIndex = balance = lastSafeLevel = 0;
        aidsUsed = new bool[] { false, false, false };

        fiftyFiftyButton.interactable = phoneAFriendButton.interactable = peopleButton.interactable = true;
        ResetAnswerButtons();
        ShowQuestion();
        UpdateQuestionLevel();
    }

    void AttachButtonListeners()
    {
        stopButton.onClick.AddListener(EndGame);
        fiftyFiftyButton.onClick.AddListener(() => UseAid(0));
        phoneAFriendButton.onClick.AddListener(() => UseAid(1));
        peopleButton.onClick.AddListener(() => UseAid(2));
    }

    void ResetAnswerButtons()
    {
        foreach (var button in answerButtons)
            button.interactable = true;
    }

    void UseAid(int aidIndex)
    {
        if (aidsUsed[aidIndex]) return;
        aidsUsed[aidIndex] = true;
        var button = aidIndex == 0 ? fiftyFiftyButton : aidIndex == 1 ? phoneAFriendButton : peopleButton;
        button.interactable = false;

        QuestionData q = questions[currentQuestionIndex];
        if (aidIndex == 0)
            UseFiftyFifty(q);
        else if (aidIndex == 1)
            PhoneAFriend(q);
        else
            People(q);
    }

    void UseFiftyFifty(QuestionData q)
    {
        List<int> wrongAnswers = new List<int>();
        for (int i = 0; i < q.answers.Length; i++)
            if (i != q.correctAnswerIndex) wrongAnswers.Add(i);

        int randomWrongAnswer = wrongAnswers[Random.Range(0, wrongAnswers.Count)];
        for (int i = 0; i < answerButtons.Length; i++)
            if (i != q.correctAnswerIndex && i != randomWrongAnswer)
                answerButtons[i].interactable = false;
    }

    void PhoneAFriend(QuestionData q)
    {
        int friendAnswerIndex = (Random.Range(0, 100) < 75) ? q.correctAnswerIndex : GetRandomWrongAnswer(q);
        imageMessage.SetActive(true);
        message.text = $"Друг каже: 'Я думаю, що правильна відповідь - {q.answers[friendAnswerIndex]}'";
    }

    void People(QuestionData q)
    {
        int[] votes = new int[q.answers.Length];
        for (int i = 0; i < 10; i++)
        {
            int vote = (Random.Range(0, 100) < 70) ? q.correctAnswerIndex : GetRandomWrongAnswer(q);
            votes[vote]++;
        }

        string audienceResponse = "Допомога залу:\n";
        for (int i = 0; i < votes.Length; i++)
            audienceResponse += $"{q.answers[i]}: {votes[i]} голосів\n";

        imageMessage.SetActive(true);
        message.text = audienceResponse;
    }

    int GetRandomWrongAnswer(QuestionData q)
    {
        List<int> possibleAnswers = new List<int>();
        for (int i = 0; i < q.answers.Length; i++)
            if (i != q.correctAnswerIndex)
                possibleAnswers.Add(i);
        return possibleAnswers[Random.Range(0, possibleAnswers.Count)];
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            gameResult.ShowResultMessage(balance);
            panel.EndGame();
            InitializeGame();
            return;
        }

        QuestionData q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            Button btn = answerButtons[i];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => AnswerSelected(index));
            btn.interactable = true;
        }
        imageMessage.SetActive(false);
        UpdateQuestionLevel();
    }

    public void AnswerSelected(int index)
    {
        QuestionData q = questions[currentQuestionIndex];
        if (index == q.correctAnswerIndex)
        {
            balance = prizeMoney[currentQuestionIndex];
            if (System.Array.Exists(safeLevels, level => level == currentQuestionIndex))
                lastSafeLevel = balance;

            currentQuestionIndex++;
            audioSource.PlayOneShot(audioRight);
            ShowQuestion();
        }
        else
        {
            balance = lastSafeLevel;
            gameResult.ShowResultMessage(balance);
            InitializeGame();
            panel.EndGame();
        }
    }

    void EndGame()
    {
        gameResult.ShowResultMessage(balance);
        InitializeGame();
    }

    void UpdateQuestionLevel()
    {
        for (int i = 0; i < circles.Length; i++)
            circles[i].color = (i == currentQuestionIndex) ? activeColor : defaultColor;
    }
}
