using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] CSV_Save_Raw CSV_R;
    public GameObject Notice_SelectSample, Notice_Welcome, Notice_GameStart;
    public GameObject Notice_ST_BreakStart, Notice_ST_Start;
    public GameObject Notice_RT_BreakStart, Notice_RT_Start;
    //public Text Text_SampleNumber;
    public TextMeshProUGUI Text_SampleNumber;
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

    void Update()
    {
        if (Term_SelectSampleNumber)
            SelectSampleNumber();

        if (Term_ExpStart)
            ExpStart();

        if (CreateCSVfile)
        {
            CSV_P.New_CSV_File();
            CSV_R.New_CSV_File();
            RT.ChangeCondition();
            CreateCSVfile = false;
        }

        if (Term_BreakTime)
            BreakTime();

        if (Term_Notice_NewCondition)
            Notice_NewCondition();
    }

    void SelectSampleNumber()
    {
        ThresholdTimer += Time.deltaTime;

        if (ThresholdTimer > 0.3f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SampleNumber++;
                ThresholdTimer = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && SampleNumber > 0)
            {
                SampleNumber--;
                ThresholdTimer = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Term_ExpStart = true;
                Term_SelectSampleNumber = false;
                Notice_SelectSample.SetActive(false);
                Notice_Welcome.SetActive(true);
                ThresholdTimer = 0;
            }
            Text_SampleNumber.text = SampleNumber.ToString();
        }
    }

    void ExpStart()
    {
        ThresholdTimer += Time.deltaTime;

        if (ThresholdTimer > 0.1f && Input.GetKeyDown(KeyCode.Alpha2))
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
                RT.ConditionCount++;
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

            if (Input.GetKeyDown(KeyCode.Alpha2) && ThresholdTimer > 1.5f)
            {
                Notice_ST_Start.SetActive(false);
                ST.Term_ST_ProceedTask = true;
                Term_Notice_NewCondition = false;
                ThresholdTimer = 0;
            }
        }

        if (RT.Term_RandomTest)
        {
            Notice_RT_Start.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Alpha2) && ThresholdTimer > 1.5f)
            {
                Notice_RT_Start.SetActive(false);
                RT.Term_RT_ProceedTask = true;
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
