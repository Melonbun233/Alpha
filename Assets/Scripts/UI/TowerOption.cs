using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerOption : MonoBehaviour
{
    public int attack_lvl;
    public int Armor;
    public int FireRes;
    public int WaterRes;
    public int WindRes;
    public int ThunderRes;
    public int cost;
    public float cooldown;
    public float attackSpeed;
    public float attackRange;
    public GameObject placable;
    public GameObject Type1;
    public GameObject Type2;
    public GameObject Type1_lvl;
    public GameObject Type2_lvl;
    public GameObject TowerModel;
    public GameObject Cost;


    public Sprite Ranger;
    public Sprite Melee;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;

    void OnMouseUp()
    {
        placement.toPlace.towerModel = TowerModel;
        placement.toPlace.towerToFollow = Instantiate(placement.toPlace.towerModel) as GameObject;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Type1_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        Type2_lvl.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        Cost.GetComponent<Text>().text = "Cost: " + cost.ToString();
    }
}
