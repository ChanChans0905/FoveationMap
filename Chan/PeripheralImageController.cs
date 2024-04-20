using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PeripheralImageController : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] GameObject PIP_Cinema, PIP_UI, PIP_Web, PIP_Game;
    [SerializeField] GameObject PeripheralImage_Cinema_1K, PeripheralImage_Cinema_2K, PeripheralImage_Cinema_3K;
    [SerializeField] GameObject PeripheralImage_UI_1K, PeripheralImage_UI_2K, PeripheralImage_UI_3K;
    [SerializeField] GameObject PeripheralImage_Web_1K, PeripheralImage_Web_2K, PeripheralImage_Web_3K;
    [SerializeField] GameObject PeripheralImage_Game_1K, PeripheralImage_Game_2K, PeripheralImage_Game_3K;

    public void RT_TurnOnPeripheralImage()
    {
        int TC = RT.ConditionOrder[RT.TaskCount];

        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                if (TC == 0 || TC == 3 || TC == 6 || TC == 9 || TC == 12)
                    PeripheralImage_Cinema_1K.SetActive(true);
                else if (TC == 1 || TC == 4 || TC == 7 || TC == 10 || TC == 13)
                    PeripheralImage_Cinema_2K.SetActive(true);
                else if (TC == 2 || TC == 5 || TC == 8 || TC == 11 || TC == 14)
                    PeripheralImage_Cinema_3K.SetActive(true);
                break;
            case 1:
                if (TC == 0 || TC == 3 || TC == 6 || TC == 9 || TC == 12)
                    PeripheralImage_UI_1K.SetActive(true);
                else if (TC == 1 || TC == 4 || TC == 7 || TC == 10 || TC == 13)
                    PeripheralImage_UI_2K.SetActive(true);
                else if (TC == 2 || TC == 5 || TC == 8 || TC == 11 || TC == 14)
                    PeripheralImage_UI_3K.SetActive(true);
                break;
            case 2:
                if (TC == 0 || TC == 3 || TC == 6 || TC == 9 || TC == 12)
                    PeripheralImage_Web_1K.SetActive(true);
                else if (TC == 1 || TC == 4 || TC == 7 || TC == 10 || TC == 13)
                    PeripheralImage_Web_2K.SetActive(true);
                else if (TC == 2 || TC == 5 || TC == 8 || TC == 11 || TC == 14)
                    PeripheralImage_Web_3K.SetActive(true);
                break;
            case 3:
                if (TC == 0 || TC == 3 || TC == 6 || TC == 9 || TC == 12)
                    PeripheralImage_Game_1K.SetActive(true);
                else if (TC == 1 || TC == 4 || TC == 7 || TC == 10 || TC == 13)
                    PeripheralImage_Game_2K.SetActive(true);
                else if (TC == 2 || TC == 5 || TC == 8 || TC == 11 || TC == 14)
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
        int SC = ST.SliderCount;

        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                if (SC == 0 || SC == 3 || SC == 6 || SC == 9 || SC == 12)
                    PeripheralImage_Cinema_3K.SetActive(true);
                else if (SC == 1 || SC == 4 || SC == 7 || SC == 10 || SC == 13)
                    PeripheralImage_Cinema_2K.SetActive(true);
                else if (SC == 2 || SC == 5 || SC == 8 || SC == 11 || SC == 14)
                    PeripheralImage_Cinema_1K.SetActive(true);
                PIP_Cinema.SetActive(true);
                break;
            case 1:
                if (SC == 0 || SC == 3 || SC == 6 || SC == 9 || SC == 12)
                    PeripheralImage_UI_3K.SetActive(true);
                else if (SC == 1 || SC == 4 || SC == 7 || SC == 10 || SC == 13)
                    PeripheralImage_UI_2K.SetActive(true);
                else if (SC == 2 || SC == 5 || SC == 8 || SC == 11 || SC == 14)
                    PeripheralImage_UI_1K.SetActive(true);
                PIP_UI.SetActive(true);
                break;
            case 2:
                if (SC == 0 || SC == 3 || SC == 6 || SC == 9 || SC == 12)
                    PeripheralImage_Web_3K.SetActive(true);
                else if (SC == 1 || SC == 4 || SC == 7 || SC == 10 || SC == 13)
                    PeripheralImage_Web_2K.SetActive(true);
                else if (SC == 2 || SC == 5 || SC == 8 || SC == 11 || SC == 14)
                    PeripheralImage_Web_1K.SetActive(true);
                PIP_Web.SetActive(true);
                break;
            case 3:
                if (SC == 0 || SC == 3 || SC == 6 || SC == 9 || SC == 12)
                    PeripheralImage_Game_3K.SetActive(true);
                else if (SC == 1 || SC == 4 || SC == 7 || SC == 10 || SC == 13)
                    PeripheralImage_Game_2K.SetActive(true);
                else if (SC == 2 || SC == 5 || SC == 8 || SC == 11 || SC == 14)
                    PeripheralImage_Game_1K.SetActive(true);
                PIP_Game.SetActive(true);
                break;
        }
    }

    public void ST_TurnOnPI_AtStart()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                PIP_Cinema.SetActive(true);
                PeripheralImage_Cinema_3K.SetActive(true);
                break;
            case 1:
                PIP_UI.SetActive(true);
                PeripheralImage_UI_3K.SetActive(true);
                break;
            case 2:
                PIP_Web.SetActive(true);
                PeripheralImage_Web_3K.SetActive(true);
                break;
            case 3:
                PIP_Game.SetActive(true);
                PeripheralImage_Game_3K.SetActive(true);
                break;
        }
    }

    public void ST_TurnOffPeripheralImageParent()
    {
        PIP_Cinema.SetActive(false);
        PIP_UI.SetActive(false);
        PIP_Web.SetActive(false);
        PIP_Game.SetActive(false);
    }
}
