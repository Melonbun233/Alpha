using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MoveData {
    public float moveSpeed;
    public MoveData (float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public static MoveData deepCopy(MoveData data) {
        return new MoveData(data.moveSpeed);
    }
}
