using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onclickUI : MonoBehaviour
{

    private Ally ally;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            placement.toPlace.instStatus(ally);
        }
    }





    private void Start()
    {
        ally = transform.GetComponent<Ally>();
    }
}
