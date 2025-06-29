using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BattlepassInfoPanel : MonoBehaviour
{
    [Header("Populate Variables")]
    public TMP_Text itemName;
    public TMP_Text rarityText;
    public Image skinImage;

    [Header("Premium Lock")]
    public bool locked;
    public GameObject lockedOverlay;

    [Header("Claimed")]
    public bool claimed;
    public Button claimButton;
    public GameObject claimedOverlay;

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
        claimButton.gameObject.SetActive(false);
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

        claimed = battlepassItem.claimed;
        claimedOverlay.SetActive(claimed);
        if (!claimed & !locked)
        {
            if (Player.instance.level >= battlepassItem.levelToClaim)
            {
                claimButton.gameObject.SetActive(true);
                claimButton.onClick.RemoveAllListeners();
                claimButton.onClick.AddListener(battlepassItem.ClaimItem);
            }
        }
        
        itemName.text = item.itemName;
        rarityText.text = ObjectNames.NicifyVariableName(item.rarity.ToString());
        rarityText.color = item.GetRarityColor();
        skinImage.color = item.itemImage.color;
        skinImage.sprite = item.itemImage.sprite;
        skinImage.material = item.itemImage.material;
        gameObject.SetActive(true);
    }
}
