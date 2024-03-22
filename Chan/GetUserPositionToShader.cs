using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetUserPositionToShader : MonoBehaviour
{
    [SerializeField] Raycast Raycast;
    [SerializeField] ExpManager ExpManager;
    float FoveaRegionRadius;
    float GroupSize;

    void Start()
    {
        FoveaRegionRadius = 1f;
        GroupSize = 1f;
        ExpManager.Trigger_AdjustFoveation = true;
    }

    void Update() 
    {
        Vector3 UserGazePoint = Raycast.UserGazePoint;
        Material ObjectMaterial = GetComponent<Renderer>().material;
        ObjectMaterial.SetVector("_GazePoint", new Vector4(UserGazePoint.x, UserGazePoint.y, UserGazePoint.z, 0));

        if (ExpManager.Trigger_AdjustFoveation)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FoveaRegionRadius++;
                Debug.Log("Fovea Region Radius is : " + FoveaRegionRadius);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                FoveaRegionRadius--;
                Debug.Log("Fovea Region Radius is : " + FoveaRegionRadius);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                GroupSize *= 2f;
                Debug.Log("Foveation is : 1/" + 1 / GroupSize);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                GroupSize /= 2f;
                Debug.Log("Foveation is : 1/" + 1 / GroupSize);
            }
            //ExpManager.Trigger_AdjustFoveation =false;
        }

        ObjectMaterial.SetFloat("_FoveaRegionRadius", FoveaRegionRadius);
        ObjectMaterial.SetFloat("_GroupSize", GroupSize);
    }
}
