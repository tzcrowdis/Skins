using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [Header("Store Buttons")]
    [SerializeField]
    public Button[] storeButtons;

    [Header("Purchase Panel")]
    public GameObject purchasePanel;
    public Image itemImage;
    public TMP_Text costText;
    public Button confirmButton;
    public Button cancelButton;
    Button itemButton;
    
    
    void Start()
    {
        foreach (Button button in storeButtons)
        {
            button.onClick.AddListener(delegate { OpenPurchasePanel(button.GetComponent<StoreButton>()); });
        }

        confirmButton.onClick.AddListener(ConfirmPurchase);
        cancelButton.onClick.AddListener(CancelPurchase);
        purchasePanel.SetActive(false);
    }

    void OpenPurchasePanel(StoreButton storeButton)
    {
        itemImage.sprite = storeButton.image.sprite;
        costText.text = storeButton.cost.text;
        itemButton = storeButton.GetComponent<Button>();

        purchasePanel.SetActive(true);
    }

    void ConfirmPurchase()
    {
        // TODO add to inventory or currency or modifier list

        itemButton.interactable = false;
        purchasePanel.SetActive(false);
    }

    void CancelPurchase()
    {
        purchasePanel.SetActive(false);
    }
}
