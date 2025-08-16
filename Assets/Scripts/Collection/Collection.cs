using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Crate;

public class Collection : MonoBehaviour
{
    [Header("Collection Content")]
    public GameObject collectionContent;

    [Header("Default Collection Items")]
    public GameObject[] defaultCollectionItems;

    [Header("Crate Purchase")]
    public GameObject crateKeyPanel;
    public Image crateKeyImage;
    public TMP_Text crateKeyCost;
    public Button crateKeyButton;

    [Header("Crate Opening")]
    public GameObject openingPanel;
    public Image rewardImage;
    public Button claimButton;
    public float rotationSpeed;
    public float crateAnimLength;
    bool crateOpeningAnimation = false;
    float crateAnimTime = 0f;
    Crate purchasedCrate;
    bool rotateClockwise = false;

    [Header("Crate Opening Sounds")]
    public AudioSource crateAudio;
    public AudioClip crateRotateSound;
    public AudioClip crateOpenSound;

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
        ResetCollection();

        crateKeyPanel.SetActive(false);
        openingPanel.SetActive(false);
    }

    public void ResetCollection()
    {
        foreach (Transform item in collectionContent.transform)
            Destroy(item.gameObject);

        foreach (GameObject defaultItem in defaultCollectionItems)
            Instantiate(defaultItem, collectionContent.transform);
    }

    void Update()
    {
        if (crateOpeningAnimation)
        {
            if (rotateClockwise)
                rewardImage.transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
            else
                rewardImage.transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));

            crateAnimTime += Time.deltaTime;
            if (crateAnimTime > crateAnimLength)
            {
                CancelInvoke();
                crateOpeningAnimation = false;
                crateAnimTime = 0f;
                OpenOpeningPanel(purchasedCrate);
                crateAudio.PlayOneShot(crateOpenSound);
            }
        }
    }

    void CrateAnimRotationFlip()
    {
        rotateClockwise = !rotateClockwise;
        crateAudio.PlayOneShot(crateRotateSound);
    }

    public void OpenCrateKeyPanel(Crate crate)
    {
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
            crateOpeningAnimation = true;
            purchasedCrate = crate;

            rewardImage.color = crate.itemImage.color;
            rewardImage.sprite = crate.itemImage.sprite;
            rewardImage.material = crate.itemImage.material;

            claimButton.gameObject.SetActive(false);
            openingPanel.SetActive(true);

            InvokeRepeating("CrateAnimRotationFlip", 0.25f, 0.5f);

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
        rewardImage.transform.rotation = Quaternion.Euler(0, 0, 0);

        claimButton.gameObject.SetActive(true);
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(delegate { AddReward(crate, reward, crate.type); });
    }

    void AddReward(Crate crate, GameObject reward, Crate.CrateType type)
    {
        switch (type) // DEPRECATED?
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

    public void AddToCollection(CollectionItem item)
    {
        Instantiate(item.gameObject, collectionContent.transform);
    }

    public void DeleteCollectionItem(CollectionItem item)
    {
        foreach (Transform colItem in collectionContent.transform)
        {
            if (colItem.GetComponent<CollectionItem>() == item)
                Destroy(colItem.gameObject);
        }
    }
}
