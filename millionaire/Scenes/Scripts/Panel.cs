using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public GameObject menuPanel;    
    public GameObject rulesPanel;
    public GameObject gamePanel;
    public GameObject endPanel;
    public Button buttonGame;
    public Button buttonrules;
    public Button back;
    public Button back1;
    public Button end;

    void Start()
    {
        ShowMenu();
        buttonGame.onClick.AddListener(StartGame);
        buttonrules.onClick.AddListener(RulesGame);
        back.onClick.AddListener(ShowMenu);
        back1.onClick.AddListener(ShowMenu);
        end.onClick.AddListener(EndGame);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        rulesPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        rulesPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void RulesGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        rulesPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    public void EndGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        rulesPanel.SetActive(false);
        endPanel.SetActive(true);
    }
}
