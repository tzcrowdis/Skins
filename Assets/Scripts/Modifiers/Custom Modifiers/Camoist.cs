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
                expGain *= 1.5f;
                Play.instance.expGain = (int)expGain;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}
