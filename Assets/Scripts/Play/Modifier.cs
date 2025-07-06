using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Modifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Modifier Details")]
    public string modifierName;
    public Rarity modifierRarity;
    public Image modifierImage;
    public int modifierCost;

    [Header("Name Panel")]
    public GameObject modifierNamePanel;
    public TMP_Text nameText;

    [Header("Effect Panel")]
    public GameObject modifierEffectPanel;

    [Header("Delete")]
    public Button deleteButton;

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    }

    void Start()
    {
        nameText.text = modifierName;

        modifierNamePanel.GetComponent<Outline>().effectColor = GetRarityColor();
        modifierNamePanel.SetActive(false);

        modifierEffectPanel.SetActive(false);

        deleteButton.onClick.AddListener(DeleteModifier);
        deleteButton.gameObject.SetActive(false);
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

    void DeleteModifier()
    {
        Player.instance.RemoveFromModifierList(this);
        
        // make all store/battlepass modifiers interactable before destroying
        // TODO review modifier locking in battlepass
        foreach (Transform bpItem in Battlepass.instance.bpContent.transform)
        {
            BattlepassItem.itemType type = bpItem.GetComponent<BattlepassItem>().type;
            if (type == BattlepassItem.itemType.Modifier)
            {
                BattlepassItem modBPItem = bpItem.GetComponent<BattlepassItem>();
                if (!modBPItem.locked & !modBPItem.claimed & modBPItem.lockedOverlay.activeSelf)
                {
                    modBPItem.lockedOverlay.SetActive(true);
                    modBPItem.locked = true;
                }  
            }
        }

        foreach (Transform storeItem in Store.instance.featuredPanel.transform)
        {
            StoreButton.itemType type = storeItem.GetComponent<StoreButton>().type;
            if (type == StoreButton.itemType.Modifier)
                storeItem.GetComponent<Button>().interactable = true;
        }

        modifierEffectPanel.transform.SetParent(transform);
        deleteButton.transform.SetParent(transform);

        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        modifierNamePanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        modifierNamePanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!modifierEffectPanel.activeSelf)
        {
            modifierEffectPanel.SetActive(true);
            modifierEffectPanel.transform.SetParent(Home.instance.transform);

            deleteButton.gameObject.SetActive(true);
            deleteButton.transform.SetParent(Home.instance.transform);
        }
        else
        {
            modifierEffectPanel.SetActive(false);
            modifierEffectPanel.transform.SetParent(transform);

            deleteButton.gameObject.SetActive(false);
            deleteButton.transform.SetParent(transform);
        }
    }
}
