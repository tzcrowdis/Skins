using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photocopy : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            int siblingIndex = transform.GetSiblingIndex();

            if (siblingIndex == 0 | Player.instance.modifiers.Count == 1)
            {
                modifierExpDescription = "there's nothing left";
                return false;
            }

            Modifier leftMod = EnemyController.instance.transform.GetChild(siblingIndex - 1).GetComponent<Modifier>();
            leftMod.ModifierEffect();
            modifierExpDescription = leftMod.modifierExpDescription;
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "failed to copy";
            return false;
        }
    }
}
