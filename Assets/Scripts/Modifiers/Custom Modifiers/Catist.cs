using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catist : Modifier
{
    Skin.Collection thisCollectionType = Skin.Collection.Cat;

    public override bool ModifierEffect()
    {
        try
        {
            if (Player.instance.skin.collection == thisCollectionType)
            {
                float expGain = (float)Play.instance.expGain;
                expGain *= mult;
                Play.instance.expGain = (int)expGain;
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }
}
