using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [Header("Play")]
    public Canvas playCanvas;
    
    [Header("Ready Up")]
    public Button readyUpBtn;
    public Canvas readyUpCanvas;

    [Header("Collection")]
    public Button collectionBtn;
    public Canvas collectionCanvas;

    [Header("Battlepass")]
    public Button battlepassBtn;
    public Canvas battlepassCanvas;

    [Header("Store")]
    public Button storeBtn;
    public Canvas storeCanvas;

    [Header("Quit")]
    public Button quitButton;

    List<Canvas> canvasList;
    List<Button> buttonList;


    public static Home instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;


        canvasList = new List<Canvas>();
        canvasList.Add(playCanvas);
        canvasList.Add(readyUpCanvas);
        canvasList.Add(collectionCanvas);
        canvasList.Add(battlepassCanvas);
        canvasList.Add(storeCanvas);

        buttonList = new List<Button>();
        readyUpBtn.onClick.AddListener(delegate { OpenCanvas(readyUpCanvas, readyUpBtn); });
        buttonList.Add(readyUpBtn);
        collectionBtn.onClick.AddListener(delegate { OpenCanvas(collectionCanvas, collectionBtn); });
        buttonList.Add(collectionBtn);
        battlepassBtn.onClick.AddListener(delegate { OpenCanvas(battlepassCanvas, battlepassBtn); });
        buttonList.Add(battlepassBtn);
        storeBtn.onClick.AddListener(delegate { OpenCanvas(storeCanvas, storeBtn); });
        buttonList.Add(storeBtn);

        quitButton.onClick.AddListener(QuitGame);
    }

    void Start()
    {
        OpenCanvas(collectionCanvas, collectionBtn);
    }

    public void OpenCanvas(Canvas canvas, Button navBtn = null)
    {
        foreach (Canvas c in canvasList)
        {
            if (c == canvas)
                c.gameObject.SetActive(true);
            else
                c.gameObject.SetActive(false);
        }

        if (navBtn)
        {
            foreach (Button btn in buttonList)
            {
                if (btn == navBtn)
                    btn.interactable = false;
                else
                    btn.interactable = true;
            }
        }
    }

    public void OpenCoinStore()
    {
        OpenCanvas(Home.instance.storeCanvas, Home.instance.storeBtn);
        Store.instance.OpenSubPanel(Store.instance.coinPanel, Store.instance.coinButton);
    }

    public void FastForward()
    {
        Time.timeScale = 10.0f;
    }

    public void NormalTime()
    {
        Time.timeScale = 1.0f;
    }

    void QuitGame()
    {
        MainMenu.instance.QuitToMainMenu();
    }
}
