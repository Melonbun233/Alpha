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

public class Character : Unit
{
    [Header("Summon Settings")]
    public float summonRange;

    [Header("Mana Settings")]
    public ManaData manaData;
    
    public override void updateAttackTarget() {

    }

    public override void updateMoveTarget() {

    }

    public static GameObject spawn(GameObject prefab, CharacterData data, 
        Vector3 position, Quaternion rotation) {
        if (prefab.GetComponent<Character>() == null) {
            Debug.Log("Cannot instantiate an non-character object");
            return null;
        }

        GameObject obj = Instantiate(prefab, position, rotation);
        CharacterData.copyData(obj, data);
        return obj;
    }
}
