using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlight : MonoBehaviour
{

    Ray cursor;
    RaycastHit hit;
    int LastLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cursor, out hit))
        {
            LastLayer = hit.transform.gameObject.layer;
            hit.transform.gameObject.layer = 10;
        }

        

    }
}
