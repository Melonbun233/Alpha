using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    public float speed;//the moving speed parameter of enemy
    public int hp;//enemy health

    public Transform Base;//Link to base transformation

    void Start()
    {
        //initialize enemy status
        speed = 1f;
        hp = 3;

        //AI寻路
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = Base.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
