using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artist : Modifier
{
    Skin.Collection thisCollectionType = Skin.Collection.Art;

    public override bool ModifierEffect()
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
}
