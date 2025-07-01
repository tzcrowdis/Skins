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
    public GameObject collectionItemObject;
    CollectionItem collectionItem;

    [Header("Modifier")]
    public GameObject modifierObject;

    [Header("Currency")]
    public int coinAmount;

    [Header("Premium Lock")]
    public bool locked;
    public TMP_Text lockedLevel;
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
        collectionItem = collectionItemObject.GetComponent<CollectionItem>();

        itemImage.sprite = collectionItem.itemImage.sprite;
        itemImage.color = collectionItem.itemImage.color;
        itemImage.material = collectionItem.itemImage.material;

        rarityColor = collectionItem.GetRarityColor();
        rarityBorder.color = rarityColor;

        lockedOverlay.SetActive(locked);

        for (int i = 0; i < Battlepass.instance.bpContent.transform.childCount; i++)
        {
            BattlepassItem child = Battlepass.instance.bpContent.transform.GetChild(i).GetComponent<BattlepassItem>();
            if (child == this)
            {
                levelToClaim = i;
                break;
            }   
        }
        claimedOverlay.SetActive(claimed);
    }

    public void UnlockItem()
    {
        locked = false;
        lockedOverlay.SetActive(locked);

        if (selected)
            BattlepassInfoPanel.instance.UnlockItem();
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
                    Player.instance.AddToModifierList(modifierObject.GetComponent<Modifier>());
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
        BattlepassInfoPanel.instance.DisplayBattlepassInfoPanel(collectionItem, this);
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
