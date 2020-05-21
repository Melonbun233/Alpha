using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamList : MonoBehaviour
{

    public List<Ally> team;
    public GameObject TowerTab;
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Ally x in team)
        {
            Instantiate(TowerTab, UI.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
