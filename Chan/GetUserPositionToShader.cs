using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetUserPositionToShader : MonoBehaviour
{
    [SerializeField] Raycast Raycast;
    [SerializeField] ExpManager ExpManager;
    float FoveaRegionSize;
    float CameraFOV;
    Vector3 UserGazePoint;
    Material ObjectMaterial;

    void Start()
    {
        FoveaRegionSize = 1f;
        CameraFOV = 10f;
        ExpManager.Trigger_AdjustFoveation = true;
        ObjectMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        UserGazePoint = Raycast.UserGazePoint;
        ObjectMaterial.SetVector("UserGazePoint", new Vector4(UserGazePoint.x, UserGazePoint.y, UserGazePoint.z, 0));

        if (ExpManager.Trigger_AdjustFoveation)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CameraFOV -= 5;
                FoveaRegionSize = 2 * (Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * 1.5f);
                Debug.Log("Fovea Region Radius is : " + CameraFOV);
                ObjectMaterial.SetFloat("FoveaRegionSize", FoveaRegionSize);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                CameraFOV += 5;
                FoveaRegionSize = 2 * (Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * 1.5f);
                Debug.Log("Fovea Region Radius is : " + CameraFOV);
                ObjectMaterial.SetFloat("FoveaRegionSize", FoveaRegionSize);
            }
        }
    }
}
