using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class E_Projectile : Projectile
{
    public AnimationCurve projectileCurve;
    public Vector3 attackerPosition;
    public GameObject spawnEnemyProjectile(Vector3 position, Quaternion rotation, GameObject attacker, GameObject target, float speed, AnimationCurve curve)
    {
        GameObject obj = Instantiate(gameObject, position, rotation) as GameObject;
        obj.transform.LookAt(target.transform.position);
        E_Projectile projectile = obj.GetComponent<E_Projectile>();
        projectile.target = target;
        projectile.attacker = attacker;
        projectile.speed = speed;
        attackerPosition = attacker.transform.position;
        movePosition = target.transform.position;
        AnimationCurve temp = new AnimationCurve();
        temp.AddKey(0, transform.position.y);
        temp.AddKey(50, curve.keys[1].value);
        temp.AddKey(100, target.transform.position.y);
        projectile.projectileCurve = temp;
        return obj;
    }

    /*
    protected override void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);

        if(other.transform.tag != "Enemy")
        {
            try
            {
                other.transform.GetComponent<Destroyable>().receiveDamage(attacker.GetComponent<Enemy>().attackData.attackDamage, attacker);
            }catch(Exception e)
            {
                print(e);
            }

            Destroy(gameObject);
        }
    }*/

    protected override void FixedUpdate()
    {
        if (gameObject == null)
        {
            return;
        }

        if(attacker == null)
        {

        }

        if (target == null &&
            Vector3.Distance(movePosition, transform.position) <= 0.2f)
        {
            //Mathf.Abs(Quaternion.Dot(moveRotation, transform.rotation)) >= 0.8f) {
            Destroy(gameObject);
            return;
        }


        float step = speed * Time.deltaTime;
        float distance = (movePosition - attackerPosition).magnitude;
        float currentDistance = (movePosition - transform.position).magnitude;
        Vector3 tempHeight = movePosition;
        tempHeight.y = projectileCurve.Evaluate(100 - ((currentDistance / distance) * 100));
        Vector3 height = Vector3.MoveTowards(transform.position, tempHeight, step);
        height.y = projectileCurve.Evaluate(100 - ((currentDistance/distance)*100));
        transform.position = height;
        
        if(currentDistance/distance >= 0.5f)transform.LookAt(movePosition);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movePosition) , step);
    }
}
