using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.Rendering;

public class UserGazePostionAndAdjustFOV : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    public Transform GazeTarget;
    public GameObject Notice_LookAtTheImage;
    RaycastHit hit;
    public float FoveaRegionSize;
    public float CameraFOV;
    public Vector3 UserGazePoint;
    float DistanceFromTheScreen;
    public float OutOfScreenTimer;

    void Start()
    {
        FoveaRegionSize = 0;
        CameraFOV = 0;
        DistanceFromTheScreen = 1.5f;
    }

    void Update()
    {
        // // Dynamic Foveated Rendering ( + Eye Tracking )
        // transform.LookAt(GazeTarget);

        if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("2D_Screen"))
        {
            Notice_LookAtTheImage.SetActive(false);
            RT.IsRestTime = false;
            OutOfScreenTimer = 0;
        }
        else
        {
            if (RT.Term_RT_ProceedTask || ST.Term_ST_ProceedTask)
            {
                Notice_LookAtTheImage.SetActive(true);
                RT.IsRestTime = true;
                OutOfScreenTimer = 1;
            }
        }

        UserGazePoint = hit.point;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 20f;
        UnityEngine.Debug.DrawRay(transform.position, forward, Color.green);
    }

    public void RT_AdjustCameraFOV()
    {
        if (RT.ConditionList[RT.ConditionCount] == 1) // UI
            CameraFOV = RT.FovOrder_HomeUI[RT.FovCount];
        else // Cinema, Web, Game
            CameraFOV = RT.FovOrder[RT.FovCount];

        /* #region 완전 랜덤하게 할 때 */
        // int CO = RT.ConditionOrder[RT.TaskCount];

        // if (RT.ConditionList[RT.ConditionCount] == 1) // UI
        // {
        //     if (CO == 0 || CO == 4 || CO == 8 || CO == 12)
        //         CameraFOV = 30;
        //     else if (CO == 1 || CO == 5 || CO == 9 || CO == 13)
        //         CameraFOV = 40;
        //     else if (CO == 2 || CO == 6 || CO == 10 || CO == 14)
        //         CameraFOV = 50;
        //     else if (CO == 3 || CO == 7 || CO == 11 || CO == 15)
        //         CameraFOV = 60;
        // }
        // else // Cinema, Web, Game
        // {
        //     if (CO == 0 || CO == 4 || CO == 8 || CO == 12)
        //         CameraFOV = 20;
        //     else if (CO == 1 || CO == 5 || CO == 9 || CO == 13)
        //         CameraFOV = 30;
        //     else if (CO == 2 || CO == 6 || CO == 10 || CO == 14)
        //         CameraFOV = 40;
        //     else if (CO == 3 || CO == 7 || CO == 11 || CO == 15)
        //         CameraFOV = 50;
        // }
        /* #endregion */

        AdjustFoveation();
    }

    public void ST_AdjustCameraFOV()
    {
        int RC = ST.RepetitionCount;

        if (RT.ConditionList[RT.ConditionCount] == 1) // UI
            CameraFOV = ST.FovOrder_HomeUI[RC];
        else // Cinema, Web, Game
            CameraFOV = ST.FovOrder[RC];

        AdjustFoveation();
    }

    public void AdjustFoveation()
    {
        FoveaRegionSize = Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * DistanceFromTheScreen;
        //UnityEngine.Debug.Log("Fovea Region Radius is : " + CameraFOV);
    }
}
