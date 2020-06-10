using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

// This controller is used to control the placement of a tower
public class PlacementController : MonoBehaviour
{

    [Header("Raycast ignore layers")]
    public LayerMask ignoreLayerMask;

    public AllyData allyData;
    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject towerPreview;

    [Header("OffSets")]
    public Vector3 wallOffset;
    public Vector3 valleyOffset;
    public Vector3 rotateAngle;

    [Header("References")]
    public GameObject UIE;
    public GameObject status;
    public TowerTab towerOption;
    public Text manaText;

    public float manaRegen;
    public float maxMana;
    public float startingMana;
    public float mana;

    




    public GameObject instStatus(Ally ally)
    {
        GameObject instant = Instantiate(status, UIE.transform) as GameObject;

        AllyData data = new AllyData(ally.healthData, ally.attackData, ally.resistanceData, ally.moveData,
            ally.effectData, ally.allyType1, ally.allyType1Level, ally.allyType2, ally.allyType2Level, 
            ally.allyLevelData);

        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<Status>().setAllydata(data);
        return instant;
    }

    public GameObject instStatus(AllyData data)
    {
        GameObject instant = Instantiate(status, UIE.transform) as GameObject;
        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<Status>().setAllydata(data);
        return instant;
    }

    // Update is called once per frame
    void Update()
    {
        updateMana();
        updateBuildTower();
    }

    // Update the build tower process
    void updateBuildTower() {
        if(towerPreview == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = ~ ignoreLayerMask;
        
        // preview to place a tower
        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {

            Time.timeScale = 0.3f;
            towerPreview.transform.position = getBuildPosition(hit);
        }

        if (Input.GetMouseButtonDown(1))
        {
            rotateAngle.y += 90f;
            Quaternion temp = new Quaternion();
            temp.eulerAngles = rotateAngle;
            towerPreview.transform.rotation = temp;
        }

        if (Input.GetKeyUp("escape"))
        {
            Destroy(towerPreview);
            Time.timeScale = 1f;
        }

        if (Input.GetMouseButtonUp(0) && hit.transform != null)
        {
            if (towerPreview.GetComponent<PlacementValidation>().Validbuild)
            {
                buildTower(hit, getBuildPosition(hit));
                Time.timeScale = 1f;
            }
            else
            {
                print("Invalid build position!");
            }
        }
        
    }

    Vector3 getBuildPosition(RaycastHit hit) {
        if (hit.transform.tag == "wall") {
            return hit.transform.position + wallOffset;
        } else if (hit.transform.tag == "valley") {
            return hit.transform.position + valleyOffset;
        }

        return hit.transform.position;
    }

    void updateMana() {
        if (mana <= maxMana)
        {
            mana += manaRegen * Time.deltaTime;
            int intMana = (int)mana;
            manaText.text = intMana.ToString();
        } else {
            int intMana = (int)maxMana;
            manaText.text = intMana.ToString();
        }
    }

    public void buildTower(RaycastHit hit, Vector3 position)
    {
        if (towerPrefab == null) {
            return;
        }
        
        // Validate the tag
        // This should be moved to placement validation later
        if (hit.transform.tag == "wall")
        {
            if (allyData.isType(AllyType.Blocker))
            {
                print("cannot place blocker on wall!");
                return;
            } 
        } else if (hit.transform.tag == "valley") {
            if (!allyData.isType(AllyType.Blocker))
            {
                print("cannot place non-blocker class on valleys!");
                return;
            }
        }

        // Spawn ally
        Quaternion rotate = new Quaternion();
        rotate.eulerAngles = rotateAngle;
        GameObject allyGameObject = Ally.spawn(towerPrefab, allyData, position, rotate);

        Destroy(towerPreview);
        mana -= allyData.allyLevelData.cost;
        rotateAngle = new Vector3(0, 0, 0);
        towerPrefab = null;
        towerOption.isCd = true;

        enableAlly(allyGameObject);
    }

    void enableAlly(GameObject allyGameObject) {
        allyGameObject.GetComponent<Ally>().enabled = true;
        allyGameObject.GetComponent<Collider>().enabled = true;

        if (allyGameObject.GetComponent<NavMeshObstacle>()){
            allyGameObject.GetComponent<NavMeshObstacle>().enabled = true;
        }
        
        allyGameObject.tag = "Ally";
    }
}
