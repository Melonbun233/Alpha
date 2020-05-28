using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwtichButton : MonoBehaviour
{

    private void OnMouseUp()
    {
        placement.toPlace.CameraAngleSwitch();
    }

}
