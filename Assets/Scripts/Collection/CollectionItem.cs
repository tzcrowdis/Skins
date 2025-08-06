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
    //public Outline rarityBorder;

    [Header("Cost")]
    public int itemCost;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public TMP_Text nameText;
    public TMP_Text rarityText;
    Vector3 defaultInfoPanelPosition;
    bool offscreen = true;

    public enum Rarity
    {
        VeryCommon,
        Common,
        Rare,
        Legendary
    }

    protected virtual void Start()
    {
        nameText.text = itemName;
        rarityText.text = ObjectNames.NicifyVariableName(rarity.ToString());

        SetRarityColor();

        defaultInfoPanelPosition = infoPanel.GetComponent<RectTransform>().anchoredPosition3D;
        infoPanel.SetActive(false);
    }

    void Update()
    {
        // adjust position if any corner is off screen
        if (infoPanel.activeSelf && offscreen)
        {
            RectTransform rect = infoPanel.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            foreach (Vector3 corner in corners)
            {
                // HACK first frame all corners are equal (not sure why) so skip this frame
                if (corner == corners[corners.Length - 1])
                    break;
                
                if (corner.x > Screen.width) // assumes off in +x direction
                {
                    Vector3 offset = Vector3.zero;
                    offset.x = corner.x - Screen.width;
                    infoPanel.GetComponent<RectTransform>().anchoredPosition3D -= offset;
                    offscreen = false;
                }

                if (corner.y < 0) // assumes off in -y direction
                {
                    Vector3 offset = Vector3.zero;
                    offset.y =  -corner.y;
                    infoPanel.GetComponent<RectTransform>().anchoredPosition3D += offset;
                    offscreen = false;
                }
            }
        }
    }

    void SetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.VeryCommon:
                //rarityBorder.effectColor = Color.white;
                rarityText.color = Color.white;
                break;
            case Rarity.Common:
                //rarityBorder.effectColor = Color.cyan;
                rarityText.color = Color.cyan;
                break;
            case Rarity.Rare:
                //rarityBorder.effectColor = Color.magenta;
                rarityText.color = Color.magenta;
                break;
            case Rarity.Legendary:
                //rarityBorder.effectColor = Color.yellow;
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
                return Color.cyan;
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
        infoPanel.transform.SetParent(Collection.instance.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.transform.SetParent(transform);
        infoPanel.GetComponent<RectTransform>().anchoredPosition3D = defaultInfoPanelPosition;
        offscreen = true;
        infoPanel.SetActive(false);
    }
}
