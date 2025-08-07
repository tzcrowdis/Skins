using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitosis : Modifier
{
    int expDelta = 200;
    
    public override bool ModifierEffect()
    {
        try
        {
            Collection.instance.AddToCollection(Player.instance.skin);

            float expGain = (float)Play.instance.expGain;
            //expGain = Mathf.Max(0f, expGain - expDelta);
            expGain -= expDelta;
            Play.instance.expGain = (int)expGain;

            modifierExpDescription = $"-200xp. duplicated {Player.instance.skin.itemName}.";

            return true;
        }
        catch
        {
            return false;
        }
    }
}
