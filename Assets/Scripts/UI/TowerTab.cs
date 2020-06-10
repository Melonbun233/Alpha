using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TowerTab : MonoBehaviour
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
    public Image cdIcon;
    public AllyData allyData;

    [Header("Sprites for UI icons")]
    public Sprite ranger;
    public Sprite melee;
    public Sprite physical;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Wind;
    public Sprite Thunder;

    private LevelController levelController;
    private PlacementController placementController;

    void Awake()
    {
        levelController = LevelController.getLevelController();
        placementController = LevelController.getPlacementController();

        GameObject towerVisual = Instantiate(getAllyModel(allyData.getMainType()), 
            TowerVisual.transform);
        towerVisual.transform.localScale = new Vector3(20, 20, 20);
        isCd = false;
        button = gameObject.GetComponent<Button>();
    }

    public GameObject getAllyPrefab(AllyType type)
    {
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

    public GameObject getAllyModel(AllyType type)
    {
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
        if (Input.GetMouseButtonDown(1) && status==null)
        {
            status = placementController.instStatus(allyData);
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
        if(placementController.towerPreview != null)
        {
            Destroy(placementController.towerPreview);
        }

        if ((int)placementController.mana < allyData.allyLevelData.cost)
        {
            print("Not Enough Mana");
            return;
        }

        if (!isCd)
        {
            placementController.allyData = allyData;
            placementController.towerPrefab = getAllyPrefab(allyData.getMainType());;
            placementController.towerPreview = Instantiate(getAllyModel(allyData.getMainType()));
            placementController.towerOption = this;
        } else {
            print("This Tower is on CD!");
            return;
        }
    }

    void updateCD() {
        if (isCd)
        {
            cdIcon.fillAmount += 1 / cd * Time.deltaTime;
        }

        if(cdIcon.fillAmount >= 1)
        {
            isCd = false;
            cdIcon.fillAmount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCD();
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

        if (levelController.levelEnded()) {
            enabled = false;
        }

        if (isCd) {
            button.enabled = false;
            highLight.SetActive(false);
        } else {
            button.enabled = true;
            highLight.SetActive(true);
        }

        if ((int)placementController.mana < allyData.allyLevelData.cost) {
            button.enabled = false;
            highLight.SetActive(false);
        } else {
            button.enabled = true;
            highLight.SetActive(true);
        }
    }
}
