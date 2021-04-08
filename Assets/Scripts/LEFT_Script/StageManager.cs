using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class StageManager : MonoBehaviour
{
    private string[] numBoxName;
    public GameDataManager mydata;
    // Use this for initialization

    public void Stage1()
    {
        mydata.refresh();
        StreamReader sr = File.OpenText("Assets/Resources/Stage1.txt");
        string[] line = sr.ReadToEnd().Split('\n');
        sr.Close();
        numBoxName = new string[line.Length];
        for (int i = 0; i < line.Length; i++)
            numBoxName[i] = line[i];

        for (int i = 0; i < line.Length; i++)
        {
            Debug.Log(Convert.ToInt32(numBoxName[i]));
            mydata.makeInBox(Convert.ToInt32(numBoxName[i]));
        }
    }/* 나중에 
    public void Stage2()
    {
        mydata.refresh();
        StreamReader sr = File.OpenText("Assets/Resources/Stage2.txt");
        numBoxName = new string[3];
        for (int i = 0; i < 3; i++)
            numBoxName[i] = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(Convert.ToInt32(numBoxName[i]));
            mydata.makeInBox(Convert.ToInt32(numBoxName[i]));
        }
    }*/
}
