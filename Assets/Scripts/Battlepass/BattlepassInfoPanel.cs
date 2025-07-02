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

    [Header("Lock")]
    public bool locked;
    public TMP_Text lockedLevel;
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

    public void BattlepassUnlock()
    {
        lockedOverlay.GetComponent<Image>().enabled = locked;
    }

    public void FullUnlock()
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
        if (locked)
        {
            if (battlepassItem.lockedLevel.text == "")
                lockedLevel.text = "";
            else
                lockedLevel.text = $"{battlepassItem.levelToClaim}";

            if (Battlepass.instance.premiumOwner)
                lockedOverlay.GetComponent<Image>().enabled = false;
            else
                lockedOverlay.GetComponent<Image>().enabled = true;

            lockedOverlay.SetActive(true);
        }
        else
        {
            if (Player.instance.level < battlepassItem.levelToClaim)
            {
                lockedOverlay.SetActive(true);
                lockedLevel.text = $"{battlepassItem.levelToClaim}";
                lockedOverlay.GetComponent<Image>().enabled = false;
            }
            else
            {
                lockedOverlay.SetActive(false);
            }
        }

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
