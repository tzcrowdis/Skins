using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camoist : Modifier
{
    Skin.Collection thisCollectionType = Skin.Collection.Camo;

    public override bool ModifierEffect()
    {
        try
        {
            if (Player.instance.skin.collection == thisCollectionType)
            {
                float expGain = (float)Play.instance.expGain;
                expGain *= mult;
                Play.instance.expGain = (int)expGain;
                modifierExpDescription = $"{mult}x for using a skin from the {thisCollectionType.ToString()} Collection";
                return true;
            }

            modifierExpDescription = $"you didn't use a skin from the {thisCollectionType.ToString()} Collection";
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "failed";
            return false;
        }
    }
}
