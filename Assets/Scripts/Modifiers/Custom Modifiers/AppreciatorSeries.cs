using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppreciatorSeries : Modifier
{
    [Header("Appreciate")]
    public Skin.Collection thisCollectionType;
    public int divideExpBy = 4;

    public override bool ModifierEffect()
    {
        try
        {
            int count = 0;
            add = 0;
            foreach (Enemy enemy in EnemyController.instance.transform.GetComponentsInChildren<Enemy>())
            {
                if (!enemy.gameObject.activeSelf)
                    continue;

                if (enemy.skin.collection == thisCollectionType)
                {
                    add += enemy.skin.GetSkinExp() / divideExpBy;
                    count++;
                }   
            }

            if (count > 0)
            {
                float expGain = (float)Play.instance.expGain;
                expGain += add;
                Play.instance.expGain = (int)expGain;

                modifierExpDescription = $"appreciated {count} skins for +{add}xp";
                return true;
            }

            modifierExpDescription = $"nothing to appreciate here";
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            modifierExpDescription = "couldn't appreciate anything";
            return false;
        }
    }
}
