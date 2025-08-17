using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multer : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            string adjective = "";
            switch (Player.instance.skin.rarity)
            {
                case CollectionItem.Rarity.VeryCommon:
                    mult = 1.1f;
                    adjective = "fine";
                    break;
                case CollectionItem.Rarity.Common:
                    mult = 1.2f;
                    adjective = "good";
                    break;
                case CollectionItem.Rarity.Rare:
                    mult = 1.3f;
                    adjective = "great";
                    break;
                case CollectionItem.Rarity.Legendary:
                    mult = 1.4f;
                    adjective = "fuckable";
                    break;
            }

            float expGain = (float)Play.instance.expGain;
            expGain *= mult;
            Play.instance.expGain = (int)expGain;

            modifierExpDescription = $"applied a {mult}x mult for a {adjective} {Home.instance.SplitCamelCase(Player.instance.skin.rarity.ToString())} skin";
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "multer failed to mult";
            return false;
        }
    }
}
