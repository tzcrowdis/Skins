using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("Skin")]
    public SpriteRenderer skinSpriteRenderer;
    [HideInInspector]
    public Skin skin;

    [Header("Play Canvas")]
    public Canvas playCanvas;
    
    void Start()
    {
        
    }

    public void SetSkin(Skin newSkin)
    {
        skin = newSkin;
        skinSpriteRenderer.color = skin.itemImage.color;
        skinSpriteRenderer.material = skin.itemImage.material;
        if (skin.itemImage.sprite)
            skinSpriteRenderer.sprite = skin.itemImage.sprite;
    }

    public void ReactToPlayerSkin(Skin player)
    {
        // TODO
    }

    // TODO display skin info on hover
}
