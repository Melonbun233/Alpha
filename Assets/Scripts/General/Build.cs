using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    bool  hasbuilt;
    GameObject Tower;

    // Start is called before the first frame update
    void Start()
    {
        hasbuilt = false;
    }

    void OnMouseUp()
    {
        if (Tower != null && !hasbuilt) { 
            if(placement.toPlace.hit.transform.tag == "walls")
            {
                GameObject instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.wallOffset, Quaternion.identity);
                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.wallOffset , Quaternion.identity) as GameObject;
                instant.GetComponent<Ally>().enabled = true;
                instant.GetComponent<Collider>().enabled = true;
                if (placement.toPlace.allyData.allyType == AllyType.Blocker) instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.towerToFollow = null;
                hasbuilt = true;
            }

            if(placement.toPlace.hit.transform.tag == "valley")
            {
                GameObject instant = Ally.spawn(Tower, placement.toPlace.allyData, transform.position + placement.toPlace.valleyOffset, Quaternion.identity);
                //GameObject instant = Instantiate(Tower, transform.position + placement.toPlace.valleyOffset, Quaternion.identity) as GameObject;
                instant.GetComponent<Ally>().enabled = true;
                instant.GetComponent<Collider>().enabled = true;
                if (placement.toPlace.allyData.allyType == AllyType.Blocker) instant.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                instant.tag = "Ally";
                Destroy(placement.toPlace.towerToFollow);
                placement.toPlace.towerToFollow = null;
                hasbuilt = true;
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
        if (placement.toPlace.towerModel != null)
        {
            Tower = placement.toPlace.towerModel;
        }
    }
}
