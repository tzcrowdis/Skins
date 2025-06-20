using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Modifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Modifier Details")]
    public string modifierName;
    public Rarity modifierRarity;

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

    void Start()
    {
        nameText.text = modifierName;
        rarityText.text = modifierRarity.ToString();

        switch (modifierRarity)
        {
            case Rarity.Common:
                rarityText.color = Color.blue;
                break;
            case Rarity.Rare:
                rarityText.color = Color.magenta;
                break;
            case Rarity.Legendary:
                rarityText.color = Color.yellow;
                break;
        }

        modifierInfoPanel.SetActive(false);
    }
    
    public virtual void ModifierEffect() { /* TODO override */ }

    public void OnPointerEnter(PointerEventData eventData)
    {
        modifierInfoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        modifierInfoPanel.SetActive(false);
    }
}
