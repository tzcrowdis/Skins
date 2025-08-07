using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;

public class SunnyDisposition : Modifier
{
    public override bool ModifierEffect()
    {
        try
        {
            // overwrite base exp and give it +1 rarity
            TMP_Text baseText = Play.instance.expContent.GetChild(0).GetComponent<TMP_Text>();

            Play.instance.expGain += 100;
            int expGain = Play.instance.expGain;

            string rarity = "";
            switch (baseText.text)
            {
                case string a when a.Contains("Very Common"):
                    rarity = "Common";
                    break;
                case string b when b.Contains("Common"):
                    rarity = "Rare";
                    break;
                case string c when c.Contains("Rare"):
                    rarity = "Legendary";
                    break;
                case string d when d.Contains("Legendary"):
                    rarity = "Extra Legendary";
                    break;
                case string e when e.Contains("Extra Legendary"):
                    rarity = "Extra Extra Legendary";
                    break;
                case string f when f.Contains("Extra Extra Legendary"):
                    rarity = "Extra Extra Extra Legendary";
                    break;
                case string g when g.Contains("Extra Extra Extra Legendary"):
                    rarity = "Extra Extra Extra Extra Legendary";
                    break;
                case string h when h.Contains("Extra Extra Extra Extra Legendary"):
                    rarity = "Extra Extra Extra Extra Extra Legendary";
                    break;
            }

            baseText.text = $"+{expGain} xp from skin rarity {rarity}";

            // TODO restart modifier phase to account for mults?

            return true;
        }
        catch
        {
            return false;
        }
    }
}
