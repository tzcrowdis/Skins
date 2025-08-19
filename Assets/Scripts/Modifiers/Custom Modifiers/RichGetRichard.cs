using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RichGetRichard : Modifier
{
    [Header("Richard")]
    public TMP_Text adderText;
    public int addDelta = 10;
    
    public override bool ModifierEffect()
    {
        try
        {
            string adderGrowth = "";
            switch (Player.instance.skin.rarity)
            {
                case Skin.Rarity.VeryCommon:
                    add += addDelta;
                    adderGrowth = "10xp";
                    break;
                case Skin.Rarity.Common:
                    add += addDelta * 2;
                    adderGrowth = "20xp";
                    break;
                case Skin.Rarity.Rare:
                    add += addDelta * 3;
                    adderGrowth = "30xp";
                    break;
                case Skin.Rarity.Legendary:
                    add += addDelta * 4;
                    adderGrowth = "40xp";
                    break;
            }

            Play.instance.expGain += add;

            adderText.text = $"+{add}xp";
            modifierExpDescription = $"grew by {adderGrowth}. added +{add}xp";

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "failed to get richer";
            return false;
        }
    }

    /*
     * DRAG
     */
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        adderText.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        adderText.gameObject.SetActive(true);
    }
}
