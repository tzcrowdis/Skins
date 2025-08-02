using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayedSkin : MonoBehaviour
{
    [Header("Skin Info")]
    public GameObject skinCanvas;
    public TMP_Text skinName;
    public Outline outline;
    Skin skin;

    [Header("Audio")]
    public AudioClip hoverSound;
    [HideInInspector] public AudioSource hoverSource;
    public AudioClip clickSound;
    [HideInInspector] public AudioSource clickSource;

    bool bossFight = false;
    Vector3 startScale;


    void Start()
    {
        startScale = transform.localScale;
        skinCanvas.SetActive(false);
    }

    public void SetSkin(Skin newSkin, bool boss = false)
    {
        skin = newSkin;
        skinName.text = skin.itemName;
        outline.effectColor = skin.GetRarityColor();

        bossFight = boss;
    }

    void OnMouseEnter()
    {
        skinCanvas.SetActive(true);

        if (bossFight)
        {
            transform.localScale = startScale * 1.1f;
            hoverSource.PlayOneShot(hoverSound);
        }
    }

    void OnMouseExit()
    {
        skinCanvas.SetActive(false);

        if (bossFight)
        {
            transform.localScale = startScale;
        }
    }

    void OnMouseDown()
    {
        if (bossFight)
        {
            switch (EnemyController.instance.boss)
            {
                case EnemyController.BossType.Randomizer:
                    EnemyController.instance.randomizerBoss.GetComponent<RandomizerBoss>().lockSkin = true;
                    EnemyController.instance.randomizerBoss.GetComponent<RandomizerBoss>().ReactToChosenSkin();
                    break;
                case EnemyController.BossType.EvilRandomizer:
                    EnemyController.instance.evilRandomizer.GetComponent<EvilRandomizer>().lockSkin = true;
                    EnemyController.instance.evilRandomizer.GetComponent<EvilRandomizer>().ReactToChosenSkin();
                    break;
                case EnemyController.BossType.MonkeyPaw:
                    EnemyController.instance.monkeyPaw.GetComponent<MonkeyPaw>().SetSkin(skin);
                    EnemyController.instance.monkeyPaw.GetComponent<MonkeyPaw>().ReactToChosenSkin();
                    break;
            }

            Play.instance.endMatchButton.gameObject.SetActive(true);
            transform.localScale = startScale;
            bossFight = false;

            clickSource.PlayOneShot(clickSound);
        }   
    }
}
