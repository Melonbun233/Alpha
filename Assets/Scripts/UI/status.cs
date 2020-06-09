using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public GameObject physical;
    public GameObject fire;
    public GameObject wind;
    public GameObject thunder;
    public GameObject water;
    public GameObject defense;
    public GameObject resistance;
    public GameObject hp;

    private AllyData allydata;

    public void setAllydata(AllyData data)
    {
        allydata = data;
    }

    private void OnMouseExit()
    {
        Destroy(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allydata != null)
        {
            fire.GetComponent<Text>().text = "Fire: " + allydata.attackData.attackDamage.getFireDamage().ToString();
            wind.GetComponent<Text>().text = "Wind: " + allydata.attackData.attackDamage.getWindDamage().ToString();
            thunder.GetComponent<Text>().text = "Thunder: " + allydata.attackData.attackDamage.getThunderDamage().ToString();
            water.GetComponent<Text>().text = "Water: " + allydata.attackData.attackDamage.getWaterDamage().ToString();
            physical.GetComponent<Text>().text = "Pyhsical: " + allydata.attackData.attackDamage.getPhysicalDamage().ToString();
            hp.GetComponent<Text>().text = "HP:   " + allydata.healthData.health + "/" + allydata.healthData.maxHealth;
            defense.GetComponent<Text>().text = "Defense: " + allydata.resistanceData.getPhysicalResistance();
            resistance.GetComponent<Text>().text = "Resistance: F" + allydata.resistanceData.getFireResistance() + " W" + allydata.resistanceData.getWaterResistance() + " T" + allydata.resistanceData.getThunderResistance() + " WT" + allydata.resistanceData.getWaterResistance();
        }
        
    }
}
