using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Nullify : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            if (transform.GetSiblingIndex() != 0)
            {
                modifierExpDescription = "failed, not in first position";
                return false;
            }

            TMP_Text baseText = Play.instance.expContent.GetChild(0).GetComponent<TMP_Text>();

            float expGain = (float)Play.instance.expGain;
            expGain = Player.instance.skin.GetSkinExp();
            Play.instance.expGain = (int)expGain;

            baseText.text = $"+{expGain}xp from skin rarity {Home.instance.SplitCamelCase(Player.instance.GetSkinRarity().ToString())} and -0xp from enemies";
            modifierExpDescription = "successfully nullified";

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "failed to nullify";
            return false;
        }
    }
}
