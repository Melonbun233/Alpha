using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicpToPlace : MonoBehaviour
{
    public GameObject ranger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                //if (hit.transform.tag == "walls") {
                    AllyData data = DefaultAllyData.defaultBlockerData;
                    data.attackData.attackNumber = 2;
                    Ally.spawn(ranger, data, hit.transform.position + new Vector3(0, 1, 0), 
                        hit.transform.rotation);
                //}
            }
        }
    }
    
}
