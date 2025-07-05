using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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

    [Header("Image + Border")]
    public Image itemImage;
    public Image rarityBorder;

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
    }

    public void GenerateItem()
    {
        ResetItem();
        
        float r = Random.Range(0f, 1f);
        if (r < 0.5f) // skin
        {
            type = itemType.Skin;
            Skin skin = ItemDatabase.instance.RandomSkinRandomCollection();

            collectionItem = skin;

            itemImage.sprite = collectionItem.itemImage.sprite;
            itemImage.color = collectionItem.itemImage.color;
            itemImage.material = collectionItem.itemImage.material;

            rarityColor = collectionItem.GetRarityColor();
            rarityBorder.color = rarityColor;
        }
        else if (r < 0.75f) // modifier
        {
            type = itemType.Modifier;
            mod = ItemDatabase.instance.RandomModifier();

            itemImage.sprite = mod.modifierImage.sprite;
            itemImage.color = mod.modifierImage.color;
            itemImage.material = mod.modifierImage.material;

            rarityBorder.color = mod.GetRarityColor();
        }
        else // currency
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

        BattlepassUnlock();
    }

    public void BattlepassUnlock()
    {
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected)
            MoveImageAndBorderUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
            MoveImageAndBorderDown();
    }

    public void Deselect()
    {
        if (selected)
            MoveImageAndBorderDown();
        selected = false;
    }

    void MoveImageAndBorderUp()
    {
        float distance = 20f;
        itemImage.transform.position += new Vector3(0, distance, 0);
        rarityBorder.transform.position += new Vector3(0, distance, 0);
        lockedOverlay.transform.position += new Vector3(0, distance, 0);
        claimedOverlay.transform.position += new Vector3(0, distance, 0);
        
    }

    void MoveImageAndBorderDown()
    {
        float distance = 20f;
        itemImage.transform.position -= new Vector3(0, distance, 0);
        rarityBorder.transform.position -= new Vector3(0, distance, 0);
        lockedOverlay.transform.position -= new Vector3(0, distance, 0);
        claimedOverlay.transform.position -= new Vector3(0, distance, 0);
    }
}
