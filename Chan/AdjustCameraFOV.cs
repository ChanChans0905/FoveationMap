using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCameraFOV : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    public int CameraFOV;

    public void RT_AdjustCameraFOV()
    {
        if (RT.ConditionOrder[RT.ConditionCount] < 3)
            CameraFOV = 25;
        else if (RT.ConditionOrder[RT.ConditionCount] < 6)
            CameraFOV = 30;
        else if (RT.ConditionOrder[RT.ConditionCount] < 9)
            CameraFOV = 35;
        else if (RT.ConditionOrder[RT.ConditionCount] < 12)
            CameraFOV = 40;
        else if (RT.ConditionOrder[RT.ConditionCount] < 5)
            CameraFOV = 45;

        User.CameraFOV = CameraFOV;
        User.AdjustFoveation();
    }

    public void ST_AdjustCameraFOV()
    {
        if (ST.SliderCount < 3)
            CameraFOV = 25;
        else if (ST.SliderCount < 6)
            CameraFOV = 30;
        else if (ST.SliderCount < 9)
            CameraFOV = 35;
        else if (ST.SliderCount < 12)
            CameraFOV = 40;
        else if (ST.SliderCount < 15)
            CameraFOV = 45;

        User.CameraFOV = CameraFOV;
        User.AdjustFoveation();
    }
}
