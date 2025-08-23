using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : CollectionItem
{
    public enum Collection
    {
        Color,
        Gradient,
        Camo,
        Cat,
        Art
    }

    [Header("Collection This Skin Belongs To")]
    public Collection collection;

    private void Awake()
    {
        itemCost = GenerateSkinCost();
    }

    protected override void Start()
    {
        base.Start();

        //itemCost = GenerateSkinCost();
    }

    public int GetSkinExp()
    {
        switch (rarity)
        {
            case Skin.Rarity.VeryCommon:
                return 100;
            case Skin.Rarity.Common:
                return 200;
            case Skin.Rarity.Rare:
                return 300;
            case Skin.Rarity.Legendary:
                return 400;
        }

        return 0;
    }

    // NOTE better than writing the prices individually but should have a location in the editor
    public int GenerateSkinCost() 
    {
        switch (rarity)
        {
            case Skin.Rarity.VeryCommon:
                return 250;
            case Skin.Rarity.Common:
                return 500;
            case Skin.Rarity.Rare:
                return 750;
            case Skin.Rarity.Legendary:
                return 1000;
        }

        return 0;
    }
}
