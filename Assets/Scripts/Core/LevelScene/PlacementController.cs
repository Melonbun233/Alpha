using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;

// This controller is used to control the placement of a tower
public class PlacementController : MonoBehaviour
{

    [Header("Raycast ignore layers")]
    public LayerMask ignoreLayerMask;

    [Header("OffSets")]
    public Vector3 wallOffset;
    public Vector3 valleyOffset;
    public Vector3 rotateAngle;

    [Header("References")]
    public GameObject UI;
    public GameObject status;
    public Text manaText;

    public Character player;


    public GameObject instStatus(Ally ally)
    {
        GameObject instant = Instantiate(status, UI.transform) as GameObject;

        AllyData data = new AllyData(ally.healthData, ally.attackData, ally.resistanceData, ally.moveData,
            ally.effectData, ally.allyType1, ally.allyType1Level, ally.allyType2, ally.allyType2Level, 
            ally.allyLevelData, 10);

        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<Status>().setAllydata(data);
        return instant;
    }

    public GameObject instStatus(AllyData data)
    {
        GameObject instant = Instantiate(status, UI.transform) as GameObject;
        Vector3 temp2 = Input.mousePosition;
        temp2.z = 1f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(temp2);
        instant.GetComponent<RectTransform>().position = temp;
        instant.GetComponent<Status>().setAllydata(data);
        return instant;
    }

    // Build the tower from selected tower tab
    public void buildTower(RaycastHit hit, Vector3 position)
    {
        if (TowerTab.selectedTowerTab == null) {
            return;
        }
        
        AllyData allyData = TowerTab.selectedTowerTab.allyData;
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
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = rotateAngle;
        GameObject allyGameObject = TowerTab.selectedTowerTab.buildTower(position, rotation, null);

        player.manaData.mana -= allyData.allyLevelData.cost;
        rotateAngle = new Vector3(0, 0, 0);

        enableAlly(allyGameObject);
    }

    public bool hasEnoughMana(float manaCost) {
        return manaCost <= player.manaData.mana;
    }

    // Update is called once per frame
    void Update()
    {
        updateMana();
        updateTowerPreview();
    }

    // Update the tower preview from the selected tower tab
    void updateTowerPreview() {
        if(TowerTab.selectedTowerTab == null)
        {
            return;
        }

        GameObject towerPreview = TowerTab.selectedTowerTab.towerPreview;
        RaycastHit hit;
        int layerMask = ~ ignoreLayerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
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
            TowerTab.selectedTowerTab.deselectTowerTab();
            Time.timeScale = 1f;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() 
            && hit.transform != null)
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
        if (player == null) {
            return;
        }

        ManaData manaData = player.manaData;
        if (manaData.mana <= manaData.maxMana)
        {
            manaData.mana += manaData.manaRegeneration * Time.deltaTime;
            int intMana = (int)manaData.mana;
            manaText.text = intMana.ToString();
        } else {
            int intMana = (int)manaData.maxMana;
            manaText.text = intMana.ToString();
        }
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
