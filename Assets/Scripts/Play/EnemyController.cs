using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Skin Collection Paths")]
    public List<string> collectionPaths;
    
    
    public static EnemyController instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void StartPlay()
    {
        Player.instance.ApplyAllModifiers();
        GenerateEnemySkins();
        GenerateEnemyReactions();
    }

    void GenerateEnemySkins()
    {
        foreach (Transform child in transform)
        {
            Enemy nme = child.GetComponent<Enemy>();
            if (nme)
            {
                // Strategy: random skin from random collection
                int collectionIndex = Random.Range(0, collectionPaths.Count);
                string[] guids = AssetDatabase.FindAssets("t:Object", new[] { collectionPaths[collectionIndex] });
                int skinIndex = Random.Range(0, guids.Length);
                Skin enemySkin = (Skin)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[skinIndex]), typeof(Skin));
                nme.SetSkin(enemySkin);

                // TODO Strategy: select skin in collection based on rarity
            }
        }
    }

    void GenerateEnemyReactions()
    {
        // TODO
    }
}
