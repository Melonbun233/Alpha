using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TowerOption : MonoBehaviour
{
    public int type1_lvl;
    public int type2_lvl;
    public List<String> types;
    public GameObject placable;
    public GameObject Type1;
    public GameObject Type2;
    public GameObject Type1_lvl;
    public GameObject Type2_lvl;
    public GameObject TowerModel;
    public GameObject Cost;
    public GameObject[] prefabs;

    public AllyData allyData;

    public Sprite ranger;
    public Sprite melee;
    public Sprite physical;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;



    public TowerOption(AllyData allyData)
    {
        this.allyData = allyData;
    }

    public GameObject allyPrefab()
    {
        AllyType type = allyData.allyType;
        switch (type)
        {
            case AllyType.Ranger:
                return prefabs[0];
            case AllyType.Blocker:
                return prefabs[1];
        }
        return null;
    }

    void OnMouseUp()
    {
        GameObject temp = allyPrefab();
        temp.GetComponent<Ally>().enabled = false;
        temp.GetComponent<Collider>().enabled = false;
        if(allyData.allyType == AllyType.Blocker)temp.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        temp.tag = "Untagged";
        placement.toPlace.towerModel = temp;
        placement.toPlace.allyData = allyData;
        placement.toPlace.towerToFollow = Instantiate(temp) as GameObject;
    }

    void Start()
    {
        types = allyData.getAttackType();

        AllyType type = allyData.allyType;
        switch (type)
        {
            case AllyType.Ranger:
                placable.GetComponent<Image>().sprite = ranger;
                break;
            case AllyType.Blocker:
                placable.GetComponent<Image>().sprite = melee;
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //Type1_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        // Type2_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        // Cost.GetComponent<Text>().text = "Cost: " + cost.ToString();
        try
        {
            String x = types[0];
            switch (x)
            {
                case "fire":
                    Type1.GetComponent<Image>().sprite = Fire;
                    break;
                case "water":
                    Type1.GetComponent<Image>().sprite = Water;
                    break;
                case "thunder":
                    Type1.GetComponent<Image>().sprite = Thunder;
                    break;
                case "wind":
                    Type1.GetComponent<Image>().sprite = Wind;
                    break;
            }

            if (types.Count == 2) x = types[1];

            switch (x)
            {
                case "fire":
                    Type2.GetComponent<Image>().sprite = Fire;
                    break;
                case "water":
                    Type2.GetComponent<Image>().sprite = Water;
                    break;
                case "thunder":
                    Type2.GetComponent<Image>().sprite = Thunder;
                    break;
                case "wind":
                    Type2.GetComponent<Image>().sprite = Wind;
                    break;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            Type1.GetComponent<Image>().sprite = physical;
            Type2.SetActive(false);
        }

    }
}
