using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    GameObject Tower;
    GameObject toPlaceTower;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnMouseUp()
    {
        if (Tower != null) { 
            if(placement.toPlace.hit.transform.tag == "walls")
            {
                toPlaceTower = Instantiate(Tower, transform.position + placement.toPlace.wallOffset , transform.rotation) as GameObject;
            }
            else
            {
                toPlaceTower = Instantiate(Tower, transform.position + placement.toPlace.valleyOffset, transform.rotation) as GameObject;
            }
        }
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print(placement.toPlace.hit.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (placement.toPlace.tower != null)
        {
            Tower = placement.toPlace.tower;
        }
    }
}
