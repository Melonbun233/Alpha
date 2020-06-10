using System.Collections;
using System.Collections.Generic;
using System;
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

    public static bool isWithinRangeObstacle(Vector3 origin, Vector3 targetPosition, float range)
    {
        RaycastHit hit;
        Ray ray = new Ray(origin, (targetPosition - origin));
        if (Physics.Raycast(ray, out hit, range, 31))
        {
            if (hit.transform.tag == "wall")
            {
                return false;
            }
            if(hit.transform.tag == "Ally" || hit.transform.tag == "Base")
            {
                return true;
            }
            return false;
        }
        else 
        {
            return false;
        }
    }

    public static void drawRange(Transform trans, float attackRange, Color color) {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(trans.position, attackRange);
    }

    public static void findGameObjectsWithinRange(List<GameObject> ptr, Vector3 position, 
        float range, string tag) {
        Collider[] colliders = Physics.OverlapSphere(position, range);

        foreach(Collider collider in colliders) {
            if (collider.gameObject == null) {
                continue;
            }
            if (collider.gameObject.tag == tag) {
                ptr.Add(collider.gameObject);
            }   
        }

    }

    public static void sortByDistance(List<GameObject> ptr, Vector3 position) {
        List<Tuple<GameObject, float>> tmp = new List<Tuple<GameObject, float>>();
        foreach(GameObject obj in ptr) {
            tmp.Add(new Tuple<GameObject, float>(obj, 
                Vector3.Distance(obj.transform.position, position)));
        }

        tmp.Sort(delegate(Tuple<GameObject, float> x, Tuple<GameObject, float> y) {
            return x.Item2.CompareTo(y.Item2);
        });

        ptr.Clear();

        for (int i = 0; i < tmp.Count; i ++) {
            ptr.Add(tmp[i].Item1);
        }
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
