using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour
{
    [Header("Item")]
    public GameObject item;
    public int itemCost;

    [Header("Display")]
    public TMP_Text itemName;
    public Image image;
    public TMP_Text cost;

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

    public void RandomizeItem()
    {
        discount = Random.Range(minDiscount, maxDiscount);
        int numSteps = (int)Mathf.Floor(discount / stepSize);
        discount = numSteps * stepSize;
        discountText.text = $"-{discount}%";

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

                if (Player.instance.modifiers.Count == Player.instance.modifierCapacity)
                    transform.GetChild(0).GetComponent<Button>().interactable = false;
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
}
