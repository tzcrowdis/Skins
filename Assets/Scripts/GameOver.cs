using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Game Over")]
    public TMP_Text winLose;
    public TMP_Text gameExplanation;
    public Button quitButton;


    public static GameOver instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        quitButton.onClick.AddListener(ReturnToMainMenu);
        
        gameObject.SetActive(false);
    }

    public void PopulateGameExplanation()
    {
        if (Player.instance.level == 10)
            winLose.text = "WIN";
        else
            winLose.text = "LOSE";

        // TODO grade based on how close they were?
        // TODO most played card?
        // TODO favorite collection?
    }

    void ReturnToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenu.instance.QuitToMainMenu();
    }
}
