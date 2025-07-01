using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Level Display")]
    public TMP_Text levelText;

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
    }

    public void AddToModifierList(Modifier mod)
    {
        if (modifiers.Count + 1 > modifierCapacity)
            return; // TODO open menu to organize modifiers

        modifiers.Add(mod);
        Instantiate(mod.gameObject, modifierPanel.transform);
    }

    void UpdateCurrencyFields()
    {
        dollarText.text = $"${dollars}";
        coinsText.text = $"&{coins}";
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
    public void SetSkin(Skin equippedSkin)
    {
        skin = equippedSkin;
        spriteRenderer.color = skin.itemImage.color;
        spriteRenderer.material = skin.itemImage.material;
        if (skin.itemImage.sprite)
            spriteRenderer.sprite = skin.itemImage.sprite;
    }

    public void ApplyAllModifiers()
    {
        // TODO
    }

    public void AddExperience(int newExp)
    {
        exp += newExp;

        if (exp >= levelCap)
        {
            exp = 0;
            level += 1;
            levelCap = levelBase * (int)Mathf.Pow(levelDelta, level + 1);
            UpdateLevelText();
        }
    }


}
