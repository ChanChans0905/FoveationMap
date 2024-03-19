using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] ExpManager ExpManager;
    public GameObject Notice_SelectSample, Notice_Welcome, Notice_GameStart;
    public GameObject Notice_NewCondition;
    public Text Text_SampleNumber;
    bool CreateCSVfile;
    float ThresholdTimer;
    int SampleNumber;
    int Next;
    bool Term_SelectSampleNumber, Term_ExpStart, Term_NewCondition;

    void Start()
    {
        Term_SelectSampleNumber = true;
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
            ExpManager.Trigger_ApplyArray = true;
            CreateCSVfile = false;
        }

        if (Term_NewCondition)
            NewCondition();
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
            ExpManager.SampleNumber = SampleNumber;
            Term_ExpStart = true;
            Term_SelectSampleNumber = false;
            Notice_SelectSample.SetActive(false);
            Notice_Welcome.SetActive(false);
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
                    Term_ExpStart = false;
                    break;
            }
        }
    }

    void NewCondition()
    {
        Notice_NewCondition.SetActive(true);

        if (Input.GetKeyDown(KeyCode.K) && ThresholdTimer > 1f )
        {
            Notice_NewCondition.SetActive(false);
        }
        Term_NewCondition = false;
    }
}
