using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void SetSkin(Skin newSkin)
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
                negExp = -5;
                break;
            case CollectionItem.Rarity.Common:
                negExp = -10;
                break;
            case CollectionItem.Rarity.Rare:
                negExp = -15;
                break;
            case CollectionItem.Rarity.Legendary:
                negExp = -25;
                break;
        }
        
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
            dialogue.text = "Impressive, but check out mine... it's more rare.";
        }
        else if (skin.rarity < playerSkin.rarity)
        {
            dialogue.text = "That's okay I guess, for you.";
        }

        // TODO more dialogue options

        if (dialogue.text != "")
        {
            dialogueCanvas.gameObject.SetActive(true);
        }   
    }
}
