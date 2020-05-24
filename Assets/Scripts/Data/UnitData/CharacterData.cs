using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData: UnitData{
    public float summonRange;
    public ManaData manaData;
    
    public CharacterData(HealthData healthData, AttackData attackData, 
        ResistanceData resistanceData, MoveData moveData, EffectData effectData, 
        ManaData manaData, float summonRange) :
        base(healthData, attackData, resistanceData, moveData, effectData) {
            this.summonRange = summonRange;
            this.manaData = manaData;
        }

    public static GameObject copyData(GameObject obj, CharacterData data) {
        if (obj.GetComponent<Character>() == null) {
            return null;
        }
        
        UnitData.copyData(obj, (UnitData)data);
        Character c = obj.GetComponent<Character>();

        c.summonRange = data.summonRange;
        c.manaData = data.manaData;
        return obj;
    }
}
