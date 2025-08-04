using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [Header("Sub-Store Buttons")]
    public Button featuredButton;
    public Button crateButton;
    public Button coinButton;
    List<Button> subButtons;

    [Header("Sub-Store Panels")]
    public GameObject featuredPanel;
    public GameObject cratePanel;
    public GameObject coinPanel;
    List<GameObject> subPanels;

    [Header("Purchase Confirmation Panel")]
    public GameObject confirmationPanel;
    public Image itemImage;
    public TMP_Text costText;
    public Button confirmButton;
    public Button cancelButton;


    public static Store instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;


        featuredButton.onClick.AddListener(delegate { OpenSubPanel(featuredPanel, featuredButton); });
        crateButton.onClick.AddListener(delegate { OpenSubPanel(cratePanel, crateButton); });
        coinButton.onClick.AddListener(delegate { OpenSubPanel(coinPanel, coinButton); });

        subButtons = new List<Button>();
        subButtons.Add(featuredButton);
        subButtons.Add(crateButton);
        subButtons.Add(coinButton);

        subPanels = new List<GameObject>();
        subPanels.Add(featuredPanel);
        subPanels.Add(cratePanel);
        subPanels.Add(coinPanel);
    }

    void Start()
    {
        RandomizeFeaturedStore();

        foreach (GameObject panel in subPanels)
            panel.SetActive(false);
        featuredPanel.SetActive(true);
        featuredButton.interactable = false;

        confirmationPanel.SetActive(false);
    }

    public void OpenSubPanel(GameObject panel, Button navBtn)
    {
        foreach (GameObject obj in subPanels)
        {
            if (obj == panel)
                obj.gameObject.SetActive(true);
            else
                obj.gameObject.SetActive(false);
        }

        foreach (Button btn in subButtons)
        {
            if (btn == navBtn)
                btn.interactable = false;
            else
                btn.interactable = true;
        }
    }

    public void OpenPurchasePanel(StoreButton storeButton)
    {
        foreach (Transform child in confirmationPanel.transform)
            child.gameObject.SetActive(false);

        itemImage.color = storeButton.image.color;
        itemImage.sprite = storeButton.image.sprite;
        itemImage.material = storeButton.image.material;
        itemImage.gameObject.SetActive(true);

        costText.text = storeButton.cost.text;
        costText.gameObject.SetActive(true);

        if (Player.instance.coins - storeButton.itemCost > 0)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Confirm";
            confirmButton.onClick.AddListener(delegate { ConfirmPurchase(storeButton); });
            confirmButton.gameObject.SetActive(true);
        }
        else
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Coins";
            confirmButton.onClick.AddListener(Home.instance.OpenCoinStore);
            confirmButton.onClick.AddListener(CancelPurchase);
            confirmButton.gameObject.SetActive(true);
        }

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(CancelPurchase);
        cancelButton.gameObject.SetActive(true);

        confirmationPanel.SetActive(true);
    }

    void ConfirmPurchase(StoreButton storeButton)
    {
        bool purchased = Player.instance.InGamePurchase(storeButton.itemCost);

        if (!purchased)
        {
            Debug.Log("not purchased"); // TODO notify player
            return;
        }  

        switch (storeButton.type)
        {
            case StoreButton.itemType.Skin:
                Collection.instance.AddToCollection(storeButton.item.GetComponent<CollectionItem>());
                break;
            case StoreButton.itemType.Crate:
                Collection.instance.AddToCollection(storeButton.item.GetComponent<CollectionItem>());
                break;
            case StoreButton.itemType.Modifier:
                Player.instance.AddToModifierList(storeButton.item.GetComponent<Modifier>());
                break;
        }

        storeButton.SetPurchased(true);
        confirmationPanel.SetActive(false);
    }

    void CancelPurchase()
    {
        confirmationPanel.SetActive(false);
    }

    public void RandomizeFeaturedStore()
    {
        foreach (Transform child in featuredPanel.transform)
        {
            child.GetComponent<StoreButton>().RandomizeItem();
        }
    }
}
