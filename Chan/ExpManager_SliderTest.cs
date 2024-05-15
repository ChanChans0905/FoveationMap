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
    [SerializeField] CSV_Save_Processed_ST CSV_P_ST;
    [SerializeField] CSV_Save_Raw CSV_R;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] PeripheralImageController PIC;

    public TextMeshProUGUI T_MinCondition, T_MaxCondition;
    public GameObject Notice_MinCondition, Notice_MaxCondition;
    public GameObject Notice_ExpEnd;
    public GameObject Notice_TaskEnd;
    public bool Term_SliderTest;
    public int PlayerAnswer;
    public float ST_MinimumFRS;
    float ThresholdTimer;
    public bool Term_ST_ProceedTask;
    public float TotalTestTime;
    public int SliderCount;
    public float RT_AverageFRS;
    float MinMaxNoticeTimer;
    bool Bool_MinMaxNoticeTimer;
    public int RepetitionCount;
    bool Term_ST_MovetoNextTask;
    public int[] FovOrder;
    public int[] FovOrder_HomeUI;
    float VideoLoopTimer;

    void Start()
    {
        FovOrder = new int[] { 20, 20, 30, 30, 40, 40, 50, 50 };
        RT.ShuffleArray(FovOrder);
        FovOrder_HomeUI = new int[] { 30, 30, 40, 40, 50, 50, 60, 60 };
        RT.ShuffleArray(FovOrder_HomeUI);
        ResetAtStart();
    }

    void Update()
    {
        if (Term_SliderTest && Term_ST_ProceedTask)
            AdjustFoveation();

        if (Term_ST_MovetoNextTask)
            MoveToNextTask();
    }

    void FixedUpdate()
    {
        if (Term_SliderTest && Term_ST_ProceedTask)
        {
            ThresholdTimer += Time.deltaTime;
            TotalTestTime += Time.deltaTime;
            VideoLoopTimer += Time.deltaTime;
        }
    }

    /* #region RT 결과 적용할 시 */
    // public void SetSliderTestCondition()
    // {
    //     // foreach (float value in RT.LastFiveAnswers)
    //     //     RT_AverageFRS += value;

    //     // ST_MinimumFRS = RT_AverageFRS / 5;

    //     User.CameraFOV = 20;
    // }
    /* #endregion */

    void AdjustFoveation()
    {
        if (ThresholdTimer >= 0.3f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                HandleSliderChange(1, 9, Notice_MaxCondition);
            else if (Input.GetKeyDown(KeyCode.Alpha1))
                HandleSliderChange(-1, 0, Notice_MinCondition);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // save
            {
                PIC.ST_TurnOnOffPI(false);
                PIC.ST_TurnOnOffPIP(false);
                CSV_P_ST.Save_CSV_Analysis();
                RepetitionCount++;
                Term_ST_ProceedTask = false;

                ResetAfterEachTask();

                if (RepetitionCount == 8)
                    BlockEnd_SliderTest();
                else
                    Term_ST_MovetoNextTask = true;
            }

            if (Bool_MinMaxNoticeTimer)
                MinMaxNoticeTimer += Time.deltaTime;

            if (MinMaxNoticeTimer > 1.5f)
            {
                Notice_MaxCondition.SetActive(false);
                Notice_MinCondition.SetActive(false);
                MinMaxNoticeTimer = 0;
                Bool_MinMaxNoticeTimer = false;
            }
        }

        if (VideoLoopTimer > 8)
        {
            PIC.ST_TurnOnOffPIP(false);
            PIC.ST_TurnOnOffPI(false);
            PIC.ST_TurnOnOffPIP(true);
            PIC.ST_TurnOnOffPI(true);
            VideoLoopTimer = 0;
        }
    }

    void HandleSliderChange(int increment, int limit, GameObject notice)
    {
        if ((increment == 1 && SliderCount < limit) || (increment == -1 && SliderCount > limit))
        {
            RT.Block.SetActive(true);
            PIC.ST_TurnOnOffPIP(false);
            PIC.ST_TurnOnOffPI(false);
            SliderCount += increment;
            PIC.ST_TurnOnOffPIP(true);
            PIC.ST_TurnOnOffPI(true);
            VideoLoopTimer = 0;
            ThresholdTimer = 0;
        }
        else
        {
            Bool_MinMaxNoticeTimer = true;
            notice.SetActive(true);
        }
    }

    void MoveToNextTask()
    {
        ThresholdTimer += Time.deltaTime;
        Notice_TaskEnd.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Alpha2) && ThresholdTimer > 1.5f)
        {
            CSV_R.New_CSV_File();
            SetStartCondition();
            Notice_TaskEnd.SetActive(false);
            ThresholdTimer = 0;
            Term_ST_ProceedTask = true;
            Term_ST_MovetoNextTask = false;
        }
    }

    public void SetStartCondition()
    {
        RT.Block.SetActive(false);
        User.ST_AdjustCameraFOV();
        PIC.ST_TurnOnOffPIP(true);
        PIC.ST_TurnOnOffPI(true);
    }

    public void BlockEnd_SliderTest()
    {
        if (RT.ConditionCount == 3)
        {
            Notice_MaxCondition.SetActive(false);
            Notice_MinCondition.SetActive(false);
            PIC.ST_TurnOnOffPIP(false);
            ExpEnd();
        }
        else
        {
            RT.ShuffleArray(FovOrder);
            RT.ShuffleArray(FovOrder_HomeUI);
            NM.Term_BreakTime = true;
            RepetitionCount = 0;
        }
    }

    void ResetAfterEachTask()
    {
        ThresholdTimer = 0;
        TotalTestTime = 0;
        Bool_MinMaxNoticeTimer = false;
        MinMaxNoticeTimer = 0;
        SliderCount = 0;
        Notice_MaxCondition.SetActive(false);
        Notice_MinCondition.SetActive(false);
        PIC.ST_TurnOnOffPIP(false);
        User.OutOfScreenTimer = 0;
        VideoLoopTimer = 0;
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
        Bool_MinMaxNoticeTimer = false;
        PlayerAnswer = 0;
        ST_MinimumFRS = 0;
        RT_AverageFRS = 0;
        ThresholdTimer = 0;
    }
}
