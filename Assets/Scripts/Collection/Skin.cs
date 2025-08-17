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
}
