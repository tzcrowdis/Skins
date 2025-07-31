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
}
