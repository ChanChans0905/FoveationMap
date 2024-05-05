using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PeripheralImageController : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] GameObject RT_PIP_Cinema, RT_PIP_UI, RT_PIP_Web, RT_PIP_Game;
    [SerializeField] GameObject RT_PI_Cinema1, RT_PI_Cinema2, RT_PI_Cinema3, RT_PI_Cinema4;
    [SerializeField] GameObject RT_PI_UI1, RT_PI_UI2, RT_PI_UI3, RT_PI_UI4;
    [SerializeField] GameObject RT_PI_Web1, RT_PI_Web2, RT_PI_Web3, RT_PI_Web4;
    [SerializeField] GameObject RT_PI_Game1, RT_PI_Game2, RT_PI_Game3, RT_PI_Game4;
    [SerializeField] GameObject ST_PIP_Cinema, ST_PIP_UI, ST_PIP_Web, ST_PIP_Game;
    public GameObject[] ST_PI_Cinema;
    public GameObject[] ST_PI_UI;
    public GameObject[] ST_PI_Web;
    public GameObject[] ST_PI_Game;
    public int PeriResol;

    public void RT_TurnOnPI()
    {
        // TaskOrder
        int CO = RT.ConditionOrder[RT.TaskCount];

        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                if (CO < 4)
                    RT_PI_Cinema1.SetActive(true);
                else if (CO < 8)
                    RT_PI_Cinema2.SetActive(true);
                else if (CO < 12)
                    RT_PI_Cinema3.SetActive(true);
                else if (CO < 16)
                    RT_PI_Cinema4.SetActive(true);
                break;

            case 1:
                if (CO < 4)
                    RT_PI_UI1.SetActive(true);
                else if (CO < 8)
                    RT_PI_UI2.SetActive(true);
                else if (CO < 12)
                    RT_PI_UI3.SetActive(true);
                else if (CO < 16)
                    RT_PI_UI4.SetActive(true);
                break;
            case 2:
                if (CO < 4)
                    RT_PI_Web1.SetActive(true);
                else if (CO < 8)
                    RT_PI_Web2.SetActive(true);
                else if (CO < 12)
                    RT_PI_Web3.SetActive(true);
                else if (CO < 16)
                    RT_PI_Web4.SetActive(true);
                break;
            case 3:
                if (CO < 4)
                    RT_PI_Game1.SetActive(true);
                else if (CO < 8)
                    RT_PI_Game2.SetActive(true);
                else if (CO < 12)
                    RT_PI_Game3.SetActive(true);
                else if (CO < 16)
                    RT_PI_Game4.SetActive(true);
                break;
        }
    }

    public void RT_TurnOffPI()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                RT_PIP_Cinema.SetActive(false);
                RT_PI_Cinema1.SetActive(false);
                RT_PI_Cinema2.SetActive(false);
                RT_PI_Cinema3.SetActive(false);
                RT_PI_Cinema4.SetActive(false);
                break;
            case 1:
                RT_PIP_UI.SetActive(false);
                RT_PI_UI1.SetActive(false);
                RT_PI_UI2.SetActive(false);
                RT_PI_UI3.SetActive(false);
                RT_PI_UI4.SetActive(false);
                break;
            case 2:
                RT_PIP_Web.SetActive(false);
                RT_PI_Web1.SetActive(false);
                RT_PI_Web2.SetActive(false);
                RT_PI_Web3.SetActive(false);
                RT_PI_Web4.SetActive(false);
                break;
            case 3:
                RT_PIP_Game.SetActive(false);
                RT_PI_Game1.SetActive(false);
                RT_PI_Game2.SetActive(false);
                RT_PI_Game3.SetActive(false);
                RT_PI_Game4.SetActive(false);
                break;
        }
    }

    public void ST_TurnOnOffPI(bool TurnOnOff)
    {
        int SC = ST.SliderCount;

        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                ST_PI_Cinema[SC].SetActive(TurnOnOff);
                break;
            case 1:
                ST_PI_UI[SC].SetActive(TurnOnOff);
                break;
            case 2:
                ST_PI_Web[SC].SetActive(TurnOnOff);
                break;
            case 3:
                ST_PI_Game[SC].SetActive(TurnOnOff);
                break;
        }
    }

    public void ST_TurnOnOffPIP(bool TurnOnOff)
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                ST_PIP_Cinema.SetActive(TurnOnOff);
                break;
            case 1:
                ST_PIP_UI.SetActive(TurnOnOff);
                break;
            case 2:
                ST_PIP_Web.SetActive(TurnOnOff);
                break;
            case 3:
                ST_PIP_Game.SetActive(TurnOnOff);
                break;
        }
    }
}
