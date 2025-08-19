using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
            if (EnemyController.instance.bossFight)
            {
                Enemy enemy = null;
                switch (EnemyController.instance.boss)
                {
                    case EnemyController.BossType.Randomizer:
                        enemy = EnemyController.instance.randomizerBoss.GetComponent<Enemy>();
                        break;
                    case EnemyController.BossType.EvilRandomizer:
                        enemy = EnemyController.instance.evilRandomizer.GetComponent<Enemy>();
                        break;
                    case EnemyController.BossType.MonkeyPaw:
                        enemy = EnemyController.instance.monkeyPaw.GetComponent<Enemy>();
                        break;
                }

                if (enemy != null && enemy.skin.collection == thisCollectionType)
                {
                    add += enemy.skin.GetSkinExp() / divideExpBy;
                    count++;
                }
            }
            else
            {
                foreach (Transform enemyTransform in EnemyController.instance.transform)
                {
                    Enemy enemy = enemyTransform.GetComponent<Enemy>();

                    if (enemy.gameObject.CompareTag("Boss"))
                        continue;

                    if (enemy.skin.collection == thisCollectionType)
                    {
                        add += enemy.skin.GetSkinExp() / divideExpBy;
                        count++;
                    }
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
