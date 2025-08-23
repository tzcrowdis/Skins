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
        if (Play.instance.expGain >= EnemyController.instance.GetBossExpThreshold())
        {
            winLose.text = "WIN";
            gameExplanation.text = $"you gained {Play.instance.expGain}xp surpassing the {EnemyController.instance.GetBossExpThreshold()}xp threshold!";
        }
        else
        {
            winLose.text = "LOSE";
            gameExplanation.text = $"you gained {Play.instance.expGain}xp but needed to get {EnemyController.instance.GetBossExpThreshold()}xp...";
        }
        
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
