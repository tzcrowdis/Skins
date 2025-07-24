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

    bool monkeyPaw = false;


    void Start()
    {
        skinCanvas.SetActive(false);
    }

    public void SetSkin(Skin newSkin, bool monkey = false)
    {
        skin = newSkin;
        skinName.text = skin.itemName;
        outline.effectColor = skin.GetRarityColor();

        monkeyPaw = monkey;
    }

    void OnMouseOver()
    {
        skinCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        skinCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        if (monkeyPaw)
            GameObject.Find("Monkey Paw").GetComponent<MonkeyPaw>().SetSkin(skin);
    }
}
