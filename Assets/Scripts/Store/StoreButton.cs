using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour
{
    [Header("Item")]
    public Image image;
    public TMP_Text cost;

    void Start()
    {
        image = GetComponent<Image>();
        cost = GetComponentsInChildren<TMP_Text>()[1];
    }
}
