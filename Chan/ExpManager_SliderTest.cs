using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ExpManager_SliderTest : MonoBehaviour
{
    [SerializeField] NoticeManager NM;
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] PeripheralImageController PIC;
    [SerializeField] AdjustCameraFOV FOV;

    public GameObject Notice_ExpEnd;
    public bool Term_SliderTest;
    public int PlayerAnswer;
    public float ST_MinimumFRS;
    float ThresholdTimer;
    public bool Term_ST_ProceedTask;
    public float TotalTestTime;
    public int AdjustmentCount;
    public int SliderCount;
    public float RT_AverageFRS;
    public TextMeshProUGUI T_FOV;

    void Start()
    {
        ResetAtStart();
    }

    void Update()
    {
        if (Term_SliderTest && Term_ST_ProceedTask)
            AdjustFoveation();
    }

    public void SetSliderTestCondition()
    {
        // foreach (float value in RT.LastFiveAnswers)
        //     RT_AverageFRS += value;

        // ST_MinimumFRS = RT_AverageFRS / 5;

        // test
        ST_MinimumFRS = 10;
        User.CameraFOV = ST_MinimumFRS;
    }

    void AdjustFoveation()
    {
        ThresholdTimer += Time.deltaTime;
        TotalTestTime += Time.deltaTime;

        if (ThresholdTimer >= 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3) && SliderCount < 15)
            {
                SliderCount++;
                PIC.TurnOffPeripheralImage();
                PIC.ST_TurnOnPeripheralImage();
                FOV.ST_AdjustCameraFOV();
                ThresholdTimer = 0;
                AdjustmentCount++;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && SliderCount > 0)
            {
                SliderCount--;
                PIC.TurnOffPeripheralImage();
                PIC.ST_TurnOnPeripheralImage();
                FOV.ST_AdjustCameraFOV();
                ThresholdTimer = 0;
                AdjustmentCount++;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) // save
            {
                Term_ST_ProceedTask = false;
                CSV_P.Save_CSV_Analysis();
                BlockEnd_SliderTest();
            }

            T_FOV.text = User.CameraFOV.ToString();
        }
    }

    public void BlockEnd_SliderTest()
    {
        if (RT.ConditionCount == 4)
            ExpEnd();
        else
        {
            NM.Term_BreakTime = true;
            ResetValue();
        }
    }

    void ResetValue()
    {
        RT_AverageFRS = 0;
        ST_MinimumFRS = 0;
        ThresholdTimer = 0;
        PlayerAnswer = 0;
        TotalTestTime = 0;
        AdjustmentCount = 0;
    }

    void ExpEnd()
    {
        Notice_ExpEnd.SetActive(true);
        Term_SliderTest = false;
        RT.Term_RandomTest = false;
    }

    void ResetAtStart()
    {
        Term_SliderTest = true;
        Term_ST_ProceedTask = true;
        PlayerAnswer = 0;
        ST_MinimumFRS = 0;
        RT_AverageFRS = 0;
        ThresholdTimer = 0;
    }
}
