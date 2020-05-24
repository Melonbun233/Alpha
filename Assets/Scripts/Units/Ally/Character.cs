using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
