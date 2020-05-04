using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_status : MonoBehaviour
{
    public int base_hp;
    public Collider enemey;

    //扣血
    private void OnTriggerEnter(Collider enemey) {
        base_hp--;
    }

    private void GameOver() {
        if (base_hp <= 0) {

        }
        else
        {

        }
    } 

    // Start is called before the first frame update
    void Start()
    {
        base_hp = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
