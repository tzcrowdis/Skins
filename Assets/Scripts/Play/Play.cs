using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class Play : MonoBehaviour
{
    [Header("End Match")]
    public Button endMatchButton;

    [Header("Post Match Summary")]
    public GameObject postMatchSummaryPanel;
    public Button skipHomeButton;

    [Header("Exp Display")]
    public float expSpeed;
    public Transform expContent;
    public GameObject expSourceText;
    [HideInInspector] public List<string> expSources;
    public RectTransform expCurrentProgressBar;
    public TMP_Text currentLevel;
    public TMP_Text nextLevel;
    //public TMP_Text expText;
    public TMP_Text expGainText;
    [HideInInspector] public int expGain = 0;
    int prevExpGain;

    [Header("Populate Timing")]
    public float stepT;
    public float delayBtwnSteps;
    float t = 0f;

    // Populate Post Match Summary
    bool postMatchSummary = false;
    bool skip = false;

    // populate progression
    bool baseExpPhase = false;
    bool modifierExpPhase = false;
    int modifierExpPhaseIndex = 0;

    // player exp state
    [HideInInspector] public int playerStartExp;
    [HideInInspector] public int playerStartLevel;
    [HideInInspector] public float displayCurrentExp;
    [HideInInspector] public int displayCurrentLevel;
    [HideInInspector] public int currentLevelCap;
    [HideInInspector] public int negativeStoppingPoint;
    float currentNewExp = 0;


    public static Play instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        endMatchButton.onClick.AddListener(EndMatch);

        postMatchSummaryPanel.SetActive(false);
        skipHomeButton.onClick.AddListener(SkipToEndPostMatchSummary);
        skipHomeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "SKIP";
    }

    void Update()
    {
        if (!skip && displayCurrentLevel == Player.instance.maxLevel)
            SkipToEndPostMatchSummary();
        
        if (skip)
            return;
        
        if (!postMatchSummary)
            return;

        if (baseExpPhase) // doesn't wait to populate
        {
            expGain = 0;
            int posExpGain = GetSkinExp();
            int negExpGain = EnemyController.instance.GetNegativeExp();
            expGain += posExpGain + negExpGain;
            expGainText.text = $"+{expGain}xp";
            
            GameObject src = Instantiate(expSourceText, expContent);
            src.GetComponent<TMP_Text>().text = $"+{posExpGain}xp from skin rarity {ObjectNames.NicifyVariableName(Player.instance.GetSkinRarity().ToString())} and {negExpGain}xp from enemies";

            if (Player.instance.modifiers.Count > 0)
            {
                GameObject tempText = Instantiate(expSourceText, expContent);
                tempText.GetComponent<TMP_Text>().text = "...";
            }

            baseExpPhase = false;
            modifierExpPhase = true;
            modifierExpPhaseIndex = 0;
            expSpeed = expGain / (stepT - delayBtwnSteps);
            prevExpGain = expGain;
            t = 0f;
        }
        else if (modifierExpPhase)
        {
            // early exit if no modifiers
            Modifier mod = null;
            if (Player.instance.modifiers.Count > 0)
                mod = Player.instance.modifiers[modifierExpPhaseIndex];
            else
                modifierExpPhaseIndex = 100;

            if (mod)
            {
                if (mod.modifierType != Modifier.Type.ExpMult & mod.modifierType != Modifier.Type.ExpAdd)
                    t = stepT + 1f;

                // update exp gain
                t += Time.deltaTime;
                if (t >= stepT)
                {
                    if (mod.modifierType == Modifier.Type.ExpMult | mod.modifierType == Modifier.Type.ExpAdd)
                    {
                        bool success = mod.ModifierEffect();
                        if (success)
                        {
                            expGainText.text = $"+{expGain}xp";
                            GameObject src = expContent.GetChild(expContent.childCount - 1).gameObject;
                            src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: {mod.ModifierExpDescription()}";

                            expSpeed = (expGain - prevExpGain) / (stepT - delayBtwnSteps);
                            prevExpGain = expGain;

                            if (expSpeed < 0)
                            {

                            }
                        }
                        else
                        {
                            GameObject src = expContent.GetChild(expContent.childCount - 1).gameObject;
                            src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: failed";
                        }

                        if (Player.instance.modifiers.Count - 1 > modifierExpPhaseIndex)
                        {
                            GameObject tempText = Instantiate(expSourceText, expContent);
                            tempText.GetComponent<TMP_Text>().text = $"...";
                        }
                    }

                    modifierExpPhaseIndex++;
                    t = 0f;
                }
            }

            // final exit
            if (modifierExpPhaseIndex >= Player.instance.modifiers.Count)
            {
                modifierExpPhase = false;
                modifierExpPhaseIndex = 0;
                t = 0f;
            }
        }


        // update exp progress bar
        // early exit
        if ((currentNewExp >= expGain & expSpeed >= 0) | (currentNewExp <= expGain & expSpeed < 0))
        {
            if (!baseExpPhase & !modifierExpPhase)
            {
                skipHomeButton.onClick.RemoveAllListeners();
                skipHomeButton.onClick.AddListener(ReturnHome);
                skipHomeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "HOME";
            }
            return;
        }

        displayCurrentExp += expSpeed * Time.deltaTime;
        currentNewExp += expSpeed * Time.deltaTime;

        // adjust for xp going below 0xp lvl0
        if (displayCurrentExp <= 0 & displayCurrentLevel <= 0 & expSpeed < 0)
        {
            displayCurrentExp = 0;
            displayCurrentLevel = 0;
            currentLevel.text = $"{displayCurrentLevel}";
            nextLevel.text = $"{displayCurrentLevel + 1}";
            t = stepT + 1f; // exit modifier phase early
            return;
        }

        // increment/decrement level
        if (expSpeed > 0 && displayCurrentExp >= currentLevelCap)
        {
            displayCurrentExp = 0f;
            displayCurrentLevel++;
            if (displayCurrentLevel == Player.instance.maxLevel)
                SkipToEndPostMatchSummary();
            currentLevel.text = $"{displayCurrentLevel}";
            nextLevel.text = $"{displayCurrentLevel + 1}";
            currentLevelCap = Player.instance.CalculateLevelCap(displayCurrentLevel);
        }
        else if (displayCurrentExp < 0)
        {
            displayCurrentLevel--;
            currentLevelCap = Player.instance.CalculateLevelCap(displayCurrentLevel);
            displayCurrentExp = currentLevelCap;
            currentLevel.text = $"{displayCurrentLevel}";
            nextLevel.text = $"{displayCurrentLevel + 1}";
        }

        expCurrentProgressBar.anchorMax = new Vector2(displayCurrentExp / (float)currentLevelCap, 0.5f);
        expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);
    }

    void EndMatch()
    {
        endMatchButton.gameObject.SetActive(false);
        EnemyController.instance.EndPlay();
        Player.instance.HidePlayer();
        StartPostMatchSummary();
        postMatchSummaryPanel.SetActive(true);
    }

    void StartPostMatchSummary()
    {
        postMatchSummary = true;

        foreach (Modifier mod in Player.instance.modifiers)
            mod.lockDrag = true;

        playerStartExp = Player.instance.exp;
        playerStartLevel = Player.instance.level;
        displayCurrentExp = playerStartExp;
        displayCurrentLevel = playerStartLevel;
        currentLevelCap = Player.instance.CalculateLevelCap(displayCurrentLevel);
        
        currentLevel.text = $"{playerStartLevel}";
        nextLevel.text = $"{playerStartLevel + 1}";
        //expText.text = $"{Player.instance.exp}/{Player.instance.levelCap}";

        expCurrentProgressBar.anchorMax = new Vector2((float)playerStartExp / (float)currentLevelCap, 0.5f);
        expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);

        foreach (Transform t in expContent)
            Destroy(t.gameObject);

        baseExpPhase = true;
        currentNewExp = 0f;
        t = 0f;

        skip = false;
        skipHomeButton.onClick.RemoveAllListeners();
        skipHomeButton.onClick.AddListener(SkipToEndPostMatchSummary);
        skipHomeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "SKIP";
    }

    void SkipToEndPostMatchSummary()
    {
        skip = true;
        foreach (Transform t in expContent)
            Destroy(t.gameObject);

        // base exp
        expGain = 0;
        expGain += GetSkinExp();
        int negExpGain = EnemyController.instance.GetNegativeExp();
        expGain += negExpGain;
        GameObject src = Instantiate(expSourceText, expContent);
        src.GetComponent<TMP_Text>().text = $"+{expGain} xp from skin rarity {ObjectNames.NicifyVariableName(Player.instance.GetSkinRarity().ToString())} and {negExpGain}xp from enemies";

        // modifier exp
        foreach (Modifier mod in Player.instance.modifiers)
        {
            if (mod.modifierType == Modifier.Type.ExpMult | mod.modifierType == Modifier.Type.ExpAdd)
            {
                bool success = mod.ModifierEffect();
                if (success)
                {
                    src = Instantiate(expSourceText, expContent);
                    src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: {mod.ModifierExpDescription()}";
                }
                else
                {
                    src = Instantiate(expSourceText, expContent);
                    src.GetComponent<TMP_Text>().text = $"{mod.nameText.text}: failed";
                }
            }
        }

        Player.instance.AddTotalExperience(expGain);

        expGainText.text = $"+{expGain}xp";

        if (Player.instance.level == Player.instance.maxLevel)
        {
            currentLevel.text = $"{Player.instance.level - 1}";
            nextLevel.text = $"{Player.instance.level}";

            expCurrentProgressBar.anchorMax = new Vector2(1f, 0.5f);
            expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);
        }
        else
        {
            currentLevel.text = $"{Player.instance.level}";
            nextLevel.text = $"{Player.instance.level + 1}";
            //expText.text = $"{Player.instance.exp}/{Player.instance.levelCap}";

            expCurrentProgressBar.anchorMax = new Vector2((float)Player.instance.exp / (float)Player.instance.levelCap, 0.5f);
            expCurrentProgressBar.sizeDelta = new Vector2(0, expCurrentProgressBar.sizeDelta.y);
        }

        skipHomeButton.onClick.RemoveAllListeners();
        skipHomeButton.onClick.AddListener(ReturnHome);
        skipHomeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "HOME";
    }

    void ReturnHome()
    {
        postMatchSummary = false;

        foreach (Modifier mod in Player.instance.modifiers)
            mod.lockDrag = false;

        if (!skip) Player.instance.AddTotalExperience(expGain);
        
        endMatchButton.gameObject.SetActive(true);
        postMatchSummaryPanel.SetActive(false);
        gameObject.SetActive(false);
        Home.instance.gameObject.SetActive(true);
        Home.instance.OpenCanvas(Home.instance.collectionCanvas, Home.instance.collectionBtn);

        if (EnemyController.instance.bossFight)
        {
            Player.instance.StartNextSeason();
            Store.instance.RandomizeFeaturedStore();
            EnemyController.instance.bossFight = false;
        }
            
        Player.instance.seasonTimerLock = false;
    }

    public int GetSkinExp()
    {
        switch (Player.instance.GetSkinRarity())
        {
            case Skin.Rarity.VeryCommon:
                return 100;
            case Skin.Rarity.Common:
                return 200;
            case Skin.Rarity.Rare:
                return 300;
            case Skin.Rarity.Legendary:
                return 400;
        }

        return 0;
    }
}
