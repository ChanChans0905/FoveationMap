using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Content;

public class CSV_Save_Processed : MonoBehaviour
{
    [SerializeField] ExpManager ExpManager;
    // 저장 데이터 : Condition, Task, 이미지 순서, foveation, 정답 여부, 
    string[] csvHeaders = new string[] { "Data Name" };
    float[] DataArray = new float[10];
    string FilePath;
    

    public void Save_CSV_Analysis()
    {
        SaveDataIntoArray();
        AppendToCsv(DataArray);
    }

    private void SaveDataIntoArray()
    {
        // number of trial, 자극 수준, 답변까지의 소요 시간, 정답 영상 위치, 피험자 응답 위치, Y/N 결과, reversal 횟수, reversal 여부, 실시간 응시 위치, 한계조건, 한계조건 총 소요시간, 횟수

        // DataArray[0] = DC.CMScombination[DC.CMSchangeCount - 1];
        // DataArray[1] = DC.taskCount;
        // DataArray[2] = DC.TaskScenario[DC.CMSchangeCount - 1, DC.taskCount];
        // DataArray[3] = DC.LaneChangeTime[DC.CMSchangeCount - 1, DC.taskCount];
        // DataArray[4] = DC.FollowingCarSpeed[DC.CMSchangeCount - 1, DC.taskCount];
        // DataArray[5] = Log_LaneChangeComplete;
        // DataArray[6] = Log_FirstReactionTime;
        // DataArray[7] = DC.NumOfCollision;
        // DataArray[8] = RayCaster.EOR_Time;
        // DataArray[9] = RayCaster.NumberOfGlances;
    }

    public void AppendToCsv(float[] data)
    {
        using (StreamWriter sw = File.AppendText(FilePath))
        {
            string csvFinalString = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (csvFinalString != "")
                {
                    csvFinalString += ",";
                }
                csvFinalString += data[i];
            }
            csvFinalString += ",";
            sw.WriteLine(csvFinalString);
        }
    }

    public void New_CSV_File()
    {
        string csvDirectoryName = "DataForAnalysis";
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);

        string csvFileName = "FoveationMap_DataForAnalysis" + ExpManager.SampleNumber + ".csv";
        FilePath = Application.dataPath + "/" + csvDirectoryName + "/" + csvFileName;

        using (StreamWriter sw = File.CreateText(FilePath))
        {
            string finalString = "";
            for (int i = 0; i < csvHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += ",";
                }
                finalString += csvHeaders[i];
            }
            finalString += ",";
            sw.WriteLine(finalString);
        }
    }
}
