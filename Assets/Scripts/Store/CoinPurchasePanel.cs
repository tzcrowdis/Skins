using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CoinPurchasePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Input Fields")]
    public int coinQuantity;
    public int dollarPrice;
    
    [Header("Properties")]
    public Image image;
    public Outline outline;
    public TMP_Text coinQuantityText;
    public Button coinPurchaseButton;
    public TMP_Text dollarPriceText;
    public TMP_Text discountText;
    float discount;

    [Header("Confirmation Panel")]
    public GameObject confirmationPanel;
    public Image confirmationImage;
    public TMP_Text confCoinAmountText;
    public TMP_Text dollarCostText;
    public Button confirmButton;
    public Button cancelButton;

    void Start()
    {
        outline.enabled = false;
        
        coinPurchaseButton.onClick.AddListener(OpenConfirmationPanel);

        coinQuantityText.text = $"\u0424{coinQuantity}";
        dollarPriceText.text = $"${dollarPrice}";

        discount = (coinQuantity / dollarPrice) - 100f;
        if (discount > 0)
            discountText.text = $"{discount}% Extra";
        else
            discountText.gameObject.SetActive(false);

        confirmationPanel.SetActive(false);
    }

    void OpenConfirmationPanel()
    {
        foreach (Transform child in confirmationPanel.transform)
            child.gameObject.SetActive(false);
        
        confirmationImage.color = image.color;
        confirmationImage.sprite = image.sprite;
        confirmationImage.material = image.material;
        confirmationImage.gameObject.SetActive(true);

        confCoinAmountText.text = coinQuantityText.text;
        confCoinAmountText.gameObject.SetActive(true);

        dollarCostText.text = dollarPriceText.text;
        dollarCostText.gameObject.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(PurchaseCoins);
        confirmButton.gameObject.SetActive(true);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CloseConfirmationPanel);
        cancelButton.gameObject.SetActive(true);

        confirmationPanel.SetActive(true);
    }

    void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    void PurchaseCoins()
    {
        bool purchased = Player.instance.CoinPurchase(dollarPrice, coinQuantity);

        if (purchased) 
            CloseConfirmationPanel();
        else
        {
            Player.instance.MoneyAlert();
            CloseConfirmationPanel();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
