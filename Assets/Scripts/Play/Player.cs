using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Skin")]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Skin skin;

    [Header("Stats")]
    public int level = 0;
    public int exp = 0;
    public int levelCap;
    public int levelBase = 100;
    public float levelDelta = 1.5f;
    
    [Header("Currencies")]
    public int dollars;
    public TMP_Text dollarText;
    public int coins;
    public TMP_Text coinsText;

    [Header("Modifiers")]
    [SerializeField]
    public List<Modifier> modifiers;
    public int modifierCapacity = 5;
    public GameObject modifierPanel;
    public GameObject emptyModifierSlot;

    [Header("Level Display")]
    public TMP_Text levelText;

    [Header("Season")]
    public int season = 1;
    public TMP_Text seasonText;
    public int lastSeason = 3;
    public TMP_Text seasonTimerText;
    public int seasonTotalMinutes = 5;
    float seasonTimeLeft;

    public static Player instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        UpdateCurrencyFields();
        UpdateLevelText();

        levelCap = levelBase * (int)Mathf.Pow(levelDelta, level + 1);

        seasonText.text = $"Season: {season}";
        seasonTimeLeft = seasonTotalMinutes * 60f;

        HidePlayer();
    }

    void Update()
    {
        seasonTimeLeft -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(seasonTimeLeft / 60f);
        int seconds = Mathf.FloorToInt(seasonTimeLeft - minutes * 60);
        seasonTimerText.text = $"{minutes}:{seconds}";

        if (seasonTimeLeft <= 0)
            StartNextSeason();
    }

    void StartNextSeason()
    {
        Battlepass.instance.GenerateBattlepassItems();

        season += 1; // TODO season themes??
        if (season > lastSeason)
            Debug.Log("GAME OVER"); // TODO

        seasonText.text = $"Season: {season}";
        seasonTimeLeft = seasonTotalMinutes * 60f;
    }

    public void AddToModifierList(Modifier mod)
    {
        if (modifiers.Count + 1 > modifierCapacity)
            return; // TODO notify player that a mod needs to be deleted

        int i = 0;
        foreach (Transform child in modifierPanel.transform)
        {
            if (child.gameObject.CompareTag("Empty Modifier Slot"))
            {
                Destroy(child.gameObject);
                break;
            }
            i++;
        }

        modifiers.Add(mod);
        GameObject newMod = Instantiate(mod.gameObject, modifierPanel.transform);
        newMod.transform.SetSiblingIndex(i);
    }

    public void RemoveFromModifierList(Modifier mod)
    {
        modifiers.Remove(mod);

        Instantiate(emptyModifierSlot.gameObject, modifierPanel.transform);
    }

    void UpdateCurrencyFields()
    {
        dollarText.text = $"${dollars}";
        coinsText.text = $"\u0424{coins}";
    }

    public void UpdateLevelText()
    {
        levelText.text = $"Lvl. {level}";
    }

    public bool CoinPurchase(int dollarAmount, int coinAmount)
    {
        if (dollars - dollarAmount < 0)
            return false;

        dollars -= dollarAmount;
        coins += coinAmount;
        UpdateCurrencyFields();
        return true;
    }

    public bool InGamePurchase(int coinAmount)
    {
        if (coins - coinAmount < 0)
            return false;

        coins -= coinAmount;
        UpdateCurrencyFields();
        return true;
    }

    /*
     * PLAY
     */
    public void DisplayPlayer()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }

    public void HidePlayer()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void SetSkin(Skin equippedSkin)
    {
        skin = equippedSkin;
        spriteRenderer.color = skin.itemImage.color;
        spriteRenderer.material = skin.itemImage.material;
        if (skin.itemImage.sprite)
            spriteRenderer.sprite = skin.itemImage.sprite;
    }

    public Skin.Rarity GetSkinRarity()
    {
        return skin.rarity;
    }

    public void AddTotalExperience(int newExp)
    {
        int expRemainder = newExp;

        exp += expRemainder;
        for (int i = 0; i < 1000; i++) // 1000 iterations instead of while to prevent infinite loop
        {
            if (exp >= levelCap)
            {
                expRemainder = exp - levelCap;
                exp = expRemainder;
                level += 1;
                levelCap = CalculateLevelCap(level);
                UpdateLevelText();
                Battlepass.instance.LevelReachedUnlock(level);
            }
            else
            {
                break;
            }   

            if (expRemainder <= 0)
                break;

            if (i == 999)
                Debug.Log("Level couldn't be found in 1000 iterations");
        }
    }

    public int CalculateLevelCap(int level)
    {
        return levelBase * (int)Mathf.Pow(levelDelta, level + 1);
    }
}
