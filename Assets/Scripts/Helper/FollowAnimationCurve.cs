using System.Collections.Generic;
using UnityEngine;

public class FollowAnimationCurve : MonoBehaviour
{
    public readonly static AnimationCurve DefaultCurveY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(50f, 3f), new Keyframe(100f, 1.5f));
    public static List<RaycastHit> CurveRayCast(Vector3 startPoint, Vector3 endPoint, AnimationCurve curve, int partition)
    {
        float partitionP = 100 / partition;
        List<RaycastHit> hits = new List<RaycastHit>();
        float distance = (endPoint - startPoint).magnitude;
        Vector3 virtualHeight = endPoint;
        Vector3 start = new Vector3(startPoint.x, startPoint.y, startPoint.z);
        Vector3 end;
        
        for(int i = 0; i < partition; i++)
        {
            
            end = Vector3.MoveTowards(start, virtualHeight, distance / partition);
            virtualHeight.y = curve.Evaluate(partitionP * (i + 1));
            end.y = curve.Evaluate(partitionP * (i+1));
            float rayDistance = (end - start).magnitude;
            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(start, (end - start), Color.red, 100f);
            if (Physics.Raycast(start, (end-start), out hit, rayDistance))
            {
                print(hit.transform.name);
                hits.Add(hit);
            }
            start = end;
        }
        return hits;
    }

    public static bool ifHitWall(List<RaycastHit> hits)
    {
        foreach(RaycastHit x in hits)
        {
            if (x.transform.tag == "walls") return true;
        }
        return false;
    }

    public static bool ifHitRanger(List<RaycastHit> hits)
    {
        foreach(RaycastHit x in hits)
        {
            if (x.transform.name.Contains("Ranger")) return true;
        }
        return false;
    }
}
