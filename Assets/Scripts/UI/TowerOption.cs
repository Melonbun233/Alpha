using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
public class TowerOption : MonoBehaviour
{
    public int type1_lvl;
    public int type2_lvl;
    public List<String> types;
    private Ally ally;
    public GameObject placable;
    public GameObject Type1;
    public GameObject Type2;
    public GameObject Type1_lvl;
    public GameObject Type2_lvl;
    public GameObject TowerModel;
    public GameObject Cost;


    public Texture ranger;
    public Texture melee;
    public Texture Fire;
    public Texture Water;
    public Texture Wind;
    public Texture Thunder;


    public TowerOption(Ally ally)
    {
        this.ally = ally;
    }

    void OnMouseUp()
    {
        placement.toPlace.towerModel = TowerModel;
        placement.toPlace.towerToFollow = Instantiate(placement.toPlace.towerModel) as GameObject;
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
                placable.GetComponent<Image>().image = ranger;
                break;
            case AllyType.Blocker:
                placable.GetComponent<Image>().image = melee;
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
                Type1.GetComponent<Image>().image = Fire;
                break;
            case "water":
                Type1.GetComponent<Image>().image = Water;
                break;
            case "thunder":
                Type1.GetComponent<Image>().image = Thunder;
                break;
            case "wind":
                Type1.GetComponent<Image>().image = Wind;
                break;
        }

        x = types[1];

        switch (x)
        {
            case "fire":
                Type2.GetComponent<Image>().image = Fire;
                break;
            case "water":
                Type2.GetComponent<Image>().image = Water;
                break;
            case "thunder":
                Type2.GetComponent<Image>().image = Thunder;
                break;
            case "wind":
                Type2.GetComponent<Image>().image = Wind;
                break;
        }

    }
}
