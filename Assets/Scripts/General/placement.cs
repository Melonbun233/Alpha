using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class placement : MonoBehaviour
{


    public LayerMask layer1;
    public LayerMask layer2;
    public static placement toPlace;
    public AllyData allyData;
    public GameObject towerPrefab;
    public Vector3 cursor;
    public GameObject towerToFollow;
    public Vector3 wallOffset;
    public Vector3 valleyOffset;
    public Vector3 rotate;


    Ray ray;
    public RaycastHit hit;

    void Awake()
    {
        toPlace = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
        rotate = new Vector3();
    }


    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, (layer1 | layer2)) && towerToFollow != null)
            //&& hit.transform.tag != "UI" && hit.transform.tag != "Ally" && hit.transform.tag != "Untagged")
        {
            if(hit.transform.tag == "walls")
            {
                towerToFollow.transform.position = hit.transform.position + wallOffset;
            }
            if(hit.transform.tag == "valley")
            {
                towerToFollow.transform.position = hit.transform.position + valleyOffset;
            }
        }

        if (Input.GetMouseButtonDown(1) && towerToFollow != null)
        {
            rotate.y += 90f;
            Quaternion temp = new Quaternion();
            temp.eulerAngles = rotate;
            towerToFollow.transform.rotation = temp;
        }


        if (Input.GetMouseButtonUp(0) && hit.transform != null && hit.transform.tag != "UI"  && towerToFollow != null)
        {
            if (towerToFollow.GetComponent<placementValidation>().Validbuild)
            {
                hit.transform.GetComponent<Build>().buildT();
            }
            else
            {
                print("Invalid build position!");
            }

            
        }



    }
}
