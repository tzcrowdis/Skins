using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Modifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Modifier Details")]
    public string modifierName;
    public Rarity modifierRarity;
    public Image modifierImage;
    public int modifierCost;
    public Type modifierType;
    public string modifierExpDescription;

    [Header("Name Panel")]
    public GameObject modifierNamePanel;
    public TMP_Text nameText;

    [Header("Detailed Panel")]
    public GameObject modifierDetailPanel;
    public TMP_Text modifierDescription;
    Transform modifierCanvas;

    [Header("Hover Effects")]
    public Outline outline;
    [HideInInspector]
    public AudioSource buttonHover;
    public AudioClip hoverSound;
    AudioSource buttonClick;
    public AudioClip clickSound;

    [Header("Drag")]
    public GameObject modifierDragPrefab;
    public GameObject modifierDrag;
    public bool lockDrag = false;

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    }

    public enum Type
    {
        ExpMult,
        ExpAdd,
        AlterEnemy
    }

    void Start()
    {
        nameText.text = modifierName;

        modifierNamePanel.GetComponent<Outline>().effectColor = GetRarityColor();
        modifierNamePanel.SetActive(false);

        modifierDetailPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(GetRarityColor())}>{modifierRarity.ToString()}</color>";
        modifierDetailPanel.SetActive(false);
        modifierCanvas = GameObject.Find("Modifier Canvas").transform;

        outline.enabled = false;
        buttonHover = GameObject.Find("Button Hover Audio Source").GetComponent<AudioSource>();
        buttonClick = GameObject.Find("Button Click Audio Source").GetComponent<AudioSource>();
    }
    
    public virtual bool ModifierEffect() { return false; /* override */ }

    public string ModifierExpDescription()
    {
        return modifierExpDescription;
    }

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
                return Color.cyan;
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
                storeItem.GetChild(0).GetComponent<Button>().interactable = true;
        }

        modifierDetailPanel.transform.SetParent(transform);

        Destroy(gameObject);
    }

    /*
     * POINTER
     */
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
            Transform modifiersPanel = modifierCanvas.GetChild(0);
            foreach (Transform modPanel in modifiersPanel)
            {
                Modifier modComp = modPanel.GetComponent<Modifier>();
                if (modComp && modComp.modifierDetailPanel.gameObject.activeSelf)
                    modComp.modifierDetailPanel.gameObject.SetActive(false);
            }
            
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

    /*
     * DRAG
     */
    // TODO lock drag during post match summary
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (lockDrag)
            return;

        modifierDrag = Instantiate(modifierDragPrefab, transform.parent.transform.parent); // parent is transform canvas... shameful
        Image img = modifierDrag.GetComponent<Image>();
        img.sprite = modifierImage.sprite;
        img.material = modifierImage.material;
        img.color = modifierImage.color;

        modifierDetailPanel.SetActive(false);
        modifierImage.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (lockDrag)
            return;

        // image tracks mouse position
        if (modifierDrag)
            modifierDrag.transform.position = eventData.position;

        // sort modifier based on panel objects positions (just set it to be the closest to the pointer?)
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;
        int i = 0;
        foreach (Transform child in Player.instance.modifierPanel.transform)
        {
            float modDist = Vector3.Distance(child.position, eventData.position);
            if (modDist < closestDistance)
            {
                closestDistance = modDist;
                closestIndex = i;
            }
            i++;
        }

        if (closestIndex != -1)
        {
            transform.SetSiblingIndex(closestIndex);
            Player.instance.ResortModifierList();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (lockDrag)
            return;

        Destroy(modifierDrag);
        modifierImage.enabled = true;
    }
}
