using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedMulter : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            string adjective = "";
            switch (Player.instance.skin.rarity)
            {
                case CollectionItem.Rarity.VeryCommon:
                    mult = 4f;
                    adjective = "succulent";
                    break;
                case CollectionItem.Rarity.Common:
                    mult = 3f;
                    adjective = "fantastic";
                    break;
                case CollectionItem.Rarity.Rare:
                    mult = 2f;
                    adjective = "great";
                    break;
                case CollectionItem.Rarity.Legendary:
                    mult = 1;
                    adjective = "meaningless";
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
            modifierExpDescription = "inverted multer failed to inverted mult";
            return false;
        }
    }
}
