using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamList : MonoBehaviour
{

    public List<Ally> team;
    public GameObject towerTab;
    public GameObject UI;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {

        foreach(Ally ally in team)
        {
            GameObject tower = Instantiate(towerTab, UI.transform) as GameObject;
            tower.GetComponent<RectTransform>().anchoredPosition = offset;
            offset.x = offset.x + 135f;
            tower.GetComponent<TowerOption>().ally = ally;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
