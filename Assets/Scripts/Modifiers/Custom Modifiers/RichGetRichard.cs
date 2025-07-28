using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RichGetRichard : Modifier
{
    [Header("Richard")]
    public TMP_Text adderText;
    int adder = 0;
    int adderDelta = 10;
    
    public override bool ModifierEffect()
    {
        try
        {
            string adderGrowth = "";
            switch (Player.instance.skin.rarity)
            {
                case Skin.Rarity.VeryCommon:
                    adder += adderDelta;
                    adderGrowth = "10xp";
                    break;
                case Skin.Rarity.Common:
                    adder += adderDelta * 2;
                    adderGrowth = "20xp";
                    break;
                case Skin.Rarity.Rare:
                    adder += adderDelta * 3;
                    adderGrowth = "30xp";
                    break;
                case Skin.Rarity.Legendary:
                    adder += adderDelta * 4;
                    adderGrowth = "40xp";
                    break;
            }

            Play.instance.expGain += adder;

            adderText.text = $"+{adder}xp";
            modifierExpDescription = $"grew by {adderGrowth}. added +{adder}xp";

            return true;
        }
        catch
        {
            return false;
        }
    }
}
