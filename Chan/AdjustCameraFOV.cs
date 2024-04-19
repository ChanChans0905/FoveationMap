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
        int TC = RT.ConditionOrder[RT.TaskCount];

        if (TC < 3)
            CameraFOV = 25;
        else if (TC < 6)
            CameraFOV = 30;
        else if (TC < 9)
            CameraFOV = 35;
        else if (TC < 12)
            CameraFOV = 40;
        else if (TC < 15)
            CameraFOV = 45;

        User.CameraFOV = CameraFOV;
        User.AdjustFoveation();
    }

    public void ST_AdjustCameraFOV()
    {
        int SC = ST.SliderCount;

        if (SC < 3)
            CameraFOV = 45;
        else if (SC < 6)
            CameraFOV = 40;
        else if (SC < 9)
            CameraFOV = 35;
        else if (SC < 12)
            CameraFOV = 30;
        else if (SC < 15)
            CameraFOV = 25;

        User.CameraFOV = CameraFOV;
        User.AdjustFoveation();
    }
}
