using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [Header("End Match")]
    public Button endMatchButton;

    [Header("Post Match Summary")]
    public GameObject postMatchSummaryPanel;
    public Button returnHomeButton;
    
    
    void Start()
    {
        endMatchButton.onClick.AddListener(EndMatch);

        postMatchSummaryPanel.SetActive(false);
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    void EndMatch()
    {
        endMatchButton.gameObject.SetActive(false);
        PopulatePostMatchSummary();
        postMatchSummaryPanel.SetActive(true);
    }

    void PopulatePostMatchSummary()
    {
        // TODO

        // + coins?

        // + exp for battlepass?
    }

    void ReturnHome()
    {
        endMatchButton.gameObject.SetActive(true);
        postMatchSummaryPanel.SetActive(false);
        gameObject.SetActive(false);
        Home.instance.gameObject.SetActive(true);
        Home.instance.OpenCanvas(Home.instance.collectionCanvas, Home.instance.collectionBtn);
        Store.instance.RandomizeFeaturedStore();
    }
}
