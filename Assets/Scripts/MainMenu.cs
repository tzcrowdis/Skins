using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("High Level")]
    public Button play;
    public Button settings;
    public Button quit;
    public ParticleSystem backgroundParticles;
    [HideInInspector] public GameObject[] openingCanvases;

    //[Header("Settings")]



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

        // TESTING (jump into season)
        //gameObject.SetActive(false);
    }

    void PlayGame()
    {
        Player.instance.season = 1;
        Player.instance.level = 0;
        Player.instance.seasonTimerLock = false;
        Player.instance.seasonTimerText.color = Color.white;
        Player.instance.Start();

        Battlepass.instance.GenerateBattlepassItems();
        Collection.instance.ResetCollection();
        Store.instance.RandomizeFeaturedStore();

        foreach (GameObject canvas in openingCanvases)
            canvas.SetActive(true);

        gameObject.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        EnemyController.instance.bossFight = false;
        
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
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OpenSettings()
    {
        Debug.Log("tried to open settings");
    }

    // TODO general
    // TODO graphics
    // TODO audio
}
