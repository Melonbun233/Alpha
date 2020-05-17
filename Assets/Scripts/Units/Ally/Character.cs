using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [Header("Summon Settings")]
    public float summonRange;

    [Header("Mana Settings")]
    public int mana;
    public int maxMana;
    // Mana regeneration method is to be determined
    // Should it be regain 1 mana every ? seconds 
    // or regain x mana every ? seconds
    public float manaRegeneration;
    
    public override void updateAttackTarget() {

    }

    public override void updateMoveTarget() {

    }
}
