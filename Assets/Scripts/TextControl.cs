using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class TextControl : MonoBehaviour {

    public Text myText;
    public GameObject myObject;
    public void setBoxText(int tmp, int num)
    {
        myText = GameObject.Find("Text_Mother").GetComponent<Text>();
        Text myObj = Instantiate(myText) as Text;
        myObj.name = Convert.ToString(tmp);
        myObj.fontSize = 35;
        myObj.text = Convert.ToString(num);
        myObj.transform.parent = GameObject.Find("Box_" + Convert.ToString(tmp)).transform;
    }
    public void setArrayText(int num, int address)
    {
        GameObject temp = GameObject.Find("Text" +Convert.ToString(address)) as GameObject;
        Text newText = temp.GetComponent<Text>();
        newText.fontSize = 35;
        newText.text = Convert.ToString(num);

        Debug.Log("test3");
    }
    public String getObjectText(GameObject tmp)
    {
        Debug.Log("test4");
        string result;
        result = tmp.name;
        return result;
    }
}
