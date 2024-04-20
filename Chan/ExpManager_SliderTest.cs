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

    public TextMeshProUGUI T_MinCondition, T_MaxCondition;
    public GameObject Notice_MinCondition, Notice_MaxCondition;
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
    float MinMaxNoticeTimer;
    bool Bool_MinMaxNoticeTimer;

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

        User.CameraFOV = 45;
    }

    void AdjustFoveation()
    {
        ThresholdTimer += Time.deltaTime;
        TotalTestTime += Time.deltaTime;
        Debug.Log("RT 컨디션 : " + RT.ConditionList[RT.ConditionCount]);

        if (ThresholdTimer >= 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (SliderCount < 14)
                {
                    SliderCount++;
                    PIC.TurnOffPeripheralImage();
                    PIC.ST_TurnOnPeripheralImage();
                    FOV.ST_AdjustCameraFOV();
                    ThresholdTimer = 0;
                    AdjustmentCount++;
                }
                else
                {
                    Bool_MinMaxNoticeTimer = true;
                    Notice_MaxCondition.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (SliderCount > 0)
                {
                    SliderCount--;
                    PIC.TurnOffPeripheralImage();
                    PIC.ST_TurnOnPeripheralImage();
                    FOV.ST_AdjustCameraFOV();
                    ThresholdTimer = 0;
                    AdjustmentCount++;
                }
                else
                {
                    Bool_MinMaxNoticeTimer = true;
                    Notice_MinCondition.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) // save
            {
                Term_ST_ProceedTask = false;
                CSV_P.Save_CSV_Analysis();
                BlockEnd_SliderTest();
            }

            T_FOV.text = User.CameraFOV.ToString();

            if (Bool_MinMaxNoticeTimer)
                MinMaxNoticeTimer += Time.deltaTime;

            if (MinMaxNoticeTimer > 2)
            {
                Notice_MaxCondition.SetActive(false);
                Notice_MinCondition.SetActive(false);
                MinMaxNoticeTimer = 0;
                Bool_MinMaxNoticeTimer = false;
            }
        }
    }

    public void BlockEnd_SliderTest()
    {
        if (RT.ConditionCount == 3)
        {
            Notice_MaxCondition.SetActive(false);
            Notice_MinCondition.SetActive(false);
            PIC.ST_TurnOffPeripheralImageParent();
            ExpEnd();
        }
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
        Bool_MinMaxNoticeTimer = false;
        MinMaxNoticeTimer = 0;
        SliderCount = 0;
        Notice_MaxCondition.SetActive(false);
        Notice_MinCondition.SetActive(false);
        PIC.ST_TurnOffPeripheralImageParent();
    }

    void ExpEnd()
    {
        Notice_ExpEnd.SetActive(true);
        Term_SliderTest = false;
    }

    void ResetAtStart()
    {
        Term_SliderTest = false;
        Term_ST_ProceedTask = false;
        PlayerAnswer = 0;
        ST_MinimumFRS = 0;
        RT_AverageFRS = 0;
        ThresholdTimer = 0;
    }
}
