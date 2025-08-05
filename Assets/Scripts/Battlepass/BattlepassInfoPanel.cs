using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using static UnityEditor.Progress;

public class BattlepassInfoPanel : MonoBehaviour
{
    [Header("Populate Variables")]
    public TMP_Text itemName;
    public TMP_Text rarityText;
    public TMP_Text descriptionText;
    public Image itemImage;

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
        if (Player.instance.level > bpItem.levelToClaim)
            locked = false;
        
        lockedOverlay.GetComponent<Image>().enabled = locked;
    }

    public void FullUnlock()
    {
        locked = false;
        lockedOverlay.SetActive(locked);
    }

    public void DisplayBattlepassInfoPanel(BattlepassItem battlepassItem)
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
                claimButton.onClick.AddListener(ItemClaimed);
            }
        }
        
        switch (battlepassItem.type)
        {
            case BattlepassItem.itemType.Skin:
                CollectionItem item = battlepassItem.collectionItem;
                itemName.text = item.itemName;
                rarityText.text = ObjectNames.NicifyVariableName(item.rarity.ToString());
                rarityText.color = item.GetRarityColor();
                descriptionText.gameObject.SetActive(false);
                break;
            case BattlepassItem.itemType.Modifier:
                Modifier mod = battlepassItem.mod;
                itemName.text = mod.modifierName;
                rarityText.text = ObjectNames.NicifyVariableName(mod.modifierRarity.ToString());
                rarityText.color = mod.GetRarityColor();
                descriptionText.gameObject.SetActive(true);
                descriptionText.text = mod.modifierDescription.text;
                break;
            case BattlepassItem.itemType.Currency:
                itemName.text = "Coins";
                rarityText.text = "Common";
                rarityText.color = Color.white;
                descriptionText.gameObject.SetActive(false);
                break;
        }
        itemImage.color = battlepassItem.itemImage.color;
        itemImage.sprite = battlepassItem.itemImage.sprite;
        itemImage.material = battlepassItem.itemImage.material;

        gameObject.SetActive(true);
    }

    void ItemClaimed()
    {
        claimButton.gameObject.SetActive(false);
        claimedOverlay.SetActive(true);
    }
}
