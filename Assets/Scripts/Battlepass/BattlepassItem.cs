using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattlepassItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Collection Item - Skin")]
    public CollectionItem collectionItem;

    [Header("Modifier")]
    public Modifier mod;

    [Header("Currency")]
    public int coinAmount;

    [Header("Premium Lock")]
    public bool locked;
    public TMP_Text lockedLevel;
    public GameObject lockImage;
    public GameObject lockedOverlay;

    [Header("Image + Border + Text")]
    public Image itemImage;
    public Image itemImageMask;
    public Image rarityBorder;
    public GameObject textPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Claim Requirement")]
    public bool claimed = false;
    public int levelToClaim;
    public GameObject claimedOverlay;

    Color rarityColor;
    bool selected = false;

    [Header("Item Type")]
    public itemType type;
    public enum itemType
    {
        Skin,
        Modifier,
        Currency
    }


    void Start()
    {
        GenerateItem();

        textPanel.SetActive(false);
    }

    public void GenerateItem()
    {
        ResetItem();
        
        float r = Random.Range(0f, 1f);
        if (levelToClaim < 8)
        {
            if (r < 0.5f)
                GenerateSkin();
            else if (r < 0.75f)
                GenerateModifier();
            else
                GenerateCurrency();
        }
        else if (levelToClaim < 10)
        {
            if (r < 0.75f)
                GenerateSkin();
            else
                GenerateModifier();
        }
        else
            GenerateSkin();
    }

    void GenerateSkin()
    {
        type = itemType.Skin;

        float r = Random.Range(0f, 1f);
        Skin.Rarity rarity = Skin.Rarity.VeryCommon;
        if (levelToClaim < 6)
        {
            // 60% very common
            // 40% common
            if (r < 0.6f)
                rarity = Skin.Rarity.VeryCommon;
            else
                rarity = Skin.Rarity.Common;
        }
        else if (levelToClaim < 8)
        {
            // 30% very common
            // 40% common
            // 30% rare
            if (r < 0.3f)
                rarity = Skin.Rarity.VeryCommon;
            else if (r < 0.7f)
                rarity = Skin.Rarity.Common;
            else
                rarity = Skin.Rarity.Rare;
        }
        else if (levelToClaim < 10)
        {
            // 50% common
            // 50% rare
            if (r < 0.5f)
                rarity = Skin.Rarity.Common;
            else
                rarity = Skin.Rarity.Rare;
        }
        else
        {
            // 50% rare
            // 50% legendary
            if (r < 0.5f)
                rarity = Skin.Rarity.Rare;
            else
                rarity = Skin.Rarity.Legendary;
        }

        Skin skin = ItemDatabase.instance.RandomSkinRandomCollection(rarity);
        collectionItem = skin;

        itemImage.sprite = collectionItem.itemImage.sprite;
        itemImage.color = collectionItem.itemImage.color;
        itemImage.material = collectionItem.itemImage.material;

        rarityColor = collectionItem.GetRarityColor();
        rarityBorder.color = rarityColor;
    }

    void GenerateModifier()
    {
        type = itemType.Modifier;

        // TODO modifier rarity grows with battle pass index

        mod = ItemDatabase.instance.RandomModifier();

        itemImage.sprite = mod.modifierImage.sprite;
        itemImage.color = mod.modifierImage.color;
        itemImage.material = mod.modifierImage.material;

        nameText.text = mod.modifierName;
        descriptionText.text = mod.modifierDescription.text;

        rarityBorder.color = mod.GetRarityColor();
    }

    void GenerateCurrency()
    {
        type = itemType.Currency;
        float coinRandom = Random.Range(0, 3);
        switch (coinRandom)
        {
            case 0:
                coinAmount = 100;
                break;
            case 1:
                coinAmount = 200;
                break;
            case 2:
                coinAmount = 300;
                break;
        }

        Image coinImage = ItemDatabase.instance.GetCoinImage();
        itemImage.sprite = coinImage.sprite;
        itemImage.color = coinImage.color;
        itemImage.material = coinImage.material;
        Color blank = Color.white;
        blank.a = 0;
        rarityBorder.color = blank;
    }

    void ResetItem()
    {
        collectionItem = null;
        mod = null;
        coinAmount = 0;

        claimed = false;
        claimedOverlay.SetActive(claimed);

        for (int i = 0; i < Battlepass.instance.bpContent.transform.childCount; i++)
        {
            BattlepassItem child = Battlepass.instance.bpContent.transform.GetChild(i).GetComponent<BattlepassItem>();
            if (child == this)
            {
                levelToClaim = i + 1;
                break;
            }
        }
        lockedLevel.text = $"{levelToClaim}";
        locked = true;

        BattlepassUnlock();
    }

    public void BattlepassUnlock()
    {
        if (Player.instance.level >= levelToClaim)
            locked = false;
        
        lockedOverlay.GetComponent<Image>().enabled = locked;

        if (selected)
            BattlepassInfoPanel.instance.BattlepassUnlock();
    }

    public void LevelUnlock()
    {
        if (!locked)
            lockImage.SetActive(false);
        else
            lockedLevel.text = "";
    }

    public void FullUnlock()
    {
        locked = false;
        lockedOverlay.SetActive(locked);

        if (selected)
            BattlepassInfoPanel.instance.FullUnlock();
    }

    public void ClaimItem()
    {      
        if (Player.instance.level >= levelToClaim)
        {
            switch (type)
            {
                case itemType.Skin:
                    Collection.instance.AddToCollection(collectionItem);
                    break;
                case itemType.Modifier:
                    Player.instance.AddToModifierList(mod);
                    break;
                case itemType.Currency:
                    Player.instance.CoinPurchase(0, coinAmount);
                    break;
            }

            claimed = true;
            claimedOverlay.SetActive(claimed);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattlepassInfoPanel.instance.DisplayBattlepassInfoPanel(this);
        selected = true;

        if (type == itemType.Modifier)
        {
            textPanel.transform.SetParent(transform);
            textPanel.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected)
        {
            MoveImageAndBorderUp();

            if (type == itemType.Modifier)
            {
                textPanel.SetActive(true);
                textPanel.transform.SetParent(Battlepass.instance.transform);
            }   
        }   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
        {
            MoveImageAndBorderDown();

            if (type == itemType.Modifier)
            {
                textPanel.transform.SetParent(transform);
                textPanel.SetActive(false);
            }
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            MoveImageAndBorderDown();
        }  
        selected = false;
    }

    void MoveImageAndBorderUp()
    {
        float distance = 20f;
        //itemImage.transform.position += new Vector3(0, distance, 0);
        itemImageMask.transform.position += new Vector3(0, distance, 0);
        rarityBorder.transform.position += new Vector3(0, distance, 0);
        lockedOverlay.transform.position += new Vector3(0, distance, 0);
        claimedOverlay.transform.position += new Vector3(0, distance, 0);

        if (type == itemType.Modifier)
            textPanel.transform.position += new Vector3(0, distance, 0);
    }

    void MoveImageAndBorderDown()
    {
        float distance = 20f;
        //itemImage.transform.position -= new Vector3(0, distance, 0);
        itemImageMask.transform.position -= new Vector3(0, distance, 0);
        rarityBorder.transform.position -= new Vector3(0, distance, 0);
        lockedOverlay.transform.position -= new Vector3(0, distance, 0);
        claimedOverlay.transform.position -= new Vector3(0, distance, 0);

        if (type == itemType.Modifier)
            textPanel.transform.position -= new Vector3(0, distance, 0);
    }
}
