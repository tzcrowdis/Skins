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


    void Start()
    {
        canvasList = new List<Canvas>();
        canvasList.Add(playCanvas);
        canvasList.Add(collectionCanvas);
        canvasList.Add(battlepassCanvas);
        canvasList.Add(storeCanvas);

        playBtn.onClick.AddListener(delegate { OpenCanvas(playCanvas); });
        collectionBtn.onClick.AddListener(delegate { OpenCanvas(collectionCanvas); });
        battlepassBtn.onClick.AddListener(delegate { OpenCanvas(battlepassCanvas); });
        storeBtn.onClick.AddListener(delegate { OpenCanvas(storeCanvas); });

        OpenCanvas(collectionCanvas);
    }

    void OpenCanvas(Canvas canvas)
    {
        foreach (Canvas c in canvasList)
        {
            if (c == canvas)
                c.gameObject.SetActive(true);
            else
                c.gameObject.SetActive(false);
        }
    }
}
