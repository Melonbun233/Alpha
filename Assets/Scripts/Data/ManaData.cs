using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManaData {
    public float mana;
    public float maxMana;
    public float manaRegeneration;

    public ManaData(float mana, float maxMana, float manaRegeneration) {
        this.mana = mana;
        this.maxMana = maxMana;
        this.manaRegeneration = manaRegeneration;
    }

    public static ManaData deepCopy(ManaData data) {
        return new ManaData(data.mana, data.maxMana, data.manaRegeneration);
    }
}
