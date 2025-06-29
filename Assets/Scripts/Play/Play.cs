using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [Header("End Match")]
    public Button endMatchButton;

    [Header("Post Match Summary")]
    public GameObject postMatchSummaryPanel;
    public Button returnHomeButton;

    [Header("Exp Display")]
    public Transform expContent;
    public GameObject expSourceText;
    public List<string> expSources;
    public RectTransform expCurrentProgressBar;
    public TMP_Text currentLevel;
    public TMP_Text nextLevel;
    public TMP_Text expText;
    
    
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
        // + exp
        Player.instance.AddExperience(100);

        currentLevel.text = $"{Player.instance.level}";
        nextLevel.text = $"{Player.instance.level + 1}";
        expText.text = $"{Player.instance.exp}/{Player.instance.levelCap}";

        expCurrentProgressBar.anchorMax = new Vector2((float)Player.instance.exp / (float)Player.instance.levelCap, 0.5f);
        expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);

        foreach (Transform t in expContent)
            Destroy(t.gameObject);

        // TODO add reasons for exp somewhere
        foreach (string source in expSources)
        {
            GameObject src = Instantiate(expSourceText, expContent);
            src.GetComponent<TMP_Text>().text = source;
        }
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
