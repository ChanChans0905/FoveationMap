using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyConditionToShader : MonoBehaviour
{
    [SerializeField] UserGazePostionAndAdjustFOV User;
    Vector3 UserGazePoint;
    float FoveaRegionSize;

    void Update()
    {
        Material ObjectMaterial = GetComponent<Renderer>().material;
        UserGazePoint = User.UserGazePoint;
        FoveaRegionSize = User.FoveaRegionSize;
        ObjectMaterial.SetVector("UserGazePoint", new Vector4(UserGazePoint.x, UserGazePoint.y, UserGazePoint.z, 0));
        ObjectMaterial.SetFloat("FoveaRegionSize", FoveaRegionSize);
    }
}
