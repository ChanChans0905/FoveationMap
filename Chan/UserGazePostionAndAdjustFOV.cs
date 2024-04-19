using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class UserGazePostionAndAdjustFOV : MonoBehaviour
{
    public Transform GazeTarget;
    public GameObject Notice_LookAtTheImage;
    RaycastHit hit;
    public TextMeshProUGUI TT;
    public float FoveaRegionSize;
    public float CameraFOV;
    public Vector3 UserGazePoint;
    float DistanceFromTheScreen;

    void Start()
    {
        FoveaRegionSize = 0;
        CameraFOV = 20f;
        DistanceFromTheScreen = 1.5f;
    }

    void Update()
    {
        // // Dynamic Foveated Rendering ( + Eye Tracking )
        // transform.LookAt(GazeTarget);

        if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("2D_Screen"))
            if (Notice_LookAtTheImage.activeSelf == true)
                Notice_LookAtTheImage.SetActive(false);
            else
            if (Notice_LookAtTheImage.activeSelf == false)
                Notice_LookAtTheImage.SetActive(true);

        UserGazePoint = hit.point;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 20f;
        UnityEngine.Debug.DrawRay(transform.position, forward, Color.green);
    }

    public void AdjustFoveation()
    {
        FoveaRegionSize = (Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * DistanceFromTheScreen);
        UnityEngine.Debug.Log("Fovea Region Radius is : " + CameraFOV);
        TT.text = CameraFOV.ToString();
    }
}
