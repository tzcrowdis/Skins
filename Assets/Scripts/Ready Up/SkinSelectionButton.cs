using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinSelectionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Skin skin;

    [Header("Skin Info")]
    public GameObject skinInfoCanvas;
    public TMP_Text skinName;
    public Outline skinRarityOutline;

    [Header("Sounds")]
    public AudioClip hoverSound;
    [HideInInspector] public AudioSource hoverSource;
    public AudioClip clickSound;
    [HideInInspector] public AudioSource clickSource;


    public void OnPointerClick(PointerEventData eventData)
    {
        ReadyUp.instance.SelectSkin(skin);
        clickSource.PlayOneShot(clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
        skinInfoCanvas.SetActive(true);
        hoverSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        skinInfoCanvas.SetActive(false);
    }

    public void PopulateSkinInfo()
    {
        skinName.text = skin.itemName;
        skinRarityOutline.effectColor = skin.GetRarityColor();
        skinInfoCanvas.SetActive(false);
    }
}
