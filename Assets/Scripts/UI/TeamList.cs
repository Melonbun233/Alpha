using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamList : MonoBehaviour
{

    public List<AllyData> team;
    public GameObject towerTab;
    public GameObject UI;
    public Vector2 startPoint;
    public float offset;

    private LevelController levelController;


    void Awake()
    {
        levelController = LevelController.getLevelController();
        UI = levelController.UI;
    }

    public void addTowerOption(AllyData allyData) {
        team.Add(allyData);
        GameObject tower = Instantiate(towerTab, UI.transform) as GameObject;
        tower.GetComponent<RectTransform>().anchoredPosition = startPoint;
        startPoint.x = startPoint.x + offset;
        tower.GetComponent<TowerTab>().allyData = allyData;
        tower.GetComponent<TowerTab>().cd = allyData.allyLevelData.cd;
    }

    public void addTowerOptions(List<AllyData> allyDatas) {
        foreach (AllyData allyData in allyDatas) {
            addTowerOption(allyData);
        }
    }
}
