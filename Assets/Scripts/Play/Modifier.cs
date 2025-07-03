using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Modifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Modifier Details")]
    public string modifierName;
    public Rarity modifierRarity;
    public Image modifierImage;
    public int modifierCost;

    [Header("Info Panel")]
    public GameObject modifierInfoPanel;
    public TMP_Text nameText;
    public TMP_Text rarityText;

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    }

    // TODO on delete, make all store/battlepass modifiers interactable

    void Start()
    {
        nameText.text = modifierName;
        rarityText.text = modifierRarity.ToString();

        rarityText.color = GetRarityColor();

        modifierInfoPanel.SetActive(false);
    }
    
    public virtual void ModifierEffect() { /* TODO override */ }

    public int GetLootPoolMult()
    {
        switch (modifierRarity)
        {
            case Rarity.Common:
                return 10;
            case Rarity.Rare:
                return 2;
            case Rarity.Legendary:
                return 1;
        }
        return 0;
    }

    public Color GetRarityColor()
    {
        switch (modifierRarity)
        {
            case Rarity.Common:
                return Color.blue;
            case Rarity.Rare:
                return Color.magenta;
            case Rarity.Legendary:
                return Color.yellow;
        }

        return Color.black;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        modifierInfoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        modifierInfoPanel.SetActive(false);
    }
}
