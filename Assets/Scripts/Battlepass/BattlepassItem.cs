using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattlepassItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Collection Item")]
    public GameObject collectionItemObject;
    CollectionItem collectionItem;

    [Header("Premium Lock")]
    public bool locked;
    public GameObject lockedOverlay;

    [Header("Image + Border")]
    public Image itemImage;
    public Image rarityBorder;

    [Header("Claim Requirement")]
    public bool claimed = false;
    public int matchesPlayedToClaim;
    public GameObject claimedPanel;

    Color rarityColor;
    bool selected = false;

    void Start()
    {
        collectionItem = collectionItemObject.GetComponent<CollectionItem>();

        itemImage.sprite = collectionItem.collectionItemImage.sprite;
        itemImage.color = collectionItem.collectionItemImage.color;
        itemImage.material = collectionItem.collectionItemImage.material;

        rarityColor = collectionItem.GetRarityColor();
        rarityBorder.color = rarityColor;

        lockedOverlay.SetActive(locked);

        //claimedPanel.SetActive(claimed);
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
        if (Player.instance.matchesPlayed >= matchesPlayedToClaim)
        {
            // TODO add item to collection or currency to player
            
            claimed = true;
            //claimedPanel.SetActive(claimed);
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
        itemImage.transform.position += new Vector3(0f, 20f, 0f);
        rarityBorder.transform.position += new Vector3(0f, 20f, 0f);
        lockedOverlay.transform.position += new Vector3(0f, 20f, 0f);
    }

    void MoveImageAndBorderDown()
    {
        itemImage.transform.position -= new Vector3(0f, 20f, 0f);
        rarityBorder.transform.position -= new Vector3(0f, 20f, 0f);
        lockedOverlay.transform.position -= new Vector3(0f, 20f, 0f);
    }
}
