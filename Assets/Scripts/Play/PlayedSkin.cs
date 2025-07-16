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


    void Start()
    {
        skinCanvas.SetActive(false);
    }

    public void SetSkin(Skin newSkin)
    {
        skin = newSkin;
        skinName.text = skin.itemName;
        outline.effectColor = skin.GetRarityColor();
    }

    void OnMouseOver()
    {
        skinCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        skinCanvas.SetActive(false);
    }
}
