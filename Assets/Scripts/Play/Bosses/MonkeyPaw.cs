using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonkeyPaw : Boss
{
    [Header("Background Particles")]
    public ParticleSystem backgroundParticles;

    [Header("Skin Options")]
    public GameObject[] skinOptions;
    public SpriteRenderer[] skinOptionsRenderers;
    public TMP_Text[] skinOptionsXp;
    public PlayedSkin[] skinOptionsPlayed;
    
    protected override void Start()
    {
        base.Start();
    }

    public void SetSkinOptions()
    {
        for (int i = 0; i < skinOptions.Length; i++)
        {
            Skin newSkin = ItemDatabase.instance.RandomSkinRandomCollection();
            skinOptionsRenderers[i].color = newSkin.itemImage.color;
            skinOptionsRenderers[i].material = newSkin.itemImage.material;
            skinOptionsRenderers[i].sprite = newSkin.itemImage.sprite;

            skinOptionsPlayed[i].SetSkin(newSkin, true);

            int optionExp = 0;
            switch (newSkin.rarity)
            {
                case CollectionItem.Rarity.VeryCommon:
                    optionExp = -5;
                    negExp = optionExp * 10;
                    break;
                case CollectionItem.Rarity.Common:
                    optionExp = -10;
                    negExp = optionExp * 4;
                    break;
                case CollectionItem.Rarity.Rare:
                    optionExp = -15;
                    negExp = optionExp * 2;
                    break;
                case CollectionItem.Rarity.Legendary:
                    optionExp = -25;
                    negExp = optionExp * 1;
                    break;
            }
            skinOptionsXp[i].text = $"{optionExp}xp";
        }
    }

    public override void SetSkin(Skin newSkin)
    {
        expText.gameObject.SetActive(true);
        playedSkin.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        skinSpriteRenderer.color = Color.white;

        base.SetSkin(newSkin);

        switch (newSkin.rarity)
        {
            case CollectionItem.Rarity.VeryCommon:
                negExp *= 10;
                break;
            case CollectionItem.Rarity.Common:
                negExp *= 4;
                break;
            case CollectionItem.Rarity.Rare:
                negExp *= 2;
                break;
            case CollectionItem.Rarity.Legendary:
                negExp *= 1;
                break;
        }
        expText.text = $"{negExp}xp";

        foreach (GameObject skinOption in skinOptions)
        {
            skinOption.SetActive(false);
        }
    }

    public override void ReactToPlayerSkin(Skin playerSkin)
    {
        dialogue.text = "Choose wisely...";
        dialogueCanvas.gameObject.SetActive(true);
    }

    public void EnableParticleEffects()
    {
        Color color1, color2, color3, color4;
        ColorUtility.TryParseHtmlString("#520000", out color1);
        ColorUtility.TryParseHtmlString("#FEFEFE", out color2);
        ColorUtility.TryParseHtmlString("#A29EFE", out color3);
        ColorUtility.TryParseHtmlString("#A5A6A5", out color4);

        Gradient gradient1 = new Gradient();
        gradient1.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0f), new GradientColorKey(color2, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        Gradient gradient2 = new Gradient();
        gradient2.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color3, 0f), new GradientColorKey(color4, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        var mainModule = backgroundParticles.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(gradient1, gradient2);
    }

    void OnEnable()
    {
        skinSpriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        expText.gameObject.SetActive(false);
        playedSkin.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnDisable()
    {
        var mainModule = backgroundParticles.main;
        mainModule.startColor = Color.gray;
    }
}
