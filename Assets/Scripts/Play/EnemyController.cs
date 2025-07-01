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

    public void EndPlay()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Enemy>().dialogueCanvas.gameObject.SetActive(false);
        }
    }

    void GenerateEnemySkins()
    {
        foreach (Transform child in transform)
        {
            Enemy nme = child.GetComponent<Enemy>();
            if (nme)
            {
                Skin enemySkin = ItemDatabase.instance.RandomSkinRandomCollection();
                nme.SetSkin(enemySkin);
            }
        }
    }

    void GenerateEnemyReactions()
    {
        Skin playerSkin = Player.instance.skin;
        foreach (Transform child in transform)
        {
            Enemy nme = child.GetComponent<Enemy>();
            if (nme)
            {
                nme.ReactToPlayerSkin(playerSkin);
            }
        }
    }
}
