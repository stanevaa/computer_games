using UnityEngine;
using TMPro;

public class GameResult : MonoBehaviour
{
    public MillionaireGame millionaireGame;
    public TMP_Text resultText;

    public void ShowResultMessage(int balance)
{
    if (balance == 1000000)
    {
        resultText.text = "Вітаємо! Ви виграли $1,000,000!";
    }
    else if (balance > 0)
    {
        resultText.text = $"Ви отримали ${balance}. Бажаємо удачі у наступній грі!";
    }
    else
    {
        resultText.text = "Нажаль, ви програли. Спробуйте ще раз!";
    }
}

}
