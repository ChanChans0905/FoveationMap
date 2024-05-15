using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Content;
using System.Data.Common;

public class CSV_Save_Processed_RT : MonoBehaviour
{
    [SerializeField] NoticeManager NM;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    // 저장 데이터 : Condition, Task, 이미지 순서, foveation, 정답 여부, 
    string[] csvHeaders = new string[] { "Condition", "TaskCount", "ConditionOrder", "ImageOrder", "PlayerAnswer", "IsCorrect", "CameraFOV", "AnsweringTimer", "RepetitionCount" };
    float[] PlayerData = new float[9];
    string csvFileName;
    string FilePath;

    public void Save_CSV_Processed()
    {
        // TaskCount, FoveaRegionSize, FoveationLevel, OriginOrder, PlayerAnswer, AnsweringTime
        PlayerData[0] = RT.ConditionList[RT.ConditionCount];
        PlayerData[1] = RT.TaskCount;
        PlayerData[2] = RT.ConditionOrder[RT.TaskCount];
        PlayerData[3] = RT.ImageOrder; // 1이면 원본 먼저, 0이면 foveation 먼저
        PlayerData[4] = RT.PlayerAnswer; // 0이면 먼저 나온 이미지 선택, 1이면 뒤에 나온 이미지 선택
        PlayerData[5] = RT.IsCorrect; // 1이면 정답, 0이면 오답
        PlayerData[6] = User.CameraFOV;
        PlayerData[7] = RT.AnsweringTimer;
        PlayerData[8] = RT.RepetitionCount;
        AppendToCsv(PlayerData);
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
        string csvDirectoryName = "ProcessedData";
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);

        csvFileName = "FM_SampleNumber_" + NM.SampleNumber + "_RandomTest.csv";

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
