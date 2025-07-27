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

    bool monkeyPaw = false;
    Vector3 startScale;


    void Start()
    {
        startScale = transform.localScale;
        skinCanvas.SetActive(false);
    }

    public void SetSkin(Skin newSkin, bool monkey = false)
    {
        skin = newSkin;
        skinName.text = skin.itemName;
        outline.effectColor = skin.GetRarityColor();

        monkeyPaw = monkey;
    }

    void OnMouseEnter()
    {
        skinCanvas.SetActive(true);

        if (monkeyPaw)
        {
            transform.localScale = startScale * 1.1f;
            hoverSource.PlayOneShot(hoverSound);
        }
    }

    void OnMouseExit()
    {
        skinCanvas.SetActive(false);

        if (monkeyPaw)
        {
            transform.localScale = startScale;
        }
    }

    void OnMouseDown()
    {
        if (monkeyPaw)
        {
            GameObject.Find("Monkey Paw").GetComponent<MonkeyPaw>().SetSkin(skin);
            clickSource.PlayOneShot(clickSound);
        }   
    }
}
