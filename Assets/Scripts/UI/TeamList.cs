using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI controller for a list of TowerTab
public class TeamList : MonoBehaviour
{

    public List<AllyData> team;
    public GameObject towerTabPrefab;
    public GameObject UI;
    public Vector2 startPoint;
    public float offset;

    private LevelSceneController levelSceneController;


    void Awake()
    {
        levelSceneController = LevelSceneController.getLevelSceneController();
        UI = levelSceneController.UI;
    }

    public void addTowerOption(AllyData allyData) {
        team.Add(allyData);
        GameObject towerTabObject = Instantiate(towerTabPrefab, UI.transform) as GameObject;
        towerTabObject.GetComponent<RectTransform>().anchoredPosition = startPoint;
        startPoint.x = startPoint.x + offset;

        towerTabObject.GetComponent<TowerTab>().setupTowerTab(allyData);
    }

    public void addTowerOptions(List<AllyData> allyDatas) {
        foreach (AllyData allyData in allyDatas) {
            addTowerOption(allyData);
        }
    }
}
