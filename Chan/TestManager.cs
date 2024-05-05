using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // test 작동 조건 : exp_manager 오브젝트 내 다른 코드 전부 비활성, Notice manager 오브젝트 코드 비활성 
    // csvManager 오브젝트 비활성 

    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] GameObject Peri_1K, Peri_2K, Peri_3K;
    public TextMeshProUGUI Tx_Resol;
    int Count_FOV, Count_Resol;
    bool AdjustFOV, AdjustResol;
    int DegreeFovAdjustment;
    float ThresholdTimer;

    void Start()
    {
        Count_FOV = 10;
        Count_Resol = 0;
        DegreeFovAdjustment = 5;
        Peri_1K.SetActive(true);
        Tx_Resol.text = "1K";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AdjustFOV = true;
            AdjustResol = false;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            AdjustFOV = false;
            AdjustResol = true;
        }

        ThresholdTimer += Time.deltaTime;

        if (AdjustFOV && ThresholdTimer > 0.2f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Count_FOV > 10)
            {
                Count_FOV -= DegreeFovAdjustment;
                ThresholdTimer = 0;
            }

            else if (Input.GetKeyDown(KeyCode.Alpha3) && Count_FOV < 65)
            {
                Count_FOV += DegreeFovAdjustment;
                ThresholdTimer = 0;
            }
        }

        User.CameraFOV = Count_FOV;
        User.AdjustFoveation();

        if (AdjustResol && ThresholdTimer > 0.2f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Count_Resol > 1)
            {
                Count_Resol--;
                ThresholdTimer = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && Count_Resol < 3)
            {
                Debug.Log(Count_Resol);
                Count_Resol++;
                ThresholdTimer = 0;
            }

            switch (Count_Resol)
            {
                case 1:
                    TurnOffPeripheralImage();
                    Peri_1K.SetActive(true);
                    Tx_Resol.text = "1K";
                    break;
                case 2:
                    TurnOffPeripheralImage();
                    Peri_2K.SetActive(true);
                    Tx_Resol.text = "2K";
                    break;
                case 3:
                    TurnOffPeripheralImage();
                    Peri_3K.SetActive(true);
                    Tx_Resol.text = "3K";
                    break;
            }
        }
    }

    void TurnOffPeripheralImage()
    {
        Peri_1K.SetActive(false);
        Peri_2K.SetActive(false);
        Peri_3K.SetActive(false);
    }
}
