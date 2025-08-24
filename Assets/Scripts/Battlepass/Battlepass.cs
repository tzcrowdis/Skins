using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battlepass : MonoBehaviour
{
    [Header("Premium Panel")]
    public GameObject premiumPanel;

    [Header("Premium Buttons")]
    public Button premiumButton;
    public Button purchasePremiumButton;
    public Button exitPremiumPanel;

    [Header("Premium Pricing")]
    public int premiumCoinPrice;
    public TMP_Text coinPriceText;
    public bool premiumOwner = false;

    [Header("Battlepass Content")]
    public GameObject bpContent;
    public GameObject bpInfoPanel;
    

    public static Battlepass instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        premiumButton.onClick.AddListener(DisplayPremiumPanel);
        exitPremiumPanel.onClick.AddListener(ExitPremiumPanel);

        coinPriceText.text = $"\u0424{premiumCoinPrice}";
        premiumPanel.SetActive(false);

        GenerateBattlepassItems();
    }

    private void OnDisable()
    {
        if (bpInfoPanel.activeSelf)
            bpInfoPanel.SetActive(false);

        foreach (Transform item in bpContent.transform)
        {
            try { item.GetComponent<BattlepassItem>().Deselect(); }
            catch { Debug.Log("couldn't deselect battlepass item"); }
        }
    }

    public void GenerateBattlepassItems()
    {
        foreach (Transform item in bpContent.transform)
            item.GetComponent<BattlepassItem>().GenerateItem();
    }

    void DisplayPremiumPanel()
    {
        if (Player.instance.coins - premiumCoinPrice < 0)
        {
            purchasePremiumButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Coins";
            purchasePremiumButton.onClick.RemoveAllListeners();
            purchasePremiumButton.onClick.AddListener(Home.instance.OpenCoinStore);
            purchasePremiumButton.onClick.AddListener(ExitPremiumPanel);
        }
        else
        {
            purchasePremiumButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Premium";
            purchasePremiumButton.onClick.RemoveAllListeners();
            purchasePremiumButton.onClick.AddListener(PurchasePremium);
        }

        premiumPanel.SetActive(true);
    }

    void ExitPremiumPanel()
    {
        premiumPanel.SetActive(false);
    }

    void PurchasePremium()
    {
        bool purchased = Player.instance.InGamePurchase(premiumCoinPrice);

        if (purchased)
        {
            premiumOwner = true;
            
            foreach (BattlepassItem bpItem in bpContent.GetComponentsInChildren<BattlepassItem>())
                bpItem.BattlepassUnlock();

            premiumButton.gameObject.SetActive(false);
            premiumPanel.SetActive(false);
        }
        else
        {
            Debug.Log("couldnt buy battlepass");
        }
    }

    public void LevelReachedUnlock(int level)
    {
        if (level <= 0)
            return;
        
        if (premiumOwner)
            bpContent.transform.GetChild(level - 1).GetComponent<BattlepassItem>().FullUnlock();
        else
            bpContent.transform.GetChild(level - 1).GetComponent<BattlepassItem>().LevelUnlock();
    }
}
