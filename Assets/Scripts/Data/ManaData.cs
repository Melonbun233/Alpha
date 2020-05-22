using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManaData {
    public int mana;
    public int maxMana;
    public float manaRegeneration;

    public ManaData(int mana, int maxMana, float manaRegeneration) {
        this.mana = mana;
        this.maxMana = maxMana;
        this.manaRegeneration = manaRegeneration;
    }
}
