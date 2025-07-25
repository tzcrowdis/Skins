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
    public GameObject evilRandomizer;
    public GameObject monkeyPaw;

    int negExp;

    public enum BossType
    {
        Randomizer,
        EvilRandomizer,
        MonkeyPaw
    }
    
    public static EnemyController instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void Update()
    {
        if (bossFight )
        {
            if (boss == BossType.Randomizer && randomizerBoss.GetComponent<RandomizerBoss>().transition)
                randomizerBoss.GetComponent<RandomizerBoss>().TransitionParticleSimSpeed();
            else if (boss == BossType.EvilRandomizer && evilRandomizer.GetComponent<EvilRandomizer>().transition)
                evilRandomizer.GetComponent<EvilRandomizer>().TransitionParticleSimSpeed();
        }   
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
                case BossType.EvilRandomizer:
                    evilRandomizer.SetActive(true);
                    break;
                case BossType.MonkeyPaw:
                    monkeyPaw.SetActive(true);
                    monkeyPaw.GetComponent<MonkeyPaw>().SetSkinOptions();
                    break;
            }

            GenerateEnemyReactions();
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
        SetNegativeExp();
        
        foreach (Transform child in transform)
        {
            child.GetComponent<Enemy>().dialogueCanvas.gameObject.SetActive(false);
        }

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void SetNegativeExp()
    {
        negExp = 0;
        foreach (Transform child in transform)
        {
            Enemy nme = child.GetComponent<Enemy>();
            if (nme & nme.gameObject.activeSelf)
                negExp += nme.negExp;
        }
    }

    public int GetNegativeExp()
    {
        return negExp;
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
            //if (child.gameObject.CompareTag("Boss"))
            //    continue;
            
            Enemy nme = child.GetComponent<Enemy>();
            if (nme & nme.gameObject.activeSelf)
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
            case BossType.EvilRandomizer:
                evilRandomizer.GetComponent<EvilRandomizer>().EnableParticleEffects();
                break;
            case BossType.MonkeyPaw:
                monkeyPaw.GetComponent<MonkeyPaw>().EnableParticleEffects();
                break;
        }
    }

    public void RemoveBoss()
    {
        bossFight = false;
    }
}
