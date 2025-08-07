using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeStones : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            // exp gain 3x if three very commons in match
            int veryCommonCount = 0;
            foreach (Transform enemy in EnemyController.instance.transform)
            {
                if (enemy.gameObject.activeSelf && enemy.GetComponent<Enemy>().skin.rarity == Skin.Rarity.VeryCommon)
                    veryCommonCount++;
            }

            if (Player.instance.skin.rarity == Skin.Rarity.VeryCommon)
                veryCommonCount++;

            if (veryCommonCount >= 3)
            {
                float expGain = (float)Play.instance.expGain;
                expGain *= 3f;
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
