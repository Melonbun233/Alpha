using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnclickUI : MonoBehaviour
{

    private Ally ally;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Placement.toPlace.instStatus(ally);
        }
    }





    private void Start()
    {
        ally = transform.GetComponent<Ally>();
    }
}
