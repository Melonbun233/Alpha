using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placement : MonoBehaviour
{
    public static placement toPlace;

    public GameObject towerOption;
    public Vector3 cursor;
    public GameObject towerToFollow;
    public Vector3 wallOffset;
    public Vector3 valleyOffset;

    Ray ray;
    public RaycastHit hit;

    void Awake()
    {
        toPlace = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000) && towerToFollow != null && hit.transform.tag != "UI" && hit.transform.tag != "Ally")
        {
            if(hit.transform.tag == "walls")
            {
                towerToFollow.transform.position = hit.transform.position + wallOffset;
            }
            else
            {
                towerToFollow.transform.position = hit.transform.position + valleyOffset;
            }
            
        }
    }
}
