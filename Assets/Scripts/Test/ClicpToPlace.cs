using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicpToPlace : MonoBehaviour
{
    public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            AllyData data = DefaultAllyData.defaultRangerData;
            data.attackData.attackDamage = new DamageData(0, 0, 0, 0, 0);
            data.attackData.attackNumber = 5;

            for (int i = 0; i < 1; i ++) {
                //data.effectData.effects.Add(new BurningEffect() as Effect);
            }
            data.effectData.effects.Add(new BurningAttackEffect() as Effect);
            data.effectData.effects.Add(new BurningAttackEffect() as Effect);
            
            if (Physics.Raycast(ray, out hit)) {
                Ally.spawn(prefab, data, hit.transform.position + new Vector3(0, 2, 0), 
                    hit.transform.rotation);
            }
        }
    }
    
}
