using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TowerOption : MonoBehaviour
{
    public int type1_lvl;
    public int type2_lvl;
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
        AllyType type = allyData.allyType1;
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
        //temp.GetComponent<Collider>().enabled = false;
        if(allyData.isType(AllyType.Blocker))temp.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        temp.tag = "allyToPlace";
        placement.toPlace.towerModel = temp;
        placement.toPlace.allyData = allyData;
        placement.toPlace.towerToFollow = Instantiate(temp) as GameObject;
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //Type1_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        // Type2_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        // Cost.GetComponent<Text>().text = "Cost: " + cost.ToString();
            AllyType x = allyData.allyType1;
            switch (x)
            {
                case AllyType.Fire:
                    Type1.GetComponent<Image>().sprite = Fire;
                    break;
                case AllyType.Water:
                    Type1.GetComponent<Image>().sprite = Water;
                    break;
                case AllyType.Thunder:
                    Type1.GetComponent<Image>().sprite = Thunder;
                    break;
                case AllyType.Wind:
                    Type1.GetComponent<Image>().sprite = Wind;
                    break;
                case AllyType.Ranger:
                    Type1.GetComponent<Image>().sprite = ranger;
                    break;
                case AllyType.Blocker:
                    Type1.GetComponent<Image>().sprite = physical;
                    break;
            }

            AllyType z = allyData.allyType2;

            switch (z)
            {
                case AllyType.Fire:
                    Type2.GetComponent<Image>().sprite = Fire;
                    break;
                case AllyType.Water:
                    Type2.GetComponent<Image>().sprite = Water;
                    break;
                case AllyType.Thunder:
                    Type2.GetComponent<Image>().sprite = Thunder;
                    break;
                case AllyType.Wind:
                    Type2.GetComponent<Image>().sprite = Wind;
                    break;
                case AllyType.Ranger:
                    Type2.GetComponent<Image>().sprite = ranger;
                    break;
                case AllyType.Blocker:
                    Type2.GetComponent<Image>().sprite = physical;
                    break;
                case AllyType.None:
                    Type2.SetActive(false);
                    break;
            }


    }
}
