using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Transform GazeTarget;
    public Vector3 UserGazePoint;
    RaycastHit hit;

    void Update()
    {

        // // Dynamic Foveated Rendering ( + Eye Tracking )
        // transform.LookAt(GazeTarget);

        if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("2D_Screen"))
            UserGazePoint = hit.point;

        Debug.Log(UserGazePoint);
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 100;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
