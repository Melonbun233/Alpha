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
    // Start is called before the first frame update

    public TeamList(List<AllyData> allydatas)
    {
        this.team = allydatas;
    }

    void Start()
    {
        foreach (AllyData ally in team)
        {
            GameObject tower = Instantiate(towerTab, UI.transform) as GameObject;
            tower.GetComponent<RectTransform>().anchoredPosition = startPoint;
            startPoint.x = startPoint.x + offset;
            tower.GetComponent<TowerOption>().allyData = ally;
            tower.GetComponent<TowerOption>().cd = ally.allyLevelData.cd;
        }
    }
}
