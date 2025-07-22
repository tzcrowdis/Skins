using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Skin")]
    public SpriteRenderer spriteRenderer;
    public PlayedSkin playedSkin;
    [HideInInspector]
    public Skin skin;
    public TMP_Text skinExpText;

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

    [Header("Bank")]
    public Button bankButton;
    public GameObject bankPanel;

    [Header("Alert")]
    public GameObject alertPrefab;

    [Header("Modifiers")]
    [SerializeField]
    public List<Modifier> modifiers;
    public int modifierCapacity = 5;
    public GameObject modifierPanel;
    public TMP_Text modifierCount;
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
    bool seasonTimerLock = false;

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
        bankButton.onClick.AddListener(OpenCloseBankPanel);
        bankPanel.SetActive(false);
        
        UpdateCurrencyFields();
        UpdateLevelText();

        levelCap = levelBase * (int)Mathf.Pow(levelDelta, level + 1);

        seasonText.text = $"Season: {season}";
        seasonTimeLeft = seasonTotalMinutes * 60f;

        modifierCount.text = $"{modifiers.Count} / {modifierCapacity}";

        HidePlayer();
    }

    void Update()
    {
        if (seasonTimerLock)
            return;

        seasonTimeLeft -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(seasonTimeLeft / 60f);
        //int seconds = Mathf.FloorToInt(seasonTimeLeft - minutes * 60);
        int seconds = Mathf.FloorToInt(seasonTimeLeft % 60);
        seasonTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); ;

        if (seasonTimeLeft <= 0)
            QueueBoss();
    }

    void QueueBoss()
    {
        seasonTimerLock = true;
        seasonTimerText.text = $"00:00";
        seasonTimerText.color = Color.red;

        GameObject alert = Instantiate(alertPrefab, Home.instance.transform);
        alert.transform.position = Home.instance.readyUpBtn.transform.position - new Vector3(115f, 0f, 0f);
        Home.instance.readyUpBtn.onClick.AddListener(delegate { DestroyAlert(alert); });
        ReadyUp.instance.startButton.onClick.AddListener(delegate { DestroyAlert(alert); });

        EnemyController.instance.QueueBoss(EnemyController.BossType.Randomizer); // TODO different bosses

        ReadyUp.instance.bossWarningContainer.SetActive(true);
        ReadyUp.instance.bossWarning.text = $"Warning! You are about to face the {EnemyController.BossType.Randomizer.ToString()}!";
    }

    public void DestroyAlert(GameObject alert)
    {
        Destroy(alert);
    }

    public void StartNextSeason()
    {
        Battlepass.instance.GenerateBattlepassItems();

        season += 1; // TODO season themes??
        if (season > lastSeason)
            Debug.Log("GAME OVER"); // TODO win/lose state

        seasonText.text = $"Season: {season}";
        seasonTimerText.color = Color.white;
        seasonTimeLeft = seasonTotalMinutes * 60f;
        seasonTimerLock = false;

        ReadyUp.instance.bossWarningContainer.SetActive(false);
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
        modifierCount.text = $"{modifiers.Count} / {modifierCapacity}";
    }

    public void RemoveFromModifierList(Modifier mod)
    {
        modifiers.Remove(mod);

        Instantiate(emptyModifierSlot.gameObject, modifierPanel.transform);

        modifierCount.text = $"{modifiers.Count} / {modifierCapacity}";
    }

    public void ResortModifierList()
    {
        modifiers.Clear();
        foreach (Transform child in modifierPanel.transform)
        {
            modifiers.Add(child.GetComponent<Modifier>());
        }
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

    public bool InGamePurchase(int coinAmount)
    {
        if (coins - coinAmount < 0)
            return false;

        coins -= coinAmount;
        UpdateCurrencyFields();
        return true;
    }

    /*
     * BANK
     */
    void OpenCloseBankPanel()
    {
        bankPanel.SetActive(!bankPanel.activeSelf);
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

        playedSkin.SetSkin(skin);

        skinExpText.text = $"+{Play.instance.GetSkinExp()}xp";
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
