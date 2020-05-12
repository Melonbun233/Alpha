using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    Physic,
    Magic // probably will change later
}
// Damage data represents the type, amount of damage dealt to another destroyable
public class DamageData
{
    public int amount;
    public DamageType type;

    public GameObject dealer;

    DamageData(int _amount, DamageType _type, GameObject _dealer) {
        amount = _amount;
        type = _type;
        dealer = _dealer;
    }
}
