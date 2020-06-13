using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultCharacterData
{
    public static CharacterData testCharacterData {
        get {
            return new CharacterData(
                new HealthData(100, 100, 1f, 0),
                new AttackData(0, 1, new DamageData(0, 0, 0, 0, 0), 0, 0),
                new ResistanceData(10, 10, 10, 10, 10),
                new MoveData(0),
                new EffectData(),
                new ManaData(0, 20, 1f),
                10f);
        }
    }
}
