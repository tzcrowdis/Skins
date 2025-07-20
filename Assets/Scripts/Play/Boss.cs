using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss Info")]
    public string bossName;
    
    protected override void Start()
    {
        base.Start();
    }
}
