using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item")]
    public GameObject item;
    public int itemCost;
    public bool purchased = false;
    bool modifierLock = false;

    [Header("Display")]
    public TMP_Text itemName;
    public Image image;
    public TMP_Text cost;
    public GameObject modDescPanel;
    public TMP_Text modDesc;

    [Header("Discount")]
    public float discount;
    public TMP_Text discountText;
    public float minDiscount;
    public float maxDiscount;
    int stepSize = 5;

    [Header("Item Type")]
    public itemType type;
    public enum itemType
    {
        Skin,
        Crate,
        Modifier
    }

    void Start()
    {
        modDescPanel.SetActive(false);
    }

    public void RandomizeItem()
    {
        discount = Random.Range(minDiscount, maxDiscount);
        int numSteps = (int)Mathf.Floor(discount / stepSize);
        discount = numSteps * stepSize;
        discountText.text = $"-{discount}%";
        purchased = false;

        switch (type)
        {
            case itemType.Skin:
                Skin skin = ItemDatabase.instance.RandomSkinRandomCollection();
                item = skin.gameObject;

                itemName.text = skin.itemName;
                itemCost = (int)(skin.itemCost - skin.itemCost * discount / 100);
                cost.text = $"\u0424{itemCost}";
                image.color = skin.itemImage.color;
                image.material = skin.itemImage.material;
                if (skin.itemImage.sprite) image.sprite = skin.itemImage.sprite;
                break;

            case itemType.Modifier:
                Modifier mod = ItemDatabase.instance.RandomModifier();
                item = mod.gameObject;

                itemName.text = mod.modifierName;
                itemCost = (int)(mod.modifierCost - mod.modifierCost * discount / 100);
                cost.text = $"\u0424{itemCost}";
                image.color = mod.modifierImage.color;
                image.material = mod.modifierImage.material;
                if (mod.modifierImage.sprite) image.sprite = mod.modifierImage.sprite;
                modDesc.text = mod.modifierDescription.text;

                if (Player.instance.modifiers.Count == Player.instance.modifierCapacity)
                    LockModifiersFull();
                break;

            case itemType.Crate:
                Crate crate = ItemDatabase.instance.RandomCrate();
                item = crate.gameObject;

                itemName.text = crate.itemName;
                itemCost = (int)(crate.itemCost - crate.itemCost * discount / 100);
                cost.text = $"\u0424{itemCost}";
                image.color = crate.itemImage.color;
                image.material = crate.itemImage.material;
                if (crate.itemImage.sprite) image.sprite = crate.itemImage.sprite;
                break;
        }
    }

    public void SetPurchased(bool success)
    {
        purchased = success;
        if (!purchased)
            return;

        itemName.text = "";
        image.sprite = null;
        image.material = null;
        image.color = Color.black;
        discountText.text = "";
        cost.text = "";
    }

    public void LockModifiersFull()
    {
        modifierLock = true;

        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }

    public void UnlockModifiersNotFull()
    {
        modifierLock = false;

        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!purchased && !modifierLock)
        {
            Store.instance.OpenPurchasePanel(this);

            if (type == itemType.Modifier)
            {
                modDescPanel.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!purchased && !modifierLock)
        {
            transform.localScale += Vector3.one * 0.05f;

            if (type == itemType.Modifier)
            {
                modDescPanel.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!purchased && !modifierLock)
        {
            transform.localScale -= Vector3.one * 0.05f;

            if (type == itemType.Modifier)
            {
                modDescPanel.SetActive(false);
            }
        }
    }
}
