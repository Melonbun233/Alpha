using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float horizontalDistance(Vector3 p1, Vector3 p2) {
        p1.y = p2.y;
        return Vector3.Distance(p1, p2);
    }

    public static float horizontalDistance(Transform t1, Transform t2) {
        return horizontalDistance(t1.position, t2.position);
    }

}
