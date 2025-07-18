using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemDatabase : MonoBehaviour
{
    [Header("Skin Paths")]
    public List<string> skinPaths;

    [Header("Crates Path")]
    public string cratesPath;

    [Header("Modifiers Path")]
    public string modifiersPath;

    [Header("Coin Image")]
    public Sprite coin;


    public static ItemDatabase instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public Skin RandomSkinRandomCollection()
    {
        int collectionIndex = Random.Range(0, skinPaths.Count);
        Skin[] skins = Resources.LoadAll<Skin>(skinPaths[collectionIndex]);
        int skinIndex = Random.Range(0, skins.Length);
        return skins[skinIndex];
    }

    public Skin RandomSkinRandomCollection(Skin.Rarity rarity)
    {
        int collectionIndex = Random.Range(0, skinPaths.Count);
        Skin[] skins = Resources.LoadAll<Skin>(skinPaths[collectionIndex]);

        // sort skins by rarity
        List<Skin> veryCommonSkins = new List<Skin>();
        List<Skin> commonSkins = new List<Skin>();
        List<Skin> rareSkins = new List<Skin>();
        List<Skin> legendarySkins = new List<Skin>();
        foreach (Skin skin in skins)
        {
            switch (skin.rarity)
            {
                case Skin.Rarity.VeryCommon:
                    veryCommonSkins.Add(skin);
                    break;
                case Skin.Rarity.Common:
                    commonSkins.Add(skin);
                    break;
                case Skin.Rarity.Rare:
                    rareSkins.Add(skin);
                    break;
                case Skin.Rarity.Legendary:
                    legendarySkins.Add(skin);
                    break;
            }
        }

        // random skin from rarity
        int skinIndex;
        switch (rarity)
        {
            case Skin.Rarity.VeryCommon:
                skinIndex = Random.Range(0, veryCommonSkins.Count);
                return veryCommonSkins[skinIndex];
            case Skin.Rarity.Common:
                skinIndex = Random.Range(0, commonSkins.Count);
                return commonSkins[skinIndex];
            case Skin.Rarity.Rare:
                skinIndex = Random.Range(0, rareSkins.Count);
                return rareSkins[skinIndex];
            case Skin.Rarity.Legendary:
                skinIndex = Random.Range(0, legendarySkins.Count); // TODO error bc no legendary skin in solid color collection
                return legendarySkins[skinIndex];
        }

        return null;
    }

    public Crate RandomCrate()
    {
        Crate[] crates = Resources.LoadAll<Crate>(cratesPath);
        int crateIndex = Random.Range(0, crates.Length);
        return crates[crateIndex];
    }

    public Modifier RandomModifier()
    {
        Modifier[] modifiers = Resources.LoadAll<Modifier>(modifiersPath);
        int modifierIndex = Random.Range(0, modifiers.Length);
        return modifiers[modifierIndex];
    }

    public Image GetCoinImage()
    {
        GameObject temp = new GameObject("temp from get coin image");
        Image coinImage = temp.AddComponent<Image>();
        coinImage.sprite = coin;
        Destroy(temp);
        return coinImage;
    }
}
