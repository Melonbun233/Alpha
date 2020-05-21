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
    public Ally ally;
    public GameObject placable;
    public GameObject Type1;
    public GameObject Type2;
    public GameObject Type1_lvl;
    public GameObject Type2_lvl;
    public GameObject TowerModel;
    public GameObject Cost;
    public GameObject[] prefabs;


    public Sprite ranger;
    public Sprite melee;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;



    public TowerOption(Ally ally)
    {
        this.ally = ally;
    }

    public GameObject allyPrefab()
    {
        AllyType type = ally.type;
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
        placement.toPlace.towerOption = temp;
        placement.toPlace.towerToFollow = Instantiate(placement.toPlace.towerOption) as GameObject;
    }

    void Start()
    {
        if (ally.attackDamage.getFireDamage() != 0) types.Add("fire");
        if (ally.attackDamage.getWaterDamage() != 0) types.Add("water");
        if (ally.attackDamage.getThunderDamage() != 0) types.Add("thunder");
        if (ally.attackDamage.getWindDamage() != 0) types.Add("wind");
        if (ally.attackDamage.getPhysicalDamage() != 0) types.Add("physic");
        AllyType type = ally.type;
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

        if(types.Count == 2)x = types[1];

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
}
