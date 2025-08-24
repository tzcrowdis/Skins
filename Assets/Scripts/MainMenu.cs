using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    public Button play;
    public Button settings;
    public Button quit;
    public ParticleSystem backgroundParticles;
    [HideInInspector] public GameObject[] openingCanvases;

    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public Button dismissTutorialButton;
    public bool tutorialDismissed = false;


    public static MainMenu instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        play.onClick.AddListener(PlayGame);
        settings.onClick.AddListener(OpenSettings);
        quit.onClick.AddListener(QuitGame);

        openingCanvases = GameObject.FindGameObjectsWithTag("Opening Canvas");
        foreach (GameObject canvas in openingCanvases)
            canvas.SetActive(false);

        Player.instance.seasonTimerLock = true;

        tutorialPanel.SetActive(!tutorialDismissed);
        dismissTutorialButton.onClick.AddListener(DismissTutorial);

        // TESTING (jump into season)
        //gameObject.SetActive(false);
    }

    void PlayGame()
    {
        Player.instance.season = 1;
        Player.instance.level = 0;
        Player.instance.seasonTimerLock = false;
        Player.instance.seasonTimerText.color = Color.white;
        Player.instance.dollars = Random.Range(10, 100); // NOTE starting $ variance
        Player.instance.coins = 0;
        Player.instance.Start();

        Player.instance.ResetModifierList();
        Battlepass.instance.GenerateBattlepassItems();
        Collection.instance.ResetCollection();
        Store.instance.RandomizeFeaturedStore();

        EnemyController.instance.ResetBosses();

        foreach (GameObject canvas in openingCanvases)
            canvas.SetActive(true);

        gameObject.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        EnemyController.instance.bossFight = false;
        MusicController.instance.ChangeMusic(MusicController.Scenario.Default);
        
        var mainParticles = backgroundParticles.main;
        mainParticles.simulationSpeed = 1f;
        mainParticles.startColor = Color.gray;

        Player.instance.seasonTimerLock = true;
        Player.instance.HidePlayer();
        foreach (Transform enemy in EnemyController.instance.transform)
            enemy.gameObject.SetActive(false);

        Home.instance.OpenCanvas(Home.instance.collectionCanvas, Home.instance.collectionBtn);
        foreach (GameObject canvas in openingCanvases)
            canvas.SetActive(false);

        gameObject.SetActive(true);
    }

    void QuitGame()
    {
        SaveManager.instance.SavePlayerSettings();
        
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OpenSettings()
    {
        gameObject.SetActive(false);
        GameSettings.instance.OpenSettings(gameObject);
    }

    void DismissTutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialDismissed = true;
        SaveManager.instance.SavePlayerSettings();
    }
}
