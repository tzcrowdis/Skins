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

    [Header("Detailed Panel")]
    public GameObject modifierDetailPanel;
    Transform modifierCanvas;

    [Header("Hover Effects")]
    public Outline outline;
    [HideInInspector]
    public AudioSource buttonHover;
    public AudioClip hoverSound;
    AudioSource buttonClick;
    public AudioClip clickSound;

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

        modifierDetailPanel.SetActive(false);
        modifierCanvas = GameObject.Find("Modifier Canvas").transform;

        outline.enabled = false;
        buttonHover = GameObject.Find("Button Hover Audio Source").GetComponent<AudioSource>();
        buttonClick = GameObject.Find("Button Click Audio Source").GetComponent<AudioSource>();
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

    public void DeleteModifier()
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

        modifierDetailPanel.transform.SetParent(transform);

        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!modifierDetailPanel.activeSelf)
            modifierNamePanel.SetActive(true);

        outline.enabled = true;
        buttonHover.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!modifierDetailPanel.activeSelf)
            modifierNamePanel.SetActive(false);

        outline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!modifierDetailPanel.activeSelf)
        {
            modifierDetailPanel.SetActive(true);
            modifierDetailPanel.transform.SetParent(modifierCanvas);
        }
        else
        {
            modifierDetailPanel.SetActive(false);
            modifierDetailPanel.transform.SetParent(transform);
        }

        buttonClick.PlayOneShot(clickSound);
    }
}
