using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [Header("Play")]
    public Button playBtn;
    public Canvas playCanvas;

    [Header("Collection")]
    public Button collectionBtn;
    public Canvas collectionCanvas;

    [Header("Battlepass")]
    public Button battlepassBtn;
    public Canvas battlepassCanvas;

    [Header("Store")]
    public Button storeBtn;
    public Canvas storeCanvas;

    List<Canvas> canvasList;
    List<Button> buttonList;


    public static Home instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        canvasList = new List<Canvas>();
        canvasList.Add(playCanvas);
        canvasList.Add(collectionCanvas);
        canvasList.Add(battlepassCanvas);
        canvasList.Add(storeCanvas);

        buttonList = new List<Button>();
        playBtn.onClick.AddListener(delegate { OpenCanvas(playCanvas, playBtn); });
        buttonList.Add(playBtn);
        collectionBtn.onClick.AddListener(delegate { OpenCanvas(collectionCanvas, collectionBtn); });
        buttonList.Add(collectionBtn);
        battlepassBtn.onClick.AddListener(delegate { OpenCanvas(battlepassCanvas, battlepassBtn); });
        buttonList.Add(battlepassBtn);
        storeBtn.onClick.AddListener(delegate { OpenCanvas(storeCanvas, storeBtn); });
        buttonList.Add(storeBtn);

        OpenCanvas(collectionCanvas, collectionBtn);
    }

    public void OpenCanvas(Canvas canvas, Button navBtn)
    {
        foreach (Canvas c in canvasList)
        {
            if (c == canvas)
                c.gameObject.SetActive(true);
            else
                c.gameObject.SetActive(false);
        }

        foreach (Button btn in buttonList)
        {
            if (btn == navBtn)
                btn.interactable = false;
            else
                btn.interactable = true;
        }
    }
}
