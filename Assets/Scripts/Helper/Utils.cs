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

    public static List<GameObject> findGameObjectsWithinRange(Vector3 position, float range, string tag) {
        Collider[] colliders = Physics.OverlapSphere(position, range);
        List<GameObject> objects = new List<GameObject>();

        foreach(Collider collider in colliders) {
            if (collider.gameObject.tag == tag) {
                objects.Add(collider.gameObject);
            }   
        }

        return objects;
    }

    public static GameObject findNearestGameObject(Vector3 position, List<GameObject> targets) {
        float nearestDistance = float.PositiveInfinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets) {
            float distance = Vector3.Distance(position, target.transform.position);
            if (distance < nearestDistance) {
                nearestTarget = target;
                nearestDistance = distance;
            }
        }

        return nearestTarget;
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

    public static int mult(int origin, float percentMlt, int flatMlt) {
        if (origin + flatMlt <= 0) {
            return origin + flatMlt;
        }
        return (int)((float)(origin + flatMlt) * (1f + percentMlt));
    }
}
