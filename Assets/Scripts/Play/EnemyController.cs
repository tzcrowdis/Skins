using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnemyController;

public class EnemyController : MonoBehaviour
{
    [Header("Bosses")]
    public bool bossFight = false;
    public BossType boss;
    public GameObject randomizerBoss;

    public enum BossType
    {
        Randomizer
    }
    
    public static EnemyController instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void StartPlay()
    {
        if (bossFight)
        {
            switch (boss)
            {
                case BossType.Randomizer:
                    randomizerBoss.SetActive(true);
                    break;
            }
        }
        else
        {
            foreach (Transform child in transform)
                if (!child.gameObject.CompareTag("Boss")) child.gameObject.SetActive(true);

            GenerateEnemySkins();
            GenerateEnemyReactions();
        }  
    }

    public void EndPlay()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Enemy>().dialogueCanvas.gameObject.SetActive(false);
        }

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void GenerateEnemySkins()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Boss"))
                continue;

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
            if (child.gameObject.CompareTag("Boss"))
                continue;
            
            Enemy nme = child.GetComponent<Enemy>();
            if (nme)
            {
                nme.ReactToPlayerSkin(playerSkin);
            }
        }
    }

    public void QueueBoss(BossType bossType)
    {
        bossFight = true;
        boss = bossType;

        switch (boss)
        {
            case BossType.Randomizer:
                randomizerBoss.GetComponent<RandomizerBoss>().EnableParticleEffects();
                break;
        }
    }

    public void RemoveBoss()
    {
        bossFight = false;
    }
}
