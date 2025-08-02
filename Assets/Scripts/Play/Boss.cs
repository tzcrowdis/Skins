using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss Info")]
    public string bossName;
    public int negExpMult;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetExpAndText()
    {
        base.SetExpAndText();

        negExp *= negExpMult;
        expText.text = $"{negExp}xp";
    }

    public virtual void ReactToChosenSkin() { }
}
