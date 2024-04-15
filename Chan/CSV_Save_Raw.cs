using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Content;
using System.Data.Common;

public class CSV_Save_Raw : MonoBehaviour
{
    [SerializeField] NoticeManager NM;
    [SerializeField] ExpManager_SliderTest ST;
    [SerializeField] ExpManager_RandomTest RT;
    [SerializeField] UserGazePostionAndAdjustFOV User;
    [SerializeField] CSV_Save_Processed CSV_P;
    string[] csvHeaders = new string[] { "Data Name" };
    float[] DA_ST = new float[10];
    float[] DA_RT = new float[10];
    string csvFileName;
    string FilePath;

    void FixedUpdate()
    {
        if (RT.Term_RT_ProceedTask)
        {
            DA_RT[0] = RT.ConditionList[RT.ConditionCount];
            DA_RT[1] = RT.TaskCount;
            DA_RT[2] = RT.ImageOrder;
            DA_RT[3] = RT.PlayerAnswer;
            DA_RT[4] = RT.IsCorrect;
            DA_RT[5] = User.CameraFOV;
            AppendToCsv(DA_RT);
        }
        else if (ST.Term_ST_ProceedTask)
        {
            DA_ST[0] = ST.PlayerAnswer;
            DA_ST[1] = ST.TotalTestTime;
            DA_ST[2] = ST.ST_MinimumFRS;
            DA_ST[3] = User.CameraFOV;
            DA_ST[4] = ST.AdjustmentCount;
            AppendToCsv(DA_ST);
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
        string csvDirectoryName = "RawData";
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);

        if (RT.Term_RandomTest)
            csvFileName = "FM_RD_SampleNumber_" + NM.SampleNumber + "_Condition_" + RT.ConditionList[RT.ConditionCount] + "_RandomTest_TaskCount" + RT.TaskCount + ".csv";

        if (ST.Term_SliderTest)
            csvFileName = "FM_SD_SampleNumber_" + NM.SampleNumber + "_Condition_" + RT.ConditionList[RT.ConditionCount] + "_SliderTest_Task" + ".csv";

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
