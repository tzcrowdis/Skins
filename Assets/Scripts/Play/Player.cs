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
    public int maxLevel = 10;
    public int exp = 0;
    public int levelCap;
    public int levelBase = 100;
    public float levelDelta = 1.5f;
    
    [Header("Currencies")]
    public int dollars;
    public TMP_Text dollarText;
    public int coins;
    public TMP_Text coinsText;

    [Header("Currency Adder")]
    public int dollarsPerSeason = 50;

    [Header("Alert")]
    public GameObject alertPrefab;

    [Header("Modifiers")]
    [SerializeField]
    public List<Modifier> modifiers;
    public int modifierCapacity = 5;
    public GameObject modifierPanel;
    public TMP_Text modifierCount;

    [Header("Level Display")]
    public TMP_Text levelText;

    [Header("Season")]
    public int season = 1;
    public TMP_Text seasonText;
    public int lastSeason = 3;
    public TMP_Text seasonTimerText;
    public int seasonTotalMinutes = 5;
    float seasonTimeLeft;
    [HideInInspector] public bool seasonTimerLock = false;

    public static Player instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void Start()
    {
        UpdateCurrencyFields();
        UpdateLevelText();

        levelCap = CalculateLevelCap(level);

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
        int seconds = Mathf.FloorToInt(seasonTimeLeft % 60);
        seasonTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (seasonTimeLeft <= 0)
            QueueBoss();
    }

    /*
     * GAME PROGRESSION
     */
    void QueueBoss()
    {
        seasonTimerLock = true;
        seasonTimerText.text = $"00:00";
        seasonTimerText.color = Color.red;

        if (!Home.instance.readyUpCanvas.gameObject.activeSelf)
        {
            GameObject alert = Instantiate(alertPrefab, Home.instance.readyUpBtn.transform);
            alert.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-115f, 0f, 0f);
            Home.instance.readyUpBtn.onClick.AddListener(delegate { DestroyAlert(alert); });
            ReadyUp.instance.startButton.onClick.AddListener(delegate { DestroyAlert(alert); });
        }

        if (season == 1)
        {
            EnemyController.instance.QueueBoss(EnemyController.BossType.Randomizer);
            ReadyUp.instance.bossWarning.text = $"Warning! You are about to face the {EnemyController.BossType.Randomizer.ToString()}!" +
                $"\nYou will need to gain more than {EnemyController.instance.GetBossExpThreshold()}xp to progress.";
        }   
        else if (season == 2)
        {
            EnemyController.instance.QueueBoss(EnemyController.BossType.MonkeyPaw);
            ReadyUp.instance.bossWarning.text = $"Warning! You are about to face the {EnemyController.BossType.MonkeyPaw.ToString()}!" +
                $"\nYou will need to gain more than {EnemyController.instance.GetBossExpThreshold()}xp to progress.";
        }   
        else if (season == 3)
        {
            EnemyController.instance.QueueBoss(EnemyController.BossType.EvilRandomizer);
            ReadyUp.instance.bossWarning.text = $"Warning! You are about to face the {EnemyController.BossType.EvilRandomizer.ToString()}!" +
                $"\nYou will need to gain more than {EnemyController.instance.GetBossExpThreshold()}xp to progress.";
        }
            
        ReadyUp.instance.bossWarningContainer.SetActive(true);
    }

    public void StartNextSeason()
    {
        // TODO season themes??

        season += 1; 
        if (season > lastSeason || Play.instance.expGain < EnemyController.instance.GetBossExpThreshold())
        {
            var particles = MainMenu.instance.backgroundParticles.main;
            particles.simulationSpeed = 1f;
            particles.startColor = Color.gray;

            HidePlayer();
            foreach (Transform enemy in EnemyController.instance.transform)
                enemy.gameObject.SetActive(false);

            foreach (GameObject canvas in MainMenu.instance.openingCanvases)
                canvas.SetActive(false);

            GameOver.instance.gameObject.SetActive(true);
            GameOver.instance.PopulateGameExplanation();

            seasonTimeLeft = Mathf.Infinity; // HACK prevents queueing next boss if lost before final

            return;
        }

        level = 0;
        exp = 0;
        levelCap = CalculateLevelCap(level);
        UpdateLevelText();

        Battlepass.instance.premiumOwner = false;
        Battlepass.instance.GenerateBattlepassItems();
        Battlepass.instance.premiumButton.gameObject.SetActive(true);

        seasonText.text = $"Season: {season}";
        seasonTimerText.color = Color.white;
        seasonTimeLeft = seasonTotalMinutes * 60f;
        seasonTimerLock = false;

        //dollars += dollarsPerSeason;
        dollars += Random.Range(25, 51); // NOTE per season $ variance
        MoneyAlert();
        UpdateCurrencyFields();

        ReadyUp.instance.bossWarningContainer.SetActive(false);
    }

    /*
     * ALERTS
     */
    public void DestroyAlert(GameObject alert)
    {
        Destroy(alert);
    }

    public void MoneyAlert()
    {
        GameObject alert = Instantiate(alertPrefab, dollarText.transform);
        alert.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-75f, 0f, 0f);
        alert.GetComponent<AlertEffect>().lifetime = 5f;
    }

    /*
     * MODIFIERS
     */

    public bool AddToModifierList(Modifier mod)
    {
        if (modifiers.Count + 1 > modifierCapacity)
            return false; // TODO notify player that a mod needs to be deleted?

        GameObject newMod = Instantiate(mod.gameObject, modifierPanel.transform);
        modifiers.Add(newMod.GetComponent<Modifier>());
        modifierCount.text = $"{modifiers.Count} / {modifierCapacity}";

        if (modifiers.Count == modifierCapacity)
        {
            foreach (Transform btn in Store.instance.featuredPanel.transform)
            {
                StoreButton storeBtn = btn.GetComponent<StoreButton>();
                if (storeBtn && storeBtn.type == StoreButton.itemType.Modifier)
                    storeBtn.LockModifiersFull();
            }
        }

        Modifier alterMod = newMod.GetComponent<Modifier>();
        foreach (Modifier activeMod in modifiers)
        {
            if (activeMod.modifierType == Modifier.Type.AlterState)
                activeMod.AlterOtherModifier(alterMod);
        }

        return true;
    }

    public void RemoveFromModifierList(Modifier mod)
    {
        modifiers.Remove(mod);
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

    public void ResetModifierList()
    {
        modifiers = new List<Modifier>();
        foreach (Transform child in modifierPanel.transform)
            Destroy(child.gameObject);
    }

    /*
     * CURRENCY
     */
    void UpdateCurrencyFields()
    {
        dollarText.text = $"${dollars}";
        coinsText.text = $"\u0424{coins}";
    }

    public bool InGamePurchase(int coinAmount)
    {
        if (coins - coinAmount < 0)
        {
            MoneyAlert();
            return false;
        } 

        coins -= coinAmount;
        UpdateCurrencyFields();
        return true;
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

        skinExpText.text = $"+{skin.GetSkinExp()}xp";
    }

    public Skin.Rarity GetSkinRarity()
    {
        return skin.rarity;
    }

    /*
     * LEVELING
     */
    public void UpdateLevelText()
    {
        if (level == maxLevel)
            levelText.text = $"Lvl. MAX";
        else
            levelText.text = $"Lvl. {level}";
    }

    public void AddTotalExperience(int newExp)
    {
        if (level == maxLevel)
            return;
        
        int expRemainder = newExp;
        int expPreviousIteration;

        exp += expRemainder;
        for (int i = 0; i < 1000; i++) // 1000 iterations instead of while to prevent infinite loop
        {
            expPreviousIteration = exp;
            
            if (exp >= levelCap)
            {
                expRemainder = exp - levelCap;
                exp = expRemainder;
                level += 1;
                UpdateLevelText();
                levelCap = CalculateLevelCap(level);
                Battlepass.instance.LevelReachedUnlock(level);
                
                if (level == maxLevel)
                    return;
            }
            else if (exp < 0)
            {
                level -= 1;
                levelCap = CalculateLevelCap(level);
                expRemainder = exp;
                exp = levelCap;
                UpdateLevelText();

                if (level <= 0 & exp <= 0)
                {
                    level = 0;
                    exp = 0;
                    UpdateLevelText();
                    return;
                }
            }

            if (exp == expPreviousIteration)
                break;

            if (expRemainder <= 0 & level == 0)
                break;

            if (i == 999)
                Debug.Log("Level couldn't be found in 1000 iterations");
        }
    }

    public int CalculateLevelCap(int level)
    {
        //return levelBase * (int)Mathf.Pow(levelDelta, level + 1 + (season - 1) * 10);
        return levelBase * (level + 1 + (season - 1) * 10);
    }
}
