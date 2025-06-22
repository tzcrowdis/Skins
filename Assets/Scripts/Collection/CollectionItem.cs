using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class CollectionItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Image")]
    public Image itemImage;
    
    [Header("Name")]
    public string itemName;
    
    [Header("Rarity")]
    public Rarity rarity;
    public Image rarityBorder;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public TMP_Text nameText;
    public TMP_Text rarityText;

    Transform collectionCanvas; // HACK to get around render order

    public enum Rarity
    {
        VeryCommon,
        Common,
        Rare,
        Legendary
    }

    protected virtual void Start()
    {
        collectionCanvas = GameObject.Find("Collection Canvas").transform;
        
        nameText.text = itemName;
        rarityText.text = ObjectNames.NicifyVariableName(rarity.ToString());

        SetRarityColor();

        infoPanel.SetActive(false);
    }

    void SetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.VeryCommon:
                rarityBorder.color = Color.white;
                rarityText.color = Color.white;
                break;
            case Rarity.Common:
                rarityBorder.color = Color.blue;
                rarityText.color = Color.blue;
                break;
            case Rarity.Rare:
                rarityBorder.color = Color.magenta;
                rarityText.color = Color.magenta;
                break;
            case Rarity.Legendary:
                rarityBorder.color = Color.yellow;
                rarityText.color = Color.yellow;
                break;
        }
    }

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.VeryCommon:
                return Color.white;
            case Rarity.Common:
                return Color.blue;
            case Rarity.Rare:
                return Color.magenta;
            case Rarity.Legendary:
                return Color.yellow;
        }

        return Color.black;
    }

    public int GetLootPoolMult()
    {
        switch (rarity)
        {
            case Rarity.VeryCommon:
                return 20;
            case Rarity.Common:
                return 10;
            case Rarity.Rare:
                return 2;
            case Rarity.Legendary:
                return 1;
        }
        return 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);
        infoPanel.transform.SetParent(collectionCanvas);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.transform.SetParent(transform);
        infoPanel.SetActive(false);
    }
}
