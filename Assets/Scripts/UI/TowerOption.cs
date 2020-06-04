using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TowerOption : MonoBehaviour
{
    [Header("GameObject reference field.")]
    public int type1_lvl;
    public int type2_lvl;
    public GameObject Type1;
    public GameObject Type2;
    public GameObject Type1_lvl;
    public GameObject Type2_lvl;
    public GameObject TowerVisual;
    public GameObject Cost;
    public GameObject[] prefabs;
    public GameObject[] modelsOnly;
    private Button button;
    public GameObject highLight;
    private static GameObject status;

    [Header("Status data")]
    public float cd;
    public bool isCd;
    public AllyData allyData;

    [Header("Sprites for UI icons")]
    public Sprite ranger;
    public Sprite melee;
    public Sprite physical;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;




    public GameObject allyPrefab()
    {
        AllyType type = allyData.allyType1;
        switch (type)
        {
            case AllyType.Ranger:
                return prefabs[0];
            case AllyType.Blocker:
                return prefabs[1];
            case AllyType.Fire:
                return prefabs[0];
            case AllyType.Thunder:
                return prefabs[0];
            case AllyType.Water:
                return prefabs[0];
            case AllyType.Wind:
                return prefabs[0];
        }
        return null;
    }

    public GameObject getModel()
    {
        AllyType type = allyData.allyType1;
        switch (type)
        {
            case AllyType.Ranger:
                return modelsOnly[0];
            case AllyType.Blocker:
                return modelsOnly[1];
            case AllyType.Fire:
                return modelsOnly[0];
            case AllyType.Thunder:
                return modelsOnly[0];
            case AllyType.Water:
                return modelsOnly[0];
            case AllyType.Wind:
                return modelsOnly[0];
        }
        return null;
    }

    private void OnMouseOver()
    {


        if (Input.GetMouseButtonDown(1)&&status==null)
        {
            status = placement.toPlace.instStatus(allyData);
        }
    }

    private void OnMouseExit()
    {
        if (status != null)
        {
            Destroy(status);
        }
    }

    void OnMouseUp()
    {
        if(placement.toPlace.towerToFollow != null)
        {
            Destroy(placement.toPlace.towerToFollow);
        }

        if ((int)placement.toPlace.Mana < allyData.allyLevelData.cost)
        {
            print("Not Enough Mana");
            return;
        }

        if (!isCd)
        {
            GameObject temp = allyPrefab();
            //temp.GetComponent<Ally>().enabled = false;
            //temp.GetComponent<Collider>().enabled = false;
            //if(allyData.isType(AllyType.Blocker))temp.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            //temp.tag = "allyToPlace";
            placement.toPlace.towerPrefab = temp;
            placement.toPlace.allyData = allyData;
            placement.toPlace.towerToFollow = Instantiate(getModel()) as GameObject;
            placement.toPlace.towerOption = this;
        }
        else
        {
            print("This Tower is on CD!");
            return;
        }
    }

    void Start()
    {
        GameObject towerVisual = Instantiate(getModel(), TowerVisual.transform);
        towerVisual.transform.localScale = new Vector3(20, 20, 20);
        isCd = false;
        button = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        Cost.GetComponent<Text>().text = "Cost: " + allyData.allyLevelData.cost.ToString();
            AllyType x = allyData.allyType1;
            switch (x)
            {
                case AllyType.Fire:
                    Type1.GetComponent<Image>().sprite = Fire;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getFireDamage().ToString();
                    break;
                case AllyType.Water:
                    Type1.GetComponent<Image>().sprite = Water;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getWaterDamage().ToString();
                    break;
                case AllyType.Thunder:
                    Type1.GetComponent<Image>().sprite = Thunder;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getThunderDamage().ToString();
                    break;
                case AllyType.Wind:
                    Type1.GetComponent<Image>().sprite = Wind;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getWindDamage().ToString();
                    break;
                case AllyType.Ranger:
                    Type1.GetComponent<Image>().sprite = ranger;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getPhysicalDamage().ToString();
                    break;
                case AllyType.Blocker:
                    Type1.GetComponent<Image>().sprite = physical;
                    Type1_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getPhysicalDamage().ToString();
                    break;
            }

            AllyType z = allyData.allyType2;

            switch (z)
            {
                case AllyType.Fire:
                    Type2.GetComponent<Image>().sprite = Fire;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getFireDamage().ToString();
                    break;
                case AllyType.Water:
                    Type2.GetComponent<Image>().sprite = Water;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getWaterDamage().ToString();
                    break;
                case AllyType.Thunder:
                    Type2.GetComponent<Image>().sprite = Thunder;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getThunderDamage().ToString();
                    break;
                case AllyType.Wind:
                    Type2.GetComponent<Image>().sprite = Wind;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getWindDamage().ToString();
                    break;
                case AllyType.Ranger:
                    Type2.GetComponent<Image>().sprite = ranger;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getPhysicalDamage().ToString();
                    break;
                case AllyType.Blocker:
                    Type2.GetComponent<Image>().sprite = physical;
                    Type2_lvl.GetComponent<Text>().text = allyData.attackData.attackDamage.getPhysicalDamage().ToString();
                    break;
                case AllyType.None:
                    Type2.SetActive(false);
                    Type2_lvl.SetActive(false);
                    break;
            }

        if (LevelController.levelCtr.Base.isDead())
        {
            gameObject.GetComponent<TowerOption>().enabled = false;
        }

        if (isCd) 
        {
            button.enabled = false;
            highLight.SetActive(false);
        } 
        else
        {
            button.enabled = true;
            highLight.SetActive(true);
        }

        if ((int)placement.toPlace.Mana < allyData.allyLevelData.cost)
        {
            button.enabled = false;
            highLight.SetActive(false);
        }
        else
        {
            button.enabled = true;
            highLight.SetActive(true);
        }
    }
}
