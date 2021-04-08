using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class movingController : MonoBehaviour {

    private int frameCheckNum;
    private int InFunc, OutFunc, ArrayInFunc, ArrayOutFunc, AddOrSub, plusminus;
    private int type;
    private int whereToGo;
    private int i;
    private int highlightCnt;

    private string[] line;
    private string numBlock;
    private string blockMode;
    public string myValue;

    public GameDataManager mydata;
    public Character mychar;
    public PlayButton myplay;

	// Use this for initialization
	void Start () {
        frameCheckNum = 0;
        InFunc = 1;
        OutFunc = 501;
        ArrayInFunc = 1001;
        ArrayOutFunc = 1501;
        AddOrSub = 2001;
        plusminus = 2501;
        highlightCnt = 0;
	}
    public void Play()
    {
        i = 0;
        line = mydata.getWholeLine().Split(null);
        Debug.Log(line[0]);
        frameCheckNum = 10000;
    }
    public void lineFunction()
    {
        while ((line[i] !="") && (frameCheckNum == 10000))
        {
            switch(line[i])
            {
                case "InBlock":
                    i++;
                    
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = InFunc;
                    break;

                case "OutBlock":
                    i++;
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = OutFunc;
                    break;

                case "ContainerInBlock":
                    i++;
                    numBlock = line[i++];
                    blockMode = line[i++];
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = ArrayInFunc;
                    break;

                case "ContainerOutBlock":
                    i++;
                    numBlock = line[i++];
                    blockMode = line[i++];
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = ArrayOutFunc;
                    break;

                case "AddBlock":
                    i++;
                    numBlock = line[i++];
                    blockMode = line[i++];
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = AddOrSub;
                    break;

                case "SubBlock":
                    i++;
                    numBlock = line[i++];
                    blockMode = line[i++];
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = AddOrSub;
                    break;

                case "IncrementBlock":
                    i++;
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = plusminus;
                    break;

                case "DecrementBlock":
                    i++;
                    myValue = line[i++];
                    myplay.oneGame.highLightList[highlightCnt].GetComponent<BlockHighLightNotify>().onHighLightClick();
                    highlightCnt++;
                    frameCheckNum = plusminus;
                    break;

                case "IfBlock":  
                    i++;
                    highlightCnt++;
                    break;

                case "ForBlock":
                    i++;
                    highlightCnt++;
                    break;

                case "BreakBlock":
                    i++;
                    highlightCnt++;
                    break;

                case "EndBlock":
                    i++;
                    highlightCnt++;
                    break;

                case "IfEndBlock":
                    i++;
                    highlightCnt++;
                    break;

                default:
                    Debug.Log("DEFAULT!!");
                    break;
            }
        }
    }
    public int blockToInt(string str)
    {
        switch (str)
        {
            case "block1":
                return 1;
            case "block2":
                return 2;
            case "block3":
                return 3;
            case "block4":
                return 4;
            case "block5":
                return 5;
            case "block6":
                return 6;
            case "block7":
                return 7;
            case "block8":
                return 8;
            case "block9":
                return 9;
            case "block0":
                return 0;
            default:
                return -1;
        }
    }
    public void setCheckNum(int num)
    {
        frameCheckNum = num;
    }
    public int getCheckNum()
    {
        return frameCheckNum;
    }
    public void refresh()
    {
        frameCheckNum = 0;
        highlightCnt = 0;
        line = new string[0];
        i = 0;
    }
    public void Update()
    {
        //In Start
        if ((frameCheckNum > 0) && (frameCheckNum < 500))
        {
            frameCheckNum++;
            mychar.InButton(mydata.getFirstInBox());
        }
        //In End

        //Out Start
        if ((frameCheckNum > 500) && (frameCheckNum < 1000))
        {
            frameCheckNum++;
            mychar.OutButton(mydata.Outbox);
            type = 2;
        }
        //Out End

        //ArrayIn Start
        if ((frameCheckNum > 1000) && (frameCheckNum < 1500))
        {
            frameCheckNum++;

            if (blockMode == "BasicMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "ArrayMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "PointMode")
            {
                whereToGo = Convert.ToInt32(mychar.mybasket.transform.GetChild(blockToInt(numBlock)).GetChild(0).GetChild(0).GetComponent<Text>());
            }

            mychar.ArrayInButton(mychar.mybasket.transform.GetChild(whereToGo).gameObject);

            type = 1;
        }
        //ArrayIn End

        //ArrayOut Start
        if ((frameCheckNum > 1500) && (frameCheckNum < 2000))
        {
            frameCheckNum++;

            if (blockMode == "BasicMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "ArrayMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "PointMode")
            {
                whereToGo = Convert.ToInt32(mychar.mybasket.transform.GetChild(blockToInt(numBlock)).GetChild(0).GetChild(0).GetComponent<Text>());
            }

            mychar.ArrayOutButton(mychar.mybasket.transform.GetChild(whereToGo).gameObject);
        }
        //ArrayOut End

        //AddSub
        if ((frameCheckNum > 2000) && (frameCheckNum < 2500))
        {
            if (blockMode == "BasicMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "ArrayMode")
            {
                whereToGo = blockToInt(numBlock);
            }
            else if (blockMode == "PointMode")
            {
                whereToGo = Convert.ToInt32(mychar.mybasket.transform.GetChild(blockToInt(numBlock)).GetChild(0).GetChild(0).GetComponent<Text>());
            }

            mychar.ArrAS(mychar.mybasket.transform.GetChild(whereToGo).gameObject);
        }

        if ((frameCheckNum > 2500) && (frameCheckNum < 3000))
        {
            mychar.charBasket.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = myValue;
            frameCheckNum = 10000;
        }

            if (frameCheckNum == 9980)
        {
            if(type == 1)
            {
                mychar.charBasket.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = myValue;
                frameCheckNum = 10000;
            }
            else if (type == 2)
            {
                mychar.charBasket.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = myValue;
                frameCheckNum = 10000;
            }
        }
        /////////////////////////////////////////
        if (frameCheckNum == 9990)
        {
            if (type == 1)
            {
                mychar.mybasket.transform.GetChild(whereToGo).GetChild(0).GetChild(0).GetComponent<Text>().text = myValue;
                frameCheckNum = 10000;
            }
            else if (type == 2)
            {
                mychar.put(mychar.charBasket.transform.GetChild(0).gameObject, mydata.Outbox);
                frameCheckNum = 10000;
            }
        }
        /////////////////////////////////////////


        if (frameCheckNum == 10000)
        {
            lineFunction();
        }
    }
}
