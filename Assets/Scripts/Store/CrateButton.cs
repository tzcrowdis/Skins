using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using static UnityEditor.Progress;

public class CrateButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Crate")]
    public string crateName;
    public Image crateImage;
    public int coinCost;
    public GameObject cratePrefab;
    public Outline outline;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public TMP_Text nameText;
    public TMP_Text itemsText;
    Transform storeCanvas;

    // TODO crate info panel with collection skins and name

    [Header("Confirmation Panel")]
    public GameObject confirmationPanel;
    public Image crateConfirmationImage;
    public TMP_Text costConfirmationText;
    public Button crateConfirmButton;
    public Button crateCancelButton;


    void Start()
    {
        nameText.text = crateName;
        itemsText.text = "";
        foreach (GameObject item in cratePrefab.GetComponent<Crate>().items)
        {
            CollectionItem colItem = item.GetComponent<CollectionItem>();
            itemsText.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(colItem.GetRarityColor())}>{colItem.itemName}</color>\n";
        }

        infoPanel.SetActive(false);
        storeCanvas = Store.instance.transform;

        outline.enabled = false;
        
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
        infoPanel.SetActive(true);
        infoPanel.transform.SetParent(storeCanvas);

        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.transform.SetParent(transform);
        infoPanel.SetActive(false);

        outline.enabled = false;
    }
}
