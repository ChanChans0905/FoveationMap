using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Video;

public class ExpManager_RandomTest : MonoBehaviour
{
    [SerializeField] NoticeManager NM;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] CSV_Save_Processed_RT CSV_P_RT;
    [SerializeField] CSV_Save_Raw CSV_R;
    [SerializeField] PeripheralImageController PIC;
    public GameObject Block;
    public int[] ConditionList = new int[4];
    public int[] ConditionOrder = new int[15];
    public int[] FovOrder = new int[4];
    public int[] FovOrder_HomeUI = new int[4];
    public int ImageOrder;
    public float TaskTimer;
    public int PlayerAnswer;
    public int IsCorrect; // 사용자 선택이 정답인지 아닌지
    public GameObject Cinema_Origin, Cinema_Foveated, UI_Origin, UI_Foveated, Web_Origin, Web_Foveated, Game_Origin, Game_Foveated;
    bool Term_InputAnswer; // 사용자 입력 가능 시기
    public int TaskCount, ConditionCount, RepetitionCount;
    public Scenario CurrentScenario;
    public GameObject Notice_OpenFirstImage, Notice_OpenSecondImage; // 각 이미지 '1번입니다, 2번입니다' 안내
    public bool Term_RT_ProceedTask;
    public float AnsweringTimer;
    bool Term_AddAnsweringTimer;
    bool Term_ChangeImageOrder;
    public bool Term_RandomTest;
    public bool BlockEnd_RandomTest;
    public bool IsRestTime;
    public int FovCount;

    void Start()
    {
        ResetAtStart();

        // 0. Cinema 1. UI 2. Web 3. Game
        ConditionList = new int[] { 0, 1, 2, 3 };
        //ShuffleArray(ConditionList);

        // peripheral image resolution
        ConditionOrder = new int[] { 0, 1, 2, 3 };
        ShuffleArray(ConditionOrder);

        // FOV size
        FovOrder = new int[] { 20, 30, 40, 50 };
        ShuffleArray(FovOrder);
        FovOrder_HomeUI = new int[] { 30, 40, 50, 60 };
        ShuffleArray(FovOrder_HomeUI);

        ChangeCondition();
    }

    void Update()
    {
        if (Term_RandomTest)
        {
            if (Term_RT_ProceedTask)
                ProceedTask();

            if (Term_InputAnswer)
                GetUserAnswer();
        }
    }

    void FixedUpdate()
    {
        if (Term_RT_ProceedTask)
            TaskTimer += Time.deltaTime;

        if (Term_AddAnsweringTimer)
            AnsweringTimer += Time.deltaTime;
    }

    public void ChangeCondition()
    {
        if (ConditionCount < 4)
        {
            if (ConditionList[ConditionCount] == 0)
                CurrentScenario = Scenario.Cinema;
            else if (ConditionList[ConditionCount] == 1)
                CurrentScenario = Scenario.UI;
            else if (ConditionList[ConditionCount] == 2)
                CurrentScenario = Scenario.Web;
            else if (ConditionList[ConditionCount] == 3)
                CurrentScenario = Scenario.Game;
        }
    }

    public void ProceedTask()
    {
        // 이미지 제공 시간 : 9초
        // 쉬는 시간 : 2초

        if (TaskTimer < 1.5f)
        {
            if (Term_ChangeImageOrder)
            {
                Block.SetActive(true);
                CSV_R.New_CSV_File();
                ImageOrder = UnityEngine.Random.Range(0, 2);
                PIC.RT_TurnOnPI();
                User.RT_AdjustCameraFOV();
                Term_ChangeImageOrder = false;
            }

            if (TaskTimer < 1f)
                Notice_OpenFirstImage.SetActive(true);
            else
            {
                MoveObject(ImageOrder == 1);
                Notice_OpenFirstImage.SetActive(false);
                Block.SetActive(false);
            }
        }

        if (TaskTimer > 1.5f && TaskTimer < 10.5f)
            TurnOnOffTexture(ImageOrder == 1, true);

        if (TaskTimer > 10.5f && TaskTimer < 12f)
        {
            TurnOnOffTexture(ImageOrder == 1, false);
            Block.SetActive(true);
            if (TaskTimer < 11.7f)
                Notice_OpenSecondImage.SetActive(true);
            else
            {
                MoveObject(ImageOrder != 1);
                Notice_OpenSecondImage.SetActive(false);
                Block.SetActive(false);
            }
        }

        if (TaskTimer > 12f && TaskTimer < 21f)
            TurnOnOffTexture(ImageOrder != 1, true);

        if (TaskTimer > 21f && TaskTimer < 22f)
        {
            TurnOnOffTexture(ImageOrder != 1, false);
            Block.SetActive(true);
        }

        if (TaskTimer > 22f)
        {
            Term_RT_ProceedTask = false;
            Term_InputAnswer = true;
            Term_AddAnsweringTimer = true;
            TaskTimer = 0;
        }
    }

    void TurnOnOffTexture(bool isOrigin, bool turnOnOff)
    {
        GameObject origin = null, foveated = null;

        switch (CurrentScenario)
        {
            case Scenario.Cinema:
                origin = Cinema_Origin;
                foveated = Cinema_Foveated;
                break;
            case Scenario.UI:
                origin = UI_Origin;
                foveated = UI_Foveated;
                break;
            case Scenario.Web:
                origin = Web_Origin;
                foveated = Web_Foveated;
                break;
            case Scenario.Game:
                origin = Game_Origin;
                foveated = Game_Foveated;
                break;
        }

        if (isOrigin)
        {
            WairFor1Second();
            origin.SetActive(turnOnOff);
        }
        else
        {
            if (turnOnOff)
                PIC.PlayVideo();
            else
                PIC.RT_TurnOffPI();
        }

        if (BlockEnd_RandomTest)
        {
            origin.SetActive(false);
            foveated.SetActive(false);
        }
    }

    IEnumerator WairFor1Second()
    {
        yield return new WaitForSeconds(1f);
    }

    void MoveObject(bool isFirst)
    {
        GameObject firstObj = null, secondObj = null;

        switch (CurrentScenario)
        {
            case Scenario.Cinema:
                firstObj = Cinema_Origin;
                secondObj = Cinema_Foveated;
                break;
            case Scenario.UI:
                firstObj = UI_Origin;
                secondObj = UI_Foveated;
                break;
            case Scenario.Web:
                firstObj = Web_Origin;
                secondObj = Web_Foveated;
                break;
            case Scenario.Game:
                firstObj = Game_Origin;
                secondObj = Game_Foveated;
                break;
        }

        if (isFirst)
        {
            firstObj.transform.localPosition = new Vector3(0, 0, 0f);
            secondObj.transform.localPosition = new Vector3(0, 0, 0.1f);
        }
        else
        {
            firstObj.transform.localPosition = new Vector3(0, 0, 0.1f);
            secondObj.transform.localPosition = new Vector3(0, 0, 0f);
        }
    }

    void GetUserAnswer()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Term_AddAnsweringTimer = false;
            PlayerAnswer = 0;
            CheckAnswerThenEndTask(PlayerAnswer);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Term_AddAnsweringTimer = false;
            PlayerAnswer = 1;
            CheckAnswerThenEndTask(PlayerAnswer);
        }
    }

    void CheckAnswerThenEndTask(int PlayerAnswer)
    {
        if (PlayerAnswer == ImageOrder)
            IsCorrect = 1;
        else
            IsCorrect = 0;

        CSV_P_RT.Save_CSV_Processed();
        TaskCount++;
        ResetAfterEachTask();

        if (TaskCount != 4)
            Term_RT_ProceedTask = true;
        else
        {
            RepetitionCount++;
            TaskCount = 0;

            ShuffleArray(ConditionOrder);

            if (RepetitionCount != 3)
                Term_RT_ProceedTask = true;
            else
            {
                NM.Term_BreakTime = true;
                RepetitionCount = 0;
                FovCount++;

                if (FovCount == 4)
                {
                    FovCount = 0;

                    ShuffleArray(FovOrder);
                    ShuffleArray(FovOrder_HomeUI);
                    BlockEnd_RandomTest = true;
                    TurnOnOffTexture(false, false);
                }
            }
        }
    }

    public void ShuffleArray(int[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    void ResetAfterEachTask()
    {
        AnsweringTimer = 0;
        Term_InputAnswer = false;
        Term_ChangeImageOrder = true;
        Term_AddAnsweringTimer = false;
        User.OutOfScreenTimer = 0;
    }


    void ResetAtStart()
    {
        ImageOrder = 0;
        PlayerAnswer = 0;
        IsCorrect = 0;
        Term_InputAnswer = false;
        TaskCount = 0;
        Term_RT_ProceedTask = false;
        AnsweringTimer = 0;
        Term_AddAnsweringTimer = false;
        Term_ChangeImageOrder = true;
        Term_RandomTest = true;
        RepetitionCount = 0;
        ConditionCount = 0;
    }

    public enum Scenario
    {
        Cinema,
        UI,
        Web,
        Game
    }
}
