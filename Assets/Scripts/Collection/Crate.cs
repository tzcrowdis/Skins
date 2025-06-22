using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Crate : CollectionItem, IPointerClickHandler
{
    [Header("Crate Type")]
    public CrateType type;

    [Header("Loot Pool")]
    public List<GameObject> items;
    List<GameObject> lootPool;
    
    [Header("Key")]
    public int keyCoinCost;

    public enum CrateType
    {
        Skins,
        Modifiers
    }

    protected override void Start()
    {
        base.Start();
        
        // TODO not memory efficient, figure out percentages
        lootPool = new List<GameObject>();
        foreach (GameObject item in items)
        {
            if (type == CrateType.Modifiers)
            {
                Modifier mod = item.GetComponent<Modifier>();
                if (mod != null)
                {
                    for (int i = 0; i < mod.GetLootPoolMult(); i++)
                    {
                        lootPool.Add(item);
                    }
                }
            }
            
            if (type == CrateType.Skins)
            {
                Skin skin = item.GetComponent<Skin>();
                if (skin != null)
                {
                    for (int i = 0; i < skin.GetLootPoolMult(); i++)
                    {
                        lootPool.Add(item);
                    }
                }
            }
        }
    }

    public GameObject GetReward()
    {
        int randomNumber = Random.Range(0, lootPool.Count);
        return lootPool[randomNumber];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Collection.instance.OpenCrateKeyPanel(this);
    }
}
