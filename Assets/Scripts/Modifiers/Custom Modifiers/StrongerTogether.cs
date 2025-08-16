using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongerTogether : Modifier
{
    [Header("Image Options")]
    public Sprite smile;
    public Sprite frown;
    
    public override bool ModifierEffect()
    {
        try
        {
            if (Player.instance.modifiers.Count > 1)
            {
                modifierExpDescription = "we are stronger together! :)";
                modifierImage.sprite = smile;
            }
            else
            {
                modifierExpDescription = "i'm all alone... :'(";
                modifierImage.sprite = frown;
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);

            modifierExpDescription = "i've failed";
            modifierImage.sprite = frown;

            return false;
        }
    }

    public override void AlterOtherModifier(Modifier mod)
    {
        // update new modifier when it's added
        if (mod.modifierType == Modifier.Type.ExpMult)
        {
            mod.mult += mult;
        }
        else if (mod.modifierType == Modifier.Type.ExpAdd)
        {
            mod.add += add;
        }
        else
        {
            // do nothing
        }
    }

    protected override void Start() 
    {
        base.Start();

        // buffs all other modifiers
        foreach (Modifier mod in Player.instance.modifiers)
        {
            if (mod == this)
                continue;

            if (mod.modifierType == Modifier.Type.ExpMult)
            {
                mod.mult += mult;
            }
            else if (mod.modifierType == Modifier.Type.ExpAdd)
            {
                mod.add += add;
            }
            else
            {
                // do nothing
            }
        }
    }

    private void OnDestroy()
    {
        // remove buff from all other modifiers
        foreach (Modifier mod in Player.instance.modifiers)
        {
            if (mod == this)
                continue;
            
            if (mod.modifierType == Modifier.Type.ExpMult)
            {
                mod.mult -= mult;
            }
            else if (mod.modifierType == Modifier.Type.ExpAdd)
            {
                mod.add -= add;
            }
            else
            {
                // do nothing
            }
        }
    }
}
