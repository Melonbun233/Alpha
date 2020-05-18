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
    public GameObject Type1_dmg;
    public GameObject Type2_dmg;
    public GameObject TowerModel;
    public GameObject Cost;


    public Sprite Ranger;
    public Sprite Melee;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Type1_dmg.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        Type2_dmg.GetComponent<Text>().text = "Level: " + attack_lvl.ToString();
        Cost.GetComponent<Text>().text = cost.ToString();
    }
}
