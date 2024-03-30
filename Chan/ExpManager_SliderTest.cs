using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpManager_SliderTest : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] NoticeManager NM;
    [SerializeField] GetUserPositionToShader User;
    public int[] ConditionList = new int[4];
    public List<float> LastFiveAnswers = new List<float>();
    public int ImageOrder;
    float TaskTimer;
    public int ReverseCount;
    public int PlayerAnswer;
    public int OriginNum; // Task 당시의 원본 이미지 번호
    public int IsCorrect; // 사용자 선택이 정답인지 아닌지
    public int PreviousAnswer; // 직전 task 정답 여부
    public bool DecreaseFoveaRegionSize; // True : 내림, False : 올림
    public GameObject Cinema_O, Cinema_F, UIsetting_O, UIsetting_F, Web_O, Web_F, Game_O, Game_F;
    bool Term_InputAnswer; // 사용자 입력 가능 시기
    public int TaskCount, ConditionCount;
    public Scenario CurrentScenario;
    public GameObject Text_Texture1, Text_Texture2; // 각 이미지 '1번입니다, 2번입니다' 안내
    public bool Term_ProceedTask;
    public float AnsweringTimer;
    bool AddAnsweringTimer;
    bool Trigger_ChangeImageOrder;
    public int ReversalAdded;
    public bool Term_SliderTest;

    void Start()
    {
        ResetAtStart();

        ConditionList = new int[] { 1, 2, 3, 4 };
        ShuffleArray(ConditionList);
    }

    void FixedUpdate()
    {
        if (Term_SliderTest)
        {
            if (Term_ProceedTask)
                ProceedTask();

            if (Term_InputAnswer)
                GetAnswer();
        }
    }

    public void ChangeCondition()
    {
        if (ConditionCount < 4)
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
        }
    }

    public void ProceedTask()
    {
        TaskTimer += Time.deltaTime;

        if (Trigger_ChangeImageOrder)
        {
            ImageOrder = UnityEngine.Random.Range(0, 2);
            Trigger_ChangeImageOrder = false;
        }

        if (TaskTimer < 3)
            if (TaskTimer < 2)
                Text_Texture1.SetActive(true);
            else
                Text_Texture1.SetActive(false);

        if (TaskTimer > 3 && TaskTimer < 8)
        {
            if (ImageOrder == 1)
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
            if (ImageOrder == 1)
                ShowTexture_Foveation(CurrentScenario);
            else
                ShowTexture_Origin(CurrentScenario);
        }

        if ((TaskTimer > 8 && TaskTimer < 9) || (TaskTimer > 17 && TaskTimer < 18))
            TurnOffTexture();

        if (TaskTimer > 17)
        {
            Term_InputAnswer = true;
            Term_ProceedTask = false;
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
        if (AddAnsweringTimer)
            AnsweringTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
        {
            AddAnsweringTimer = false;
            PlayerAnswer = 1;
            CheckAnswer(PlayerAnswer);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AddAnsweringTimer = false;
            PlayerAnswer = 2;
            CheckAnswer(PlayerAnswer);
        }
    }

    void CheckAnswer(int PlayerAnswer)
    {
        if (PlayerAnswer == OriginNum)
        {
            IsCorrect = 1;
            DecreaseFoveaRegionSize = true;
        }
        else
        {
            IsCorrect = 0;
            DecreaseFoveaRegionSize = false;
        }

        CheckReverseCount();
        CSV_P.Save_CSV_Analysis();

        if (ReverseCount >= 8)
        {
            LastFiveAnswers[TaskCount - 8] = User.CameraFOV;
        }

        TaskCount++;
        TaskTimer = 0;
        AddAnsweringTimer = true;
        AnsweringTimer = 0;
        AdjustFoveation(DecreaseFoveaRegionSize);
        Term_InputAnswer = false;
        ReversalAdded = 0;
        Trigger_ChangeImageOrder = true;

        if (ReverseCount != 12)
            Term_ProceedTask = true;
        else
            BlockEnd_SliderTest();
    }

    void CheckReverseCount()
    {
        if (PreviousAnswer != IsCorrect)
        {
            ReversalAdded = 1;
            ReverseCount++;
            PreviousAnswer = IsCorrect;
        }
    }

    void AdjustFoveation(bool FoveationUpOrDown)
    {
        if (FoveationUpOrDown)
            User.CameraFOV -= 5;
        else
            User.CameraFOV += 5;

        if (LastFiveAnswers.Count == 5)
            LastFiveAnswers.RemoveAt(0);

        LastFiveAnswers.Add(User.CameraFOV);

        User.Term_AdjustFoveation = true;
    }

    void BlockEnd_SliderTest()
    {
        RT.SetRandomTestCondition();
        NM.Term_BreakTime = true;
        ResetValue();
    }


    void ResetValue()
    {
        ReverseCount = 0;
        TaskTimer = 0;
        Term_InputAnswer = false;
        Term_ProceedTask = false;
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

    void ResetAtStart()
    {
        ImageOrder = 0;
        TaskTimer = 0;
        ReverseCount = 0;
        PlayerAnswer = 0;
        OriginNum = 0;
        IsCorrect = 0;
        PreviousAnswer = 0;
        DecreaseFoveaRegionSize = false;
        Term_InputAnswer = false;
        TaskCount = 0;
        ConditionCount = 0;
        Term_ProceedTask = false;
        AnsweringTimer = 0;
        AddAnsweringTimer = false;
        Trigger_ChangeImageOrder = false;
        ReversalAdded = 0;
        Term_SliderTest = true;
    }

    public enum Scenario
    {
        Cinema,
        UIsetting,
        Web,
        Game
    }
}
