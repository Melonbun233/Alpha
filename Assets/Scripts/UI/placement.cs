using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class placement : MonoBehaviour
{
    public static placement toPlace;

    [Header("Raycast layers.")]
    public LayerMask layer1;
    public LayerMask layer2;

    public AllyData allyData;
    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject towerToFollow;

    [Header("OffSets")]
    public Vector3 wallOffset;
    public Vector3 valleyOffset;
    public Vector3 rotate;

    [Header("References")]
    public Camera Maincamera;
    public GameObject UIE;
    public GameObject status;
    public TowerOption towerOption;

    [Header("Mana Data")]
    public float Mana;
    public float MaxMana;
    public float ManaRegen;
    public float startingMana;


    Ray ray;
    public RaycastHit hit;

    public void CameraAngleSwitch()
    {
        Maincamera.GetComponent<CameraController>().angleSwtich();
    }


    void Awake()
    {
        toPlace = this;
    }

    public GameObject instStatus(Ally ally)
    {
        GameObject instant = Instantiate(status, UIE.transform) as GameObject;

        AllyData data = new AllyData(ally.healthData, ally.attackData, ally.resistanceData, ally.moveData,
            ally.effectData, ally.allyType1, ally.allyType1Level, ally.allyType2, ally.allyType2Level, ally.allyLevelData);

        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<status>().setAllydata(data);
        return instant;
    }

    public GameObject instStatus(AllyData data)
    {
        GameObject instant = Instantiate(status, UIE.transform) as GameObject;
        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<status>().setAllydata(data);
        return instant;
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

        if (Input.GetKeyUp("escape") && towerToFollow != null)
        {
            Destroy(towerToFollow);
        }

        if (Input.GetMouseButtonUp(0) && hit.transform != null && hit.transform.tag != "UI"  && towerToFollow != null)
        {
            if (towerToFollow.GetComponent<placementValidation>().Validbuild)
            {
                hit.transform.GetComponent<Build>().towerOption = this.towerOption;
                hit.transform.GetComponent<Build>().buildT();
            }
            else
            {
                print("Invalid build position!");
            }

            
        }

    }
}
