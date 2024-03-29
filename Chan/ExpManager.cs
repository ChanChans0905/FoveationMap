using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    [SerializeField] CSV_Save_Processed CSV_P;
    public int[] ConditionList = new int[4];
    public int[] TaskList = new int[30];
    float TaskTimer;
    public int ReverseCount;
    public float FoveationLevel; // 1,2,4,8,16
    public int PlayerAnswer;
    public int OriginNum; // Task 당시의 원본 이미지 번호
    public bool IsCorrect; // 사용자 선택이 정답인지 아닌지
    public bool PreviousAnswer; // 직전 task 정답 여부
    public bool DecreaseFoveation; // True : 내림, False : 올림
    public GameObject Cinema_O, Cinema_F, UIsetting_O, UIsetting_F, Web_O, Web_F, Game_O, Game_F;
    public GameObject Notice_ExpEnd;
    bool Term_InputAnswer; // 사용자 입력 가능 시기
    int TaskCount, ConditionCount;
    public Scenario CurrentScenario;
    public GameObject Text_Texture1, Text_Texture2; // 각 이미지 '1번입니다, 2번입니다' 안내
    public bool Trigger_ProceedTask, Trigger_AdjustFoveation;
    public int SampleNumber;

    void Start()
    {
        // 시나리오 순서 랜덤 배정
        ConditionList = new int[] { 1, 2, 3, 4 };
        ShuffleArray(ConditionList);
    }

    void FixedUpdate()
    {

        if (Trigger_ProceedTask)
            ProceedTask();

        if (Term_InputAnswer)
            GetAnswer();
    }

    public void ChangeCondition()
    {
        if (ConditionList[ConditionCount] == 1)
            CurrentScenario = Scenario.Cinema;
        else if (ConditionList[ConditionCount] == 2)
            CurrentScenario = Scenario.UIsetting;
        else if (ConditionList[ConditionCount] == 3)
            CurrentScenario = Scenario.Web;
        else if (ConditionList[ConditionCount] == 4)
            CurrentScenario = Scenario.Game;

        ConditionCount++;

        if (ConditionCount == 5)
            ExpEnd();

        Trigger_ProceedTask = true;
    }

    public void ProceedTask()
    {
        TaskTimer += Time.deltaTime;

        if (TaskTimer < 3)
            if (TaskTimer < 2)
                Text_Texture1.SetActive(true);
            else
                Text_Texture1.SetActive(false);

        if (TaskTimer > 3 && TaskTimer < 8)
        {
            if (TaskList[TaskCount] == 1)
                ShowTexture_Origin(CurrentScenario);
            else
                ShowTexture_Foveation(CurrentScenario);
        }

        if (TaskTimer > 9 && TaskTimer < 12)
            if (TaskTimer < 11)
                Text_Texture2.SetActive(true);
            else
                Text_Texture2.SetActive(false);

        if (TaskTimer > 12 && TaskTimer < 17)
        {
            if (TaskList[TaskCount] == 1)
                ShowTexture_Foveation(CurrentScenario);
            else
                ShowTexture_Origin(CurrentScenario);
        }

        if ((TaskTimer > 8 && TaskTimer < 9) || (TaskTimer > 17 && TaskTimer < 18))
            TurnOffTexture();

        if (TaskTimer > 17)
        {
            Term_InputAnswer = true;
            Trigger_ProceedTask = false;
        }
    }

    void ShowTexture_Origin(Scenario CurrentScenario)
    {
        if (CurrentScenario == Scenario.Cinema)
            Cinema_O.SetActive(true);
        else if (CurrentScenario == Scenario.UIsetting)
            UIsetting_O.SetActive(true);
        else if (CurrentScenario == Scenario.Web)
            Web_O.SetActive(true);
        else if (CurrentScenario == Scenario.Game)
            Game_O.SetActive(true);
    }

    void ShowTexture_Foveation(Scenario CurrentScenario)
    {
        if (CurrentScenario == Scenario.Cinema)
            Cinema_F.SetActive(true);
        else if (CurrentScenario == Scenario.UIsetting)
            UIsetting_F.SetActive(true);
        else if (CurrentScenario == Scenario.Web)
            Web_F.SetActive(true);
        else if (CurrentScenario == Scenario.Game)
            Game_F.SetActive(true);
    }

    void TurnOffTexture()
    {
        Cinema_O.SetActive(false);
        Cinema_F.SetActive(false);
        UIsetting_O.SetActive(false);
        UIsetting_F.SetActive(false);
        Web_O.SetActive(false);
        Web_F.SetActive(false);
        Game_O.SetActive(false);
        Game_F.SetActive(false);
    }

    void GetAnswer()
    {
        GetAnswer();
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerAnswer = 1;
            CheckAnswer(PlayerAnswer);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerAnswer = 2;
            CheckAnswer(PlayerAnswer);
        }
    }

    void CheckAnswer(int PlayerAnswer)
    {
        if (PlayerAnswer == OriginNum)
        {
            IsCorrect = true;
            DecreaseFoveation = true;
        }
        else
        {
            IsCorrect = false;
            DecreaseFoveation = false;
        }

        CSV_P.Save_CSV_Analysis();
        TaskCount++;
        TaskTimer = 0;
        IncreaseReverseCount();
        AdjustFoveation(DecreaseFoveation);
        Trigger_AdjustFoveation = true;
        Term_InputAnswer = false;
        if (ReverseCount != 12)
            Trigger_ProceedTask = true;
        else
            BlockEnd();
    }

    void IncreaseReverseCount()
    {
        if (PreviousAnswer != IsCorrect)
        {
            ReverseCount++;
            PreviousAnswer = IsCorrect;
        }
    }

    void AdjustFoveation(bool FoveationUpOrDown)
    {
        if (FoveationUpOrDown)
            FoveationLevel /= 2;
        else
            FoveationLevel *= 2;
    }

    void BlockEnd()
    {
        ResetValue();
        ChangeCondition();
    }

    void ExpEnd()
    {
        ResetValue();
        Notice_ExpEnd.SetActive(true);
    }

    void ResetValue()
    {
        ReverseCount = 0;
        FoveationLevel = 0;
        TaskTimer = 0;
        Term_InputAnswer = false;
        Trigger_ProceedTask = false;
        Trigger_AdjustFoveation = false;
    }

    void ShuffleArray(int[] array)
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

    public enum Scenario
    {
        Cinema,
        UIsetting,
        Web,
        Game
    }
}
