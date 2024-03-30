using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetUserPositionToShader : MonoBehaviour
{
    [SerializeField] Raycast Raycast;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    float FoveaRegionSize;
    public bool Term_AdjustFoveation;
    public float CameraFOV;
    Vector3 UserGazePoint;
    Material ObjectMaterial;

    void Start()
    {
        Term_AdjustFoveation = false;
        FoveaRegionSize = 1f;
        CameraFOV = 10f;
        ObjectMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        UserGazePoint = Raycast.UserGazePoint;
        ObjectMaterial.SetVector("UserGazePoint", new Vector4(UserGazePoint.x, UserGazePoint.y, UserGazePoint.z, 0));

        if (Term_AdjustFoveation)
            AdjustFoveation();
    }

    void AdjustFoveation()
    {
        FoveaRegionSize = 2 * (Mathf.Tan(Mathf.Deg2Rad * CameraFOV / 2f) * 1.5f);
        Debug.Log("Fovea Region Radius is : " + CameraFOV);
        ObjectMaterial.SetFloat("FoveaRegionSize", FoveaRegionSize);
        Term_AdjustFoveation = false;
    }
}
