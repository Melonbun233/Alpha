using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamList : MonoBehaviour
{

    public List<AllyData> team;
    public GameObject towerTab;
    public GameObject UI;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {

        team.Add(DefaultAllyData.defaultRangerData);
        team.Add(DefaultAllyData.defaultBlockerData);

        foreach(AllyData ally in team)
        {
            GameObject tower = Instantiate(towerTab, UI.transform) as GameObject;
            tower.GetComponent<RectTransform>().anchoredPosition = offset;
            offset.x = offset.x + 130f;
            tower.GetComponent<TowerOption>().allyData = ally;
            tower.GetComponent<TowerOption>().cd = ally.allyLevelData.cd;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
