using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    public GameObject Notice_SelectSample, Notice_Welcome, Notice_GameStart;
    public GameObject Notice_ST_BreakStart, Notice_ST_Start;
    public GameObject Notice_RT_BreakStart, Notice_RT_Start;
    public Text Text_SampleNumber;
    bool CreateCSVfile;
    float ThresholdTimer;
    public int SampleNumber;
    int Next;
    bool Term_SelectSampleNumber, Term_ExpStart;
    public bool Term_Notice_NewCondition;
    public bool Term_BreakTime;
    float BreakTimer;

    void Start()
    {
        ResetAtStart();
    }

    void FixedUpdate()
    {
        if (Term_SelectSampleNumber && ThresholdTimer > 0.3f)
            SelectSampleNumber();

        if (Term_ExpStart)
            ExpStart();

        if (CreateCSVfile)
        {
            CSV_P.New_CSV_File();
            ST.ChangeCondition();
            CreateCSVfile = false;
        }

        if (Term_BreakTime)
            BreakTime();

        if (Term_Notice_NewCondition)
            Notice_NewCondition();
    }

    void SelectSampleNumber()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SampleNumber++;
            ThresholdTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SampleNumber--;
            ThresholdTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SampleNumber += 10;
            ThresholdTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SampleNumber -= 10;
            ThresholdTimer = 0;
        }

        Text_SampleNumber.text = SampleNumber.ToString();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Term_ExpStart = true;
            Term_SelectSampleNumber = false;
            Notice_SelectSample.SetActive(false);
            Notice_Welcome.SetActive(true);
            ThresholdTimer = 0;
        }
    }

    void ExpStart()
    {
        if (ThresholdTimer > 1.0f && Input.GetKeyDown(KeyCode.K))
        {
            Next++;
            ThresholdTimer = 0;

            switch (Next)
            {
                case 1:
                    Notice_Welcome.SetActive(false);
                    Notice_GameStart.SetActive(true);
                    break;

                case 2:
                    Notice_GameStart.SetActive(false);
                    CreateCSVfile = true;
                    Term_Notice_NewCondition = true;
                    Term_ExpStart = false;
                    break;
            }
        }
    }

    void BreakTime()
    {
        BreakTimer += Time.deltaTime;

        if (ST.Term_SliderTest)
        {
            if (BreakTimer < 60)
                Notice_ST_BreakStart.SetActive(true);
            else
            {
                Notice_ST_BreakStart.SetActive(false);
                ST.Term_SliderTest = false;
                RT.Term_RandomTest = true;
                CreateCSVfile = true;
                BreakTimer = 0;
                Term_BreakTime = false;
                Term_Notice_NewCondition = true;
            }
        }

        if (RT.Term_RandomTest)
        {
            if (BreakTimer < 60)
                Notice_RT_BreakStart.SetActive(true);
            else
            {
                Notice_RT_BreakStart.SetActive(false);
                RT.Term_RandomTest = false;
                ST.Term_SliderTest = true;
                CreateCSVfile = true;
                BreakTimer = 0;
                Term_BreakTime = false;
                Term_Notice_NewCondition = true;
            }
        }
    }

    void Notice_NewCondition()
    {
        ThresholdTimer += Time.deltaTime;

        if (ST.Term_SliderTest)
        {
            Notice_ST_Start.SetActive(true);

            if (Input.GetKeyDown(KeyCode.K) && ThresholdTimer > 2f)
            {
                Notice_ST_Start.SetActive(false);
                ST.Term_ProceedTask = true;
                Term_Notice_NewCondition = false;
                ThresholdTimer = 0;
            }
        }

        if (RT.Term_RandomTest)
        {
            Notice_RT_Start.SetActive(true);

            if (Input.GetKeyDown(KeyCode.K) && ThresholdTimer > 2f)
            {
                Notice_RT_Start.SetActive(false);
                RT.Term_ProceedTask = true;
                Term_Notice_NewCondition = false;
                ThresholdTimer = 0;
            }
        }
    }

    void ResetAtStart()
    {
        CreateCSVfile = false;
        ThresholdTimer = 0;
        SampleNumber = 0;
        BreakTimer = 0;
        Next = 0;
        Term_ExpStart = false;
        Term_Notice_NewCondition = false;
        Term_BreakTime = false;
        Term_SelectSampleNumber = true;
    }
}
