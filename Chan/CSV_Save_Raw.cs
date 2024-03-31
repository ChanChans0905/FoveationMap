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
    [SerializeField] GetUserPositionToShader User;
    [SerializeField] CSV_Save_Processed CSV_P;
    string[] csvHeaders = new string[] { "Data Name" };
    float[] DA_ST = new float[10];
    float[] DA_RT = new float[10];
    string csvFileName;
    string FilePath;

    void FixedUpdate()
    {
        if (ST.Term_ProceedTask)
        {
            DA_ST[0] = ST.ConditionList[ST.ConditionCount];
            DA_ST[1] = ST.TaskCount;
            DA_ST[2] = ST.ImageOrder;
            DA_ST[3] = ST.PlayerAnswer;
            DA_ST[4] = ST.IsCorrect;
            DA_ST[5] = ST.ReversalAdded;
            DA_ST[6] = ST.ReverseCount;
            DA_ST[7] = User.CameraFOV;
            DA_ST[8] = User.UserGazePoint.x;
            DA_ST[8] = User.UserGazePoint.y;
            DA_ST[8] = User.UserGazePoint.z;
            AppendToCsv(DA_ST);
        }
        else if (RT.Term_ProceedTask)
        {
            DA_RT[0] = RT.PlayerAnswer;
            DA_RT[1] = RT.TotalTestTime;
            DA_RT[2] = RT.RT_MinimumFRS;
            DA_RT[3] = User.CameraFOV;
            DA_RT[4] = RT.AdjustmentCount;
            DA_RT[8] = User.UserGazePoint.x;
            DA_RT[8] = User.UserGazePoint.y;
            DA_RT[8] = User.UserGazePoint.z;
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
        string csvDirectoryName = "RawData";
        string dir = Application.dataPath + "/" + csvDirectoryName;
        Directory.CreateDirectory(dir);

        if (ST.Term_SliderTest)
            csvFileName = "FM_RD_SampleNumber_" + NM.SampleNumber + "_Condition_" + ST.ConditionList[ST.ConditionCount] + "_SliderTest" + ".csv";

        if (RT.Term_RandomTest)
            csvFileName = "FM_RD_SampleNumber_" + NM.SampleNumber + "_Condition_" + ST.ConditionList[ST.ConditionCount] + "_RandomTest" + ".csv";

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
