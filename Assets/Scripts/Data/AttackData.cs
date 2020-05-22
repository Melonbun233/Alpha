using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData {
    public float attackRange;
    public float attackCoolDown;
    public DamageData attackDamage;
    public int attackNumber;
    public float attackAoeRange;    

    public AttackData(float attackRange, float attackCoolDown, DamageData attackDamage,
        int attackNumber, float attackAoeRange) {
            this.attackRange = attackRange;
            this.attackCoolDown = attackCoolDown;
            this.attackDamage = attackDamage;
            this.attackNumber = attackNumber;
            this.attackAoeRange = attackAoeRange;
        }
}
