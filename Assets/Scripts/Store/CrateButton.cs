using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CrateButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Crate")]
    public Image crateImage;
    public int coinCost;
    public GameObject cratePrefab;
    
    // TODO crate info panel with collection skins and name
    
    [Header("Confirmation Panel")]
    public GameObject confirmationPanel;
    public Image crateConfirmationImage;
    public TMP_Text costConfirmationText;
    public Button crateConfirmButton;
    public Button crateCancelButton;


    void Start()
    {
        crateCancelButton.onClick.AddListener(CloseCrateConfirmationPanel);

        confirmationPanel.SetActive(false);
    }

    void ConfirmCratePurchase()
    {
        bool purchased = Player.instance.InGamePurchase(coinCost);

        if (purchased)
        {
            Collection.instance.AddToCollection(cratePrefab.GetComponent<CollectionItem>());
            confirmationPanel.SetActive(false);

            // TODO signal success
        }
        else
        {
            OpenCrateConfirmationPanel();

            // TODO signal failure
        }
    }

    void OpenCrateConfirmationPanel()
    {
        crateConfirmButton.onClick.RemoveAllListeners();
        if (Player.instance.coins -  coinCost < 0)
        {
            crateConfirmButton.onClick.AddListener(Home.instance.OpenCoinStore);
            crateConfirmButton.onClick.AddListener(CloseCrateConfirmationPanel);
            crateConfirmButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Coins";
        }
        else
        {
            crateConfirmButton.onClick.AddListener(ConfirmCratePurchase);
            crateConfirmButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Crate";
        }   

        crateConfirmationImage.color = crateImage.color;
        crateConfirmationImage.sprite = crateImage.sprite;
        crateConfirmationImage.material = crateImage.material;

        costConfirmationText.text = $"\u0424{coinCost}";

        confirmationPanel.SetActive(true);
    }

    void CloseCrateConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenCrateConfirmationPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO hover effect
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO remove hover effect
    }
}
