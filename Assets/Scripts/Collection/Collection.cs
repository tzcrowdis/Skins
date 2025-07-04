using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Crate;

public class Collection : MonoBehaviour
{
    List<CollectionItem> items;

    [Header("Collection Content")]
    public GameObject collectionContent;

    [Header("Crate Purchase")]
    public GameObject crateKeyPanel;
    public Image crateKeyImage;
    public TMP_Text crateKeyCost;
    public Button crateKeyButton;

    [Header("Crate Opening")]
    public GameObject openingPanel;
    public Image rewardImage;
    public Button claimButton;

    public static Collection instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        // NOTE temp while no save/load functionailty
        items = new List<CollectionItem>();
        foreach (Transform child in collectionContent.transform)
        {
            items.Add(child.GetComponent<CollectionItem>());
        }

        crateKeyPanel.SetActive(false);
        openingPanel.SetActive(false);
    }

    public void AddToCollection(CollectionItem item)
    {
        Instantiate(item.gameObject, collectionContent.transform);
    }

    public void OpenCrateKeyPanel(Crate crate)
    {
        crateKeyImage.sprite = crate.itemImage.sprite;
        crateKeyCost.text = $"\u0424{crate.keyCoinCost}";

        crateKeyButton.onClick.RemoveAllListeners();
        if (Player.instance.coins - crate.keyCoinCost < 0)
        {
            crateKeyButton.onClick.AddListener(Home.instance.OpenCoinStore);
            crateKeyButton.onClick.AddListener(CloseCrateKeyPanel);
            crateKeyButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Coins";
        }
        else
        {
            crateKeyButton.onClick.AddListener(delegate { PurchaseCrateKey(crate); });
            crateKeyButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Get Key";
        }

        crateKeyPanel.SetActive(true);
    }

    void PurchaseCrateKey(Crate crate)
    {
        bool purchased = Player.instance.InGamePurchase(crate.keyCoinCost);

        if (purchased)
        {
            OpenOpeningPanel(crate);
            CloseCrateKeyPanel();
        }
        else
        {
            OpenCrateKeyPanel(crate);
        }
    }

    void CloseCrateKeyPanel()
    {
        crateKeyPanel.SetActive(false);
    }

    public void OpenOpeningPanel(Crate crate)
    {
        GameObject reward = crate.GetReward();

        switch (crate.type)
        {
            case CrateType.Skins:
                Image skinImage = reward.GetComponent<Skin>().itemImage;
                rewardImage.color = skinImage.color;
                rewardImage.sprite = skinImage.sprite;
                rewardImage.material = skinImage.material;
                break;
            case CrateType.Modifiers:
                Image modImage = reward.GetComponent<Modifier>().modifierImage;
                rewardImage.color = modImage.color;
                rewardImage.sprite = modImage.sprite;
                rewardImage.material = modImage.material;
                break;
        }

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(delegate { AddReward(crate, reward, crate.type); });

        openingPanel.SetActive(true);
    }

    void AddReward(Crate crate, GameObject reward, Crate.CrateType type)
    {
        switch (type)
        {
            case CrateType.Skins:
                Collection.instance.AddToCollection(reward.GetComponent<Skin>());
                break;
            case CrateType.Modifiers:
                Player.instance.AddToModifierList(reward.GetComponent<Modifier>());
                break;
        }

        Destroy(crate.gameObject);
        openingPanel.SetActive(false);
    }
}
