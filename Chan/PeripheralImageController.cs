using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Video;

public class PeripheralImageController : MonoBehaviour
{
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] GameObject RT_PI_Cinema_Fovea, RT_PI_Game_Fovea;
    [SerializeField] GameObject ST_PIP_Cinema, ST_PIP_HomeUI, ST_PIP_Web, ST_PIP_Game;
    [SerializeField] GameObject ST_PI_Cinema_Fovea, ST_PI_Game_Fovea;
    [SerializeField] VideoPlayer RT_VP_Cinema_Fovea, RT_VP_Game_Fovea;
    [SerializeField] VideoPlayer ST_VP_Cinema_Fovea, ST_VP_Game_Fovea;
    public GameObject[] RT_PI_Cinema;
    public GameObject[] RT_PI_HomeUI;
    public GameObject[] RT_PI_Web;
    public GameObject[] RT_PI_Game;
    public GameObject[] ST_PI_Cinema;
    public GameObject[] ST_PI_HomeUI;
    public GameObject[] ST_PI_Web;
    public GameObject[] ST_PI_Game;
    public VideoPlayer[] RT_VP_Cinema;
    public VideoPlayer[] RT_VP_Game;
    public VideoPlayer[] ST_VP_Cinema;
    public VideoPlayer[] ST_VP_Game;
    public int PeriResol;

    public void RT_TurnOnPI()
    {
        int CO = RT.ConditionOrder[RT.TaskCount];

        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                RT_PI_Cinema_Fovea.SetActive(true);
                RT_PI_Cinema[CO].SetActive(true);
                RT_VP_Cinema_Fovea.Prepare();
                RT_VP_Cinema[CO].Prepare();
                break;
            case 1:
                RT_PI_HomeUI[CO].SetActive(true);
                break;
            case 2:
                RT_PI_Web[CO].SetActive(true);
                break;
            case 3:
                RT_PI_Game_Fovea.SetActive(true);
                RT_PI_Game[CO].SetActive(true);
                RT_VP_Game_Fovea.Prepare();
                RT_VP_Game[CO].Prepare();
                break;
        }
    }

    public void RT_TurnOffPI()
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                RT_PI_Cinema_Fovea.SetActive(false);
                for (int i = 0; i < 4; i++)
                    RT_PI_Cinema[i].SetActive(false);
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                    RT_PI_HomeUI[i].SetActive(false);
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                    RT_PI_Web[i].SetActive(false);
                break;
            case 3:
                RT_PI_Game_Fovea.SetActive(false);
                for (int i = 0; i < 4; i++)
                    RT_PI_Game[i].SetActive(false);
                break;
        }
    }

    public void RT_TurnOnOffPIP(bool TurnOnOff)
    {
        switch (RT.ConditionList[RT.ConditionCount])
        {
            case 0:
                RT.Cinema_Foveated.SetActive(TurnOnOff);
                break;
            case 1:
                RT.UI_Foveated.SetActive(TurnOnOff);
                break;
            case 2:
                RT.Web_Foveated.SetActive(TurnOnOff);
                break;
            case 3:
                RT.Game_Foveated.SetActive(TurnOnOff);
                break;
        }
    }

    public void ST_TurnOnOffPI(bool TurnOnOff)
    {
        int SC = ST.SliderCount;
        int Condition = RT.ConditionList[RT.ConditionCount];

        switch (Condition)
        {
            case 0:
                ST_PI_Cinema_Fovea.SetActive(TurnOnOff);
                ST_PI_Cinema[SC].SetActive(TurnOnOff);

                break;

            case 1:
                ST_PI_HomeUI[SC].SetActive(TurnOnOff);
                RT.Block.SetActive(false);
                break;
            case 2:
                ST_PI_Web[SC].SetActive(TurnOnOff);
                RT.Block.SetActive(false);
                break;
            case 3:
                ST_PI_Game_Fovea.SetActive(TurnOnOff);
                ST_PI_Game[SC].SetActive(TurnOnOff);
                break;
        }

        if (TurnOnOff)
        {
            if (Condition == 0)
            {
                ST_VP_Cinema_Fovea.Prepare();
                ST_VP_Cinema[SC].Prepare();
                ST_VP_Cinema_Fovea.time = 0;
                ST_VP_Cinema[SC].time = 0;
                StartCoroutine(PlayVideo(ST_VP_Cinema_Fovea, ST_VP_Cinema[SC]));

            }
            else if (Condition == 3)
            {
                ST_VP_Game_Fovea.Prepare();
                ST_VP_Game[SC].Prepare();
                ST_VP_Game_Fovea.time = 0;
                ST_VP_Game[SC].time = 0;
                StartCoroutine(PlayVideo(ST_VP_Game_Fovea, ST_VP_Game[SC]));
            }
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
                ST_PIP_HomeUI.SetActive(TurnOnOff);
                break;
            case 2:
                ST_PIP_Web.SetActive(TurnOnOff);
                break;
            case 3:
                ST_PIP_Game.SetActive(TurnOnOff);
                break;
        }
    }

    public void PlayVideo()
    {
        int CO = RT.ConditionOrder[RT.TaskCount];
        int Condition = RT.ConditionList[RT.ConditionCount];

        if (Condition == 0)
            StartCoroutine(PlayVideo(RT_VP_Cinema_Fovea, RT_VP_Cinema[CO]));
        else if (Condition == 3)
            StartCoroutine(PlayVideo(RT_VP_Game_Fovea, RT_VP_Game[CO]));
    }

    public IEnumerator PlayVideo(VideoPlayer VP1, VideoPlayer VP2)
    {
        yield return new WaitForSeconds(1f);
        RT.Block.SetActive(false);
        VP1.Play();
        VP2.Play();
        yield return null;
    }
}

