using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitosis : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            Collection.instance.AddToCollection(Player.instance.skin);

            float expGain = (float)Play.instance.expGain;
            expGain -= add;
            Play.instance.expGain = (int)expGain;

            modifierExpDescription = $"-200xp. duplicated {Player.instance.skin.itemName}.";

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "failed to replicate";
            return false;
        }
    }
}
