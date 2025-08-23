using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Skin")]
    public SpriteRenderer skinSpriteRenderer;
    [HideInInspector]
    public Skin skin;

    [Header("Skin Info")]
    public PlayedSkin playedSkin;

    [Header("Dialogue")]
    public Canvas dialogueCanvas;
    public TMP_Text dialogue;
    public RectTransform dialogueBoxBackground;

    [Header("Play Canvas")]
    public Canvas playCanvas;

    [Header("XP")]
    public TMP_Text expText;
    [HideInInspector] public int negExp;


    protected virtual void Start()
    {
        //dialogueCanvas.gameObject.SetActive(false);
    }

    public virtual void SetSkin(Skin newSkin)
    {
        skin = newSkin;
        skinSpriteRenderer.color = skin.itemImage.color;
        skinSpriteRenderer.material = skin.itemImage.material;
        if (skin.itemImage.sprite)
            skinSpriteRenderer.sprite = skin.itemImage.sprite;
        else
            skinSpriteRenderer.sprite = null;

        playedSkin.SetSkin(newSkin);

        SetExpAndText();
    }

    protected virtual void SetExpAndText()
    {
        switch (skin.rarity)
        {
            case CollectionItem.Rarity.VeryCommon:
                negExp = -10;
                break;
            case CollectionItem.Rarity.Common:
                negExp = -20;
                break;
            case CollectionItem.Rarity.Rare:
                negExp = -30;
                break;
            case CollectionItem.Rarity.Legendary:
                negExp = -40;
                break;
        }
        negExp *= Player.instance.season;
        
        expText.text = $"{negExp}xp";
    }

    public virtual void ReactToPlayerSkin(Skin playerSkin)
    {
        dialogue.text = "";
        
        if (playerSkin.name == skin.name)
        {
            float r = Random.Range(0f, 1f);
            if (r < 0.75f)
            {
                dialogue.text = "Same, \nnice!";
            }
            else
            {
                dialogue.text = "Copycat...";
            }
        }
        else if (skin.rarity > playerSkin.rarity)
        {
            float r = Random.Range(0f, 1f);
            if (r < 0.5f)
            {
                dialogue.text = "Impressive, but check out mine... higher rarity.";
            }
            else
            {
                dialogue.text = "The rarity of your skin pales in comparison to mine.";
            }
        }
        else if (skin.rarity < playerSkin.rarity)
        {
            float r = Random.Range(0f, 1f);
            if (r < 0.5f)
            {
                dialogue.text = "That's okay I guess, for you.";
            }
            else
            {
                dialogue.text = "Wow, that's more rarer than mine!";
            }
        }

        // TODO more dialogue options

        if (dialogue.text != "")
        {
            dialogueCanvas.gameObject.SetActive(true);
        }   
    }
}
