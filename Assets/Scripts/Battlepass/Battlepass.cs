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

    [Header("Battlepass Content")]
    public GameObject bpContent;

    void Start()
    {
        premiumButton.onClick.AddListener(DisplayPremiumPanel);
        exitPremiumPanel.onClick.AddListener(ExitPremiumPanel);

        coinPriceText.text = $"&{premiumCoinPrice}";
        premiumPanel.SetActive(false);
    }

    void DisplayPremiumPanel()
    {
        if (Player.instance.coins - premiumCoinPrice < 0)
        {
            purchasePremiumButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Coins";
            purchasePremiumButton.onClick.RemoveAllListeners();
            purchasePremiumButton.onClick.AddListener(OpenCoinStore);
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
            foreach (BattlepassItem bpItem in bpContent.GetComponentsInChildren<BattlepassItem>())
                bpItem.UnlockItem();

            premiumButton.gameObject.SetActive(false);
            premiumPanel.SetActive(false);
        }
        else
        {
            Debug.Log("couldnt buy battlepass");
        }
    }

    void OpenCoinStore()
    {
        ExitPremiumPanel();
        Home.instance.OpenCanvas(Home.instance.storeCanvas, Home.instance.storeBtn); // TODO open directly to coin tab
    }
}
