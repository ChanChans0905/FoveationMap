using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PeripheralImageController : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] GameObject PeripheralImage_Cinema_1K, PeripheralImage_Cinema_2K, PeripheralImage_Cinema_3K;
    [SerializeField] GameObject PeripheralImage_UI_1K, PeripheralImage_UI_2K, PeripheralImage_UI_3K;
    [SerializeField] GameObject PeripheralImage_Web_1K, PeripheralImage_Web_2K, PeripheralImage_Web_3K;
    [SerializeField] GameObject PeripheralImage_Game_1K, PeripheralImage_Game_2K, PeripheralImage_Game_3K;

    public void RT_TurnOnPeripheralImage()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                if (RT.ConditionOrder[RT.TaskCount] == 0 || RT.ConditionOrder[RT.TaskCount] == 3 || RT.ConditionOrder[RT.TaskCount] == 6 || RT.ConditionOrder[RT.TaskCount] == 9 || RT.ConditionOrder[RT.TaskCount] == 12)
                    PeripheralImage_Cinema_1K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 1 || RT.ConditionOrder[RT.TaskCount] == 4 || RT.ConditionOrder[RT.TaskCount] == 7 || RT.ConditionOrder[RT.TaskCount] == 10 || RT.ConditionOrder[RT.TaskCount] == 13)
                    PeripheralImage_Cinema_2K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 2 || RT.ConditionOrder[RT.TaskCount] == 5 || RT.ConditionOrder[RT.TaskCount] == 8 || RT.ConditionOrder[RT.TaskCount] == 11 || RT.ConditionOrder[RT.TaskCount] == 14)
                    PeripheralImage_Cinema_3K.SetActive(true);
                break;
            case 1:
                if (RT.ConditionOrder[RT.TaskCount] == 0 || RT.ConditionOrder[RT.TaskCount] == 3 || RT.ConditionOrder[RT.TaskCount] == 6 || RT.ConditionOrder[RT.TaskCount] == 9 || RT.ConditionOrder[RT.TaskCount] == 12)
                    PeripheralImage_UI_1K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 1 || RT.ConditionOrder[RT.TaskCount] == 4 || RT.ConditionOrder[RT.TaskCount] == 7 || RT.ConditionOrder[RT.TaskCount] == 10 || RT.ConditionOrder[RT.TaskCount] == 13)
                    PeripheralImage_UI_2K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 2 || RT.ConditionOrder[RT.TaskCount] == 5 || RT.ConditionOrder[RT.TaskCount] == 8 || RT.ConditionOrder[RT.TaskCount] == 11 || RT.ConditionOrder[RT.TaskCount] == 14)
                    PeripheralImage_UI_3K.SetActive(true);
                break;
            case 2:
                if (RT.ConditionOrder[RT.TaskCount] == 0 || RT.ConditionOrder[RT.TaskCount] == 3 || RT.ConditionOrder[RT.TaskCount] == 6 || RT.ConditionOrder[RT.TaskCount] == 9 || RT.ConditionOrder[RT.TaskCount] == 12)
                    PeripheralImage_Web_1K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 1 || RT.ConditionOrder[RT.TaskCount] == 4 || RT.ConditionOrder[RT.TaskCount] == 7 || RT.ConditionOrder[RT.TaskCount] == 10 || RT.ConditionOrder[RT.TaskCount] == 13)
                    PeripheralImage_Web_2K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 2 || RT.ConditionOrder[RT.TaskCount] == 5 || RT.ConditionOrder[RT.TaskCount] == 8 || RT.ConditionOrder[RT.TaskCount] == 11 || RT.ConditionOrder[RT.TaskCount] == 14)
                    PeripheralImage_Web_3K.SetActive(true);
                break;
            case 3:
                if (RT.ConditionOrder[RT.TaskCount] == 0 || RT.ConditionOrder[RT.TaskCount] == 3 || RT.ConditionOrder[RT.TaskCount] == 6 || RT.ConditionOrder[RT.TaskCount] == 9 || RT.ConditionOrder[RT.TaskCount] == 12)
                    PeripheralImage_Game_1K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 1 || RT.ConditionOrder[RT.TaskCount] == 4 || RT.ConditionOrder[RT.TaskCount] == 7 || RT.ConditionOrder[RT.TaskCount] == 10 || RT.ConditionOrder[RT.TaskCount] == 13)
                    PeripheralImage_Game_2K.SetActive(true);
                else if (RT.ConditionOrder[RT.TaskCount] == 2 || RT.ConditionOrder[RT.TaskCount] == 5 || RT.ConditionOrder[RT.TaskCount] == 8 || RT.ConditionOrder[RT.TaskCount] == 11 || RT.ConditionOrder[RT.TaskCount] == 14)
                    PeripheralImage_Game_3K.SetActive(true);
                break;
        }
    }

    public void TurnOffPeripheralImage()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                PeripheralImage_Cinema_1K.SetActive(false);
                PeripheralImage_Cinema_2K.SetActive(false);
                PeripheralImage_Cinema_3K.SetActive(false);
                break;
            case 1:
                PeripheralImage_UI_1K.SetActive(false);
                PeripheralImage_UI_2K.SetActive(false);
                PeripheralImage_UI_3K.SetActive(false);
                break;
            case 2:
                PeripheralImage_Web_1K.SetActive(false);
                PeripheralImage_Web_2K.SetActive(false);
                PeripheralImage_Web_3K.SetActive(false);
                break;
            case 3:
                PeripheralImage_Game_1K.SetActive(false);
                PeripheralImage_Game_2K.SetActive(false);
                PeripheralImage_Game_3K.SetActive(false);
                break;
        }
    }

    public void ST_TurnOnPeripheralImage()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                if (ST.SliderCount == 0 || ST.SliderCount == 3 || ST.SliderCount == 6 || ST.SliderCount == 9 || ST.SliderCount == 12)
                    PeripheralImage_Cinema_3K.SetActive(true);
                else if (ST.SliderCount == 1 || ST.SliderCount == 4 || ST.SliderCount == 7 || ST.SliderCount == 10 || ST.SliderCount == 13)
                    PeripheralImage_Cinema_2K.SetActive(true);
                else if (ST.SliderCount == 2 || ST.SliderCount == 5 || ST.SliderCount == 8 || ST.SliderCount == 11 || ST.SliderCount == 14)
                    PeripheralImage_Cinema_1K.SetActive(true);
                break;
            case 1:
                if (ST.SliderCount == 0 || ST.SliderCount == 3 || ST.SliderCount == 6 || ST.SliderCount == 9 || ST.SliderCount == 12)
                    PeripheralImage_UI_3K.SetActive(true);
                else if (ST.SliderCount == 1 || ST.SliderCount == 4 || ST.SliderCount == 7 || ST.SliderCount == 10 || ST.SliderCount == 13)
                    PeripheralImage_UI_2K.SetActive(true);
                else if (ST.SliderCount == 2 || ST.SliderCount == 5 || ST.SliderCount == 8 || ST.SliderCount == 11 || ST.SliderCount == 14)
                    PeripheralImage_UI_1K.SetActive(true);
                break;
            case 2:
                if (ST.SliderCount == 0 || ST.SliderCount == 3 || ST.SliderCount == 6 || ST.SliderCount == 9 || ST.SliderCount == 12)
                    PeripheralImage_Web_3K.SetActive(true);
                else if (ST.SliderCount == 1 || ST.SliderCount == 4 || ST.SliderCount == 7 || ST.SliderCount == 10 || ST.SliderCount == 13)
                    PeripheralImage_Web_2K.SetActive(true);
                else if (ST.SliderCount == 2 || ST.SliderCount == 5 || ST.SliderCount == 8 || ST.SliderCount == 11 || ST.SliderCount == 14)
                    PeripheralImage_Web_1K.SetActive(true);
                break;
            case 3:
                if (ST.SliderCount == 0 || ST.SliderCount == 3 || ST.SliderCount == 6 || ST.SliderCount == 9 || ST.SliderCount == 12)
                    PeripheralImage_Game_3K.SetActive(true);
                else if (ST.SliderCount == 1 || ST.SliderCount == 4 || ST.SliderCount == 7 || ST.SliderCount == 10 || ST.SliderCount == 13)
                    PeripheralImage_Game_2K.SetActive(true);
                else if (ST.SliderCount == 2 || ST.SliderCount == 5 || ST.SliderCount == 8 || ST.SliderCount == 11 || ST.SliderCount == 14)
                    PeripheralImage_Game_1K.SetActive(true);
                break;
        }
    }
}
