using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannibal : Modifier
{
    [Header("Cannibal")]
    public TMP_Text multText;
    float mult = 1;
    float multDelta = 0.1f;

    
    public override bool ModifierEffect()
    {
        try 
        {
            switch (Player.instance.skin.rarity)
            {
                case Skin.Rarity.VeryCommon:
                    mult += multDelta;
                    break;
                case Skin.Rarity.Common:
                    mult += multDelta * 2;
                    break;
                case Skin.Rarity.Rare:
                    mult += multDelta * 3;
                    break;
                case Skin.Rarity.Legendary:
                    mult += multDelta * 4;
                    break;
            }

            float expGain = Play.instance.expGain;
            Play.instance.expGain = (int)(expGain * mult);

            multText.text = $"{mult}x";
            modifierExpDescription = $"ate {Player.instance.skin.itemName}. multiplied xp by {mult}x.";

            Collection.instance.DeleteCollectionItem(Player.instance.skin);
            Player.instance.skin = null; // testing

            return true;
        }
        catch
        {
            return false;
        }
    }

    /*
     * DRAG
     */
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        multText.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        multText.gameObject.SetActive(true);
    }
}
