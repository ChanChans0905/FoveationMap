using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ExpManager_RandomTest : MonoBehaviour
{
    [SerializeField] NoticeManager NM;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] CSV_Save_Processed CSV_P;
    [SerializeField] CSV_Save_Raw CSV_R;
    [SerializeField] PeripheralImageController PIC;
    [SerializeField] AdjustCameraFOV FOV;
    public int[] ConditionList = new int[4];
    public int[] ConditionOrder = new int[15];
    public int ImageOrder;
    float TaskTimer;
    public int PlayerAnswer;
    public int IsCorrect; // 사용자 선택이 정답인지 아닌지
    public GameObject Cinema_Origin, Cinema_Foveated, UI_Origin, UI_Foveated, Web_Origin, Web_Foveated, Game_Origin, Game_Foveated;
    bool Term_InputAnswer; // 사용자 입력 가능 시기
    public int TaskCount, ConditionCount;
    public Scenario CurrentScenario;
    public GameObject Notice_OpenFirstImage, Notice_OpenSecondImage; // 각 이미지 '1번입니다, 2번입니다' 안내
    public bool Term_RT_ProceedTask;
    public float AnsweringTimer;
    bool Term_AddAnsweringTimer;
    bool Term_ChangeImageOrder;
    public bool Term_RandomTest;
    public TextMeshProUGUI Num_C, Num_T, Num_R;

    void Start()
    {
        ResetAtStart();

        // 1. Cinema 2. UI 3. Web 4. Game
        ConditionList = new int[] { 0, 1, 2, 3 };
        // ShuffleArray(ConditionList);

        // fovea region size 5 x peripheral region resolution 3
        // 25,30,35,40,45 x 1k,2k,3k 
        ConditionOrder = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
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

        Num_C.text = ConditionList[ConditionCount].ToString();
        Num_T.text = TaskCount.ToString();
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

            ShuffleArray(ConditionOrder);
        }
    }

    public void ProceedTask()
    {
        TaskTimer += Time.deltaTime;

        if (TaskTimer < 2)
        {
            if (Term_ChangeImageOrder)
            {
                CSV_R.New_CSV_File();
                ImageOrder = UnityEngine.Random.Range(0, 2);
                PIC.RT_TurnOnPeripheralImage();
                FOV.RT_AdjustCameraFOV();
                Term_ChangeImageOrder = false;
            }

            if (TaskTimer < 1.5f)
                Notice_OpenFirstImage.SetActive(true);
            else
                Notice_OpenFirstImage.SetActive(false);
        }

        if (TaskTimer > 2 && TaskTimer < 8)
        {
            if (ImageOrder == 1)
                ShowTexture_Origin(CurrentScenario);
            else
                ShowTexture_Foveation(CurrentScenario);
        }

        if (TaskTimer > 9 && TaskTimer < 12)
            if (TaskTimer < 11)
                Notice_OpenSecondImage.SetActive(true);
            else
                Notice_OpenSecondImage.SetActive(false);

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
            PIC.TurnOffPeripheralImage();
            Term_RT_ProceedTask = false;
            Term_InputAnswer = true;
            Term_AddAnsweringTimer = true;
        }
    }

    void ShowTexture_Origin(Scenario CurrentScenario)
    {
        if (CurrentScenario == Scenario.Cinema)
            Cinema_Origin.SetActive(true);
        else if (CurrentScenario == Scenario.UI)
            UI_Origin.SetActive(true);
        else if (CurrentScenario == Scenario.Web)
            Web_Origin.SetActive(true);
        else if (CurrentScenario == Scenario.Game)
            Game_Origin.SetActive(true);
    }

    void ShowTexture_Foveation(Scenario CurrentScenario)
    {
        if (CurrentScenario == Scenario.Cinema)
            Cinema_Foveated.SetActive(true);
        else if (CurrentScenario == Scenario.UI)
            UI_Foveated.SetActive(true);
        else if (CurrentScenario == Scenario.Web)
            Web_Foveated.SetActive(true);
        else if (CurrentScenario == Scenario.Game)
            Game_Foveated.SetActive(true);
    }

    void TurnOffTexture()
    {
        Cinema_Origin.SetActive(false);
        Cinema_Foveated.SetActive(false);
        UI_Origin.SetActive(false);
        UI_Foveated.SetActive(false);
        Web_Origin.SetActive(false);
        Web_Foveated.SetActive(false);
        Game_Origin.SetActive(false);
        Game_Foveated.SetActive(false);
    }

    void GetUserAnswer()
    {
        if (Term_AddAnsweringTimer)
            AnsweringTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Term_AddAnsweringTimer = false;
            PlayerAnswer = 0;
            CheckAnswer(PlayerAnswer);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Term_AddAnsweringTimer = false;
            PlayerAnswer = 1;
            CheckAnswer(PlayerAnswer);
        }
    }

    void CheckAnswer(int PlayerAnswer)
    {
        if (PlayerAnswer == ImageOrder)
            IsCorrect = 1;
        else
            IsCorrect = 0;

        CSV_P.Save_CSV_Analysis();

        TaskCount++;
        TaskTimer = 0;
        AnsweringTimer = 0;
        Term_InputAnswer = false;
        Term_ChangeImageOrder = true;

        if (TaskCount != 14)
            Term_RT_ProceedTask = true;
        else
            BlockEnd_RandomTest();
    }

    void BlockEnd_RandomTest()
    {
        // ST.SetSliderTestCondition();
        NM.Term_BreakTime = true;
        ResetAtStart();
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
        PlayerAnswer = 0;
        IsCorrect = 0;
        Term_InputAnswer = false;
        TaskCount = 0;
        ConditionCount = 0;
        Term_RT_ProceedTask = false;
        AnsweringTimer = 0;
        Term_AddAnsweringTimer = false;
        Term_ChangeImageOrder = false;
        Term_RandomTest = true;
    }

    public enum Scenario
    {
        Cinema,
        UI,
        Web,
        Game
    }
}
