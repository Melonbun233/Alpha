using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwtichButton : MonoBehaviour
{

    private void OnMouseUp()
    {
        Placement.toPlace.CameraAngleSwitch();
    }

}
