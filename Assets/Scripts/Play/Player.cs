using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int matchesPlayed;
    
    [Header("Currencies")]
    public int dollars;
    public TMP_Text dollarText;
    public int coins;
    public TMP_Text coinsText;

    [Header("Modifiers")]
    [SerializeField]
    public List<Modifier> modifiers;

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
    }

    void UpdateCurrencyFields()
    {
        dollarText.text = $"${dollars}";
        coinsText.text = $"&{coins}";
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
}
