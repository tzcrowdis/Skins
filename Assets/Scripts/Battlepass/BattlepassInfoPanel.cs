using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BattlepassInfoPanel : MonoBehaviour
{
    [Header("Populate Variables")]
    public TMP_Text skinName;
    public TMP_Text rarityText;
    public Image skinImage;

    [Header("Premium Lock")]
    public bool locked;
    public GameObject lockedOverlay;

    BattlepassItem bpItem;

    public static BattlepassInfoPanel instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
        lockedOverlay.SetActive(false);
    }

    public void UnlockItem()
    {
        locked = false;
        lockedOverlay.SetActive(locked);
    }

    public void DisplayBattlepassInfoPanel(CollectionItem item, BattlepassItem battlepassItem)
    {
        if (bpItem != null & bpItem != battlepassItem)
            bpItem.Deselect();
        bpItem = battlepassItem;

        locked = battlepassItem.locked;
        lockedOverlay.SetActive(locked);
        
        skinName.text = item.skinName;
        rarityText.text = ObjectNames.NicifyVariableName(item.rarity.ToString());
        rarityText.color = item.GetRarityColor();
        skinImage.color = item.collectionItemImage.color;
        skinImage.sprite = item.collectionItemImage.sprite;
        skinImage.material = item.collectionItemImage.material;
        gameObject.SetActive(true);
    }
}
