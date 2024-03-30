using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager_RandomTest : MonoBehaviour
{
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] NoticeManager NM;
    [SerializeField] GetUserPositionToShader User;
    public GameObject Notice_ExpEnd;
    public bool Term_RandomTest;
    public int PlayerAnswer;
    float ST_AverageFRS;
    public float RT_MinimumFRS;
    float ThresholdTimer;
    public bool Term_ProceedTask;

    void Start()
    {
        ResetAtStart();
    }

    void Update()
    {
        if (Term_RandomTest && Term_ProceedTask)
            AdjustFoveation();
    }

    public void SetRandomTestCondition()
    {
        // 확인 필요, 마지막 5번 횟수 or 5번 reversal 할 때 당시의 foveation?
        for (int i = 0; i < 5; i++)
        {
            ST_AverageFRS += ST.LastFiveAnswers[i];
        }
        ST_AverageFRS = Mathf.Floor(ST_AverageFRS / 5);
        RT_MinimumFRS = ST_AverageFRS;
    }

    void AdjustFoveation()
    {

        ThresholdTimer += Time.deltaTime;

        if (ThresholdTimer >= 1)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                User.CameraFOV += 5;
                ThresholdTimer = 0;
                User.Term_AdjustFoveation = true;
            }
            if (Input.GetKeyDown(KeyCode.K) && RT_MinimumFRS > ST_AverageFRS)
            {
                User.CameraFOV -= 5;
                ThresholdTimer = 0;
                User.Term_AdjustFoveation = true;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                NM.Term_BreakTime = true;
                CSV_P.Save_CSV_Analysis();
                BlockEnd_RandomTest();
            }
        }
    }

    void ResetValue()
    {
        ST_AverageFRS = 0;
        RT_MinimumFRS = 0;
        ThresholdTimer = 0;
        PlayerAnswer = 0;
    }

    public void BlockEnd_RandomTest()
    {
        if (ST.ConditionCount == 4)
            ExpEnd();
        else
        {
            ST.ChangeCondition();
            NM.Term_BreakTime = true;
            ResetValue();
        }
    }

    void ExpEnd()
    {
        Notice_ExpEnd.SetActive(true);
        Term_RandomTest = false;
        ST.Term_SliderTest = false;
    }

    void ResetAtStart()
    {
        Term_RandomTest = false;
        Term_ProceedTask = false;
        PlayerAnswer = 0;
        RT_MinimumFRS = 0;
        ST_AverageFRS = 0;
        ThresholdTimer = 0;
    }
}
