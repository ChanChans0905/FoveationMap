using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Content;
using System.Data.Common;

public class CSV_Save_Processed : MonoBehaviour
{
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] NoticeManager NT;
    [SerializeField] GetUserPositionToShader User;
    // 저장 데이터 : Condition, Task, 이미지 순서, foveation, 정답 여부, 
    string[] csvHeaders = new string[] { "Data Name" };
    float[] DA_ST = new float[10];
    float[] DA_RT = new float[10];
    string csvFileName;
    string FilePath;

    public void Save_CSV_Analysis()
    {
        // number of trial, 자극 수준, 답변까지의 소요 시간, 정답 영상 위치, 피험자 응답 위치, Y/N 결과, reversal 횟수, reversal 여부, 실시간 응시 위치, 한계조건, 한계조건 총 소요시간, 횟수
        // condition, #of trial, 정답 영상 위치, 참가자 정답 입력 영상 위치, 정답 여부, reversal 여부, 
        if (ST.Term_SliderTest)
        {
            DA_ST[0] = ST.ConditionList[ST.ConditionCount];
            DA_ST[1] = ST.TaskCount;
            DA_ST[2] = ST.ImageOrder;
            DA_ST[3] = ST.PlayerAnswer;
            DA_ST[4] = ST.IsCorrect;
            DA_ST[5] = ST.ReversalAdded;
            DA_ST[6] = ST.ReverseCount;
            DA_ST[7] = User.CameraFOV;
            AppendToCsv(DA_ST);
        }

        if (RT.Term_RandomTest)
        {
            DA_RT[0] = RT.PlayerAnswer;
            DA_RT[1] = RT.TotalTestTime;
            AppendToCsv(DA_RT);
        }

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

        if (ST.Term_SliderTest)
            csvFileName = "FM_PD_SampleNumber" + NT.SampleNumber + "_Condition_" + ST.ConditionList[ST.ConditionCount] + "_SliderTest" + ".csv";

        if (RT.Term_RandomTest)
            csvFileName = "FM_PD_SampleNumber" + NT.SampleNumber + "_Condition_" + ST.ConditionList[ST.ConditionCount] + "_RandomTest" + ".csv";

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
