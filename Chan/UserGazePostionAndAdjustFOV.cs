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
    public TextMeshProUGUI TT;
    public float FoveaRegionSize;
    public float CameraFOV;
    public Vector3 UserGazePoint;
    float DistanceFromTheScreen;
    public float OutOfScreenTimer;

    void Start()
    {
        FoveaRegionSize = 0;
        CameraFOV = 20f;
        DistanceFromTheScreen = 1.5f;
    }

    void FixedUpdate()
    {
        // // Dynamic Foveated Rendering ( + Eye Tracking )
        // transform.LookAt(GazeTarget);

        if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("2D_Screen"))
        {
            Notice_LookAtTheImage.SetActive(false);
            RT.IsRestTime = false;
        }
        else
        {
            if (RT.Term_RT_ProceedTask || ST.Term_ST_ProceedTask)
            {
                Notice_LookAtTheImage.SetActive(true);
                RT.IsRestTime = true;
                OutOfScreenTimer += Time.deltaTime;
            }
        }

        UserGazePoint = hit.point;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 20f;
        UnityEngine.Debug.DrawRay(transform.position, forward, Color.green);
    }

    public void RT_AdjustCameraFOV()
    {
        int CO = RT.ConditionOrder[RT.TaskCount];

        if (CO == 0 || CO == 4 || CO == 8 || CO == 12)
            CameraFOV = 25;
        else if (CO == 1 || CO == 5 || CO == 9 || CO == 13)
            CameraFOV = 30;
        else if (CO == 2 || CO == 6 || CO == 10 || CO == 14)
            CameraFOV = 35;
        else if (CO == 3 || CO == 7 || CO == 11 || CO == 15)
            CameraFOV = 40;

        AdjustFoveation();
    }

    public void ST_AdjustCameraFOV()
    {
        int RC = ST.RepetitionCount;

        if (RC == 0)
            CameraFOV = 20;
        else if (RC == 2)
            CameraFOV = 30;
        else if (RC == 4)
            CameraFOV = 35;
        else if (RC == 6)
            CameraFOV = 40;
    }

    public void AdjustFoveation()
    {
        FoveaRegionSize = Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * DistanceFromTheScreen;
        //UnityEngine.Debug.Log("Fovea Region Radius is : " + CameraFOV);
        TT.text = CameraFOV.ToString();
    }
}
