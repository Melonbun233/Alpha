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

    public static bool isWithinRange(Vector3 origin, Vector3 targetPosition, float range) {
        return horizontalDistance(origin, targetPosition) <= range;
    }

    public static void drawRange(Transform trans, float attackRange, Color color) {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(trans.position, attackRange);
    }

    // Create a line renderer to draw a line between two points
    public static GameObject drawLine(Vector3 start, Vector3 end, Color color, 
        float width = 0.1f, string name = "line", 
        string shaderName = "HDRP/Lit") {

        GameObject line = new GameObject(name);
        line.transform.position = start;
        line.AddComponent<LineRenderer>();

        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find(shaderName));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        return line;
    }

}
