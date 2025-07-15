using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    public Skin RandomSkinRandomCollection() // TODO rework with Resources.LoadAll
    {
        int collectionIndex = Random.Range(0, skinPaths.Count);
        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { skinPaths[collectionIndex] });
        int skinIndex = Random.Range(0, guids.Length);
        return (Skin)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[skinIndex]), typeof(Skin));
    }

    // TODO select skin in collection based on rarity
    public Skin RandomSkinRandomCollection(Skin.Rarity rarity) // TODO rework with Resources.LoadAll
    {
        int collectionIndex = Random.Range(0, skinPaths.Count);
        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { skinPaths[collectionIndex] });
        int skinIndex = Random.Range(0, guids.Length);
        return (Skin)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[skinIndex]), typeof(Skin));
    }

    public Crate RandomCrate() // TODO rework with Resources.LoadAll
    {
        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { cratesPath });
        int crateIndex = Random.Range(0, guids.Length);
        return (Crate)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[crateIndex]), typeof(Crate));
    }

    public Modifier RandomModifier() // TODO rework with Resources.LoadAll
    {
        string[] guids = AssetDatabase.FindAssets("t:Object", new[] { modifiersPath });
        int modifierIndex = Random.Range(0, guids.Length);
        return (Modifier)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[modifierIndex]), typeof(Modifier));
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
