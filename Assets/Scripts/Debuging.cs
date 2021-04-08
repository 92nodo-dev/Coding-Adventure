using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
/*
기본적으로 배열 클릭은 그 클릭된 곳의 값을 가져옴

for나 if를 위한 숫자는 스테이지마다 가변적으로 제공하거나 사용자가 연산을 통해서 만들도록 유도

나중에 배열안의 값을 실제값이 아닌 주소값으로 여기는 방법이 필요
 */
public class Debuging {

    private int myValue;        //캐릭터 value
    private int[] inBox;
    private int[] outBox;
    private int[] arrayBox;

    private bool pairing;   //pairing error check
    private bool scope;     //scope error check
    private bool ordering;   //ordering error check

    private int inCount;        //InBox Count
    private int outCount;       //OutBox count

    public int lineCount;       //코드의 전체 길이
    private int orderCount;     //코드 순서 count

    public string[] blockList;  //코드를 한줄단위로 구분
    private string numBlock;
    private string blockMode;

    private int gameCount = 0;
    public string wholeLine = "";
    private bool error = false;

    public List<GameObject> highLightList;          //하이라이트 할 블록들의 순서리스트
    //------------------
    public Debuging(string line)    //스테이지에 따른 설정               //Inbox 등 크기 믿 숫자를 받아오는 함수 필요//
    {
        StreamReader sr = File.OpenText("Assets/Resources/Stage1.txt");
        string[] fileLine = sr.ReadToEnd().Split('\n');
        sr.Close();

        highLightList = new List<GameObject>();

        inBox = new int[fileLine.Length];

        for(int i = 0; i < fileLine.Length; i++)
        {
            inBox[i] = Convert.ToInt32(fileLine[i]);
            Debug.Log(inBox[i]);
        }

        sr = File.OpenText("Assets/Resources/Stage1Result.txt");
        string[] resultLine = sr.ReadToEnd().Split('\n');
        sr.Close();

        outBox = new int[resultLine.Length];
        arrayBox = new int[10];
        myValue = 0;            //임시

        inCount = 0;
        outCount = 0;

        char split = '\n';
        blockList = line.Split(split);

        lineCount = blockList.Length - 1;   //공백이 생겨서 -1
        orderCount = 0;

        pairingCheck();
        scopeCheck();
        OrderingCheck();
    }
    //------------------
    public bool getPairingCheck()
    {
        return pairing;
    }
    public bool getScopeCheck()
    {
        return scope;
    }
    public bool getOrderingCheck()
    {
        return ordering;
    }
    public int getMyValue()
    {
        return myValue;
    }
    public string getInBox()
    {
        string result = "";
        for (int i = 0; i < inBox.Length; i++)
        {
            result += inBox[i].ToString() + " ";
        }
        return result;
    }
    public string getOutBox()
    {
        string result = "";
        for (int i = 0; i < outBox.Length; i++)
        {
            result += outBox[i].ToString() + " ";
        }
        return result;
    }
    public string getArrayBox()
    {
        string result = "";
        for (int i = 0; i < arrayBox.Length; i++)
        {
            result += arrayBox[i].ToString() + " ";
        }
        return result;
    }
    public int getArrayBox(int i)
    {
        return arrayBox[i];
    }

    public void run()
    {
        forBlock(1, 1);
    }
    
    //---------------
    private int forBlock(int num, int count)
    {
        if (orderCount == lineCount)    //run()을 for로 시작했기에 끝내줘야함
            num--;
        while (0 < num)
        {
            GameObject Content = GameObject.Find("Content");
            GameObject Cell = Content.transform.GetChild(orderCount).gameObject;
            GameObject highLight = Cell.transform.GetChild(0).gameObject;
            highLightList.Add(highLight);

            string block = blockList[orderCount++];     //하나의 블록
            string blockName = block.Split(null)[0];   //블록의 이름
            Debug.Log(gameCount++ + " --" + blockName + "--" + orderCount);

            if (!error)
            {
                wholeLine += blockName + " ";
            }
            else
                wholeLine += "error ";

            switch (blockName)
            {
                case "InBlock":
                    try
                    {
                        Debug.Log("CHECK2");
                        inBlock();
                    }
                    catch(Exception e)
                    {
                        Debug.Log("Error : Out of Array");
                        error = true;
                    }
                    break;

                case "OutBlock":
                    try
                    {
                        outBlock();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : Out of Array");
                        error = true;
                    }
                    break;

                case "ContainerInBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    wholeLine += numBlock + " ";
                    numBlock = subStitNumber(numBlock);     //숫자가 없다면 null
                    Debug.Log(numBlock);
                    try
                    {
                        arrayIn(Convert.ToInt32(numBlock));
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : No Number");
                        error = true;
                    }
                    break;

                case "ContainerOutBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    wholeLine += numBlock + " ";
                    numBlock = subStitNumber(numBlock);
                    try
                    {
                        arrayOut(Convert.ToInt32(numBlock));
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : No Number");
                        error = true;
                    }
                    break;

                case "AddBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    wholeLine += numBlock + " ";
                    numBlock = subStitNumber(numBlock);
                    try
                    {
                        addBlock(Convert.ToInt32(numBlock));
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : No Number");
                        error = true;
                    }
                    break;

                case "SubBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    wholeLine += numBlock + " ";
                    numBlock = subStitNumber(numBlock);
                    try
                    {
                        subBlock(Convert.ToInt32(numBlock));
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : No Number");
                        error = true;
                    }
                    break;
                case "IncrementBlock":
                    incrementBlock();
                    break;

                case "DecrementBlock":
                    decrementBlock();
                    break;
                case "IfBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    numBlock = subStitNumber(numBlock);
                    string optionStr = block.Split(null)[3];   //블록의 조건

                    switch (optionStr)
                    {
                        case "item1":
                            if (!(myValue < Convert.ToInt32(numBlock)))      //조건문이 False라면
                                ifBlock();
                            break;

                        case "item2":
                            if (!(myValue > Convert.ToInt32(numBlock)))
                                ifBlock();
                            break;

                        case "item3":
                            if (!(myValue == Convert.ToInt32(numBlock)))
                                ifBlock();
                            break;

                        case "item4":
                            if (!(myValue <= Convert.ToInt32(numBlock)))
                                ifBlock();
                            break;

                        case "item5":
                            if (!(myValue >= Convert.ToInt32(numBlock)))
                                ifBlock();
                            break;

                        case "item6":
                            if (!(myValue != Convert.ToInt32(numBlock)))
                                ifBlock();
                            break;

                        default:
                            Debug.Log("Error : No Option");
                            break;
                    }
                    break;

                case "ForBlock":
                    blockMode = block.Split(null)[1];   //Mode
                    numBlock = block.Split(null)[2];    //블록의 숫자
                    numBlock = subStitNumber(numBlock);
                    try
                    {
                        if (numBlock == "0")        //for에 숫자가 0이 들어오면 for문을 작동하지 못하게 해야함 ㅡㅡ>>
                            breakFor();

                        forBlock(Convert.ToInt32(numBlock), orderCount);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error : No Number");
                        error = true;
                    }
                    break;

                case "BreakBlock":
                    breakFor(); //해당 for문의 End 다음으로 이동
                    num = -1;   //해당 for문을 종료
                    break;

                case "EndBlock":
                    num--;
                    if (num <= 0)///0
                    {
                        num = 0;
                        break;
                    }
                    orderCount = count;
                    break;

                case "IfEndBlock":
                    break;

                default:
                    Debug.Log("DEFAULT!!"); //블록이 배치되어 있지 않을 때
                    num--;
                    break;
            }
            if (orderCount == lineCount)    //run()을 for로 시작했기에 끝내줘야함
                num--;
        }

        return myValue;
    }

    private void inBlock()
    {
        myValue = inBox[inCount++];
        Debug.Log("myvalue");
        wholeLine += myValue + " ";
    }
    private void outBlock()
    {
        outBox[outCount++] = myValue;
    }
    private void arrayIn(int num)
    {
        wholeLine += blockMode + " ";

        if (blockMode == "BasicMode")
        {
            arrayBox[num] = myValue;    //수정
        }
        else if (blockMode == "ArrayMode")
        {
            arrayBox[num] = myValue;
        }
        else if (blockMode == "PointMode")
        {
            arrayBox[arrayBox[num]] = myValue;
        }
        wholeLine += Convert.ToString(myValue) + " ";
    }
    private void arrayOut(int num)
    {
        wholeLine += blockMode + " ";

        if (blockMode == "BasicMode")
        {
            myValue = arrayBox[num];    //수정
        }
        else if (blockMode == "ArrayMode")
        {
            myValue = arrayBox[num];
        }
        else if (blockMode == "PointMode")
        {
            myValue = arrayBox[arrayBox[num]];
        }
        wholeLine += Convert.ToString(myValue) + " ";
    }
    private void addBlock(int num)
    {
        wholeLine += blockMode + " ";

        int myValueInt = myValue;

        if (blockMode == "BasicMode")
        {
            myValueInt += num;
        }
        else if (blockMode == "ArrayMode")
        {
            myValueInt += arrayBox[num];
        }
        else if (blockMode == "PointMode")
        {
            myValueInt += arrayBox[arrayBox[num]];
        }

        myValue = myValueInt;
        wholeLine += Convert.ToString(myValue) + " ";
    }
    private void subBlock(int num)
    {
        wholeLine += blockMode + " ";

        int myValueInt = myValue;

        if (blockMode == "BasicMode")
        {
            myValueInt -= num;
        }
        else if (blockMode == "ArrayMode")
        {
            myValueInt -= arrayBox[num];
        }
        else if (blockMode == "PointMode")
        {
            myValueInt -= arrayBox[arrayBox[num]];
        }

        myValue = myValueInt;
        wholeLine += Convert.ToString(myValue) + " ";

    }
    private void incrementBlock()
    {
        myValue++;
        wholeLine += Convert.ToString(myValue) + " ";
    }
    private void decrementBlock()
    {
        myValue--;
        wholeLine += Convert.ToString(myValue) + " ";
    }
    /*
    private void incrementBlock()
    {
        int myValueInt = Convert.ToInt32(myValue);
        myValueInt++;
        myValue = myValueInt;
        wholeLine += Convert.ToString(myValue) + " ";
    }
    private void decrementBlock()
    {
        int myValueInt = Convert.ToInt32(myValue);
        myValueInt--;
        myValue = myValueInt;
        wholeLine += Convert.ToString(myValue) + " ";
    }
    */
    private void ifBlock()
    {
        int IfCount = 1;
        for (int i = orderCount; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "IfEndBlock" && IfCount == 1)
            {
                orderCount = i;
                break;
            }
            else if (blockList[i].Split(null)[0] == "IfBlock")    //내부에 if문이 있다면 count++
                IfCount++;
            else if (blockList[i].Split(null)[0] == "IfEndBlock")
                IfCount--;
        }
    }
    private void breakFor() //현재 for를 break하는 함수
    {
        int forCount = 1;       //내부에 중첩된 for문이 있는지 확인
        for (int i = orderCount; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "EndBlock" && forCount == 1)    //for의 End 다음으로 이동
            {
                orderCount = i + 1;
                break;
            }
            else if (blockList[i].Split(null)[0] == "ForBlock")    //내부에 for문이 있다면 count++
                forCount++;
            else if (blockList[i].Split(null)[0] == "EndBlock")
                forCount--;
        }
    }
    private string subStitNumber(string str)
    {
        switch (str)
        {
            case "block1":
                str = "1";
                break;
            case "block2":
                str = "2";
                break;
            case "block3":
                str = "3";
                break;
            case "block4":
                str = "4";
                break;
            case "block5":
                str = "5";
                break;
            case "block6":
                str = "6";
                break;
            case "block7":
                str = "7";
                break;
            case "block8":
                str = "8";
                break;
            case "block9":
                str = "9";
                break;
            case "block0":
                str = "0";
                break;
        }
        return str;
    }
    private string subStitOption(string str)
    {
        switch (str)
        {
            case "item1":
                str = "<";
                break;
            case "item2":
                str = ">";
                break;
            case "item3":
                str = "==";
                break;
            case "item4":
                str = "<=";
                break;
            case "item5":
                str = ">=";
                break;
            case "item6":
                str = "!=";
                break;
        }
        return str;
    }
    private void pairingCheck()     //for-end, if-ifend의 배치 확인
    {
        int fCount = 0, feCount = 0;         //for Block Count
        int iCount = 0, ieCount = 0;         //if Block Count

        for (int i = 0; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "ForBlock")
                fCount++;
            else if (blockList[i].Split(null)[0] == "EndBlock")
                feCount++;
            else if (blockList[i].Split(null)[0] == "IfBlock")
                iCount++;
            else if (blockList[i].Split(null)[0] == "IfEndBlock")
                ieCount++;
        }

        if (fCount != feCount || iCount != ieCount)
            pairing = false;
        else
            pairing = true;
    }
    private void scopeCheck()       //for, if의 scope의 곂침 확인
    {
        int fScope = 0;
        int iScope = 0;
        scope = true;

        for (int i = 0; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "ForBlock")
            {
                for (int j = i + 1; j < lineCount; j++)
                {
                    if (blockList[j].Split(null)[0] == "IfBlock")
                        iScope++;
                    else if (blockList[j].Split(null)[0] == "IfEndBlock")
                        iScope--;
                    else if (blockList[j].Split(null)[0] == "ForBlock")
                        iScope = 0;
                    else if (blockList[j].Split(null)[0] == "EndBlock")
                    {
                        //Debug.Log("IN for = " + iCount);
                        if (iScope == 0)
                        {
                            scope = true;
                            i = j + 1;
                            break;
                        }
                        else
                        {
                            scope = false;
                            return;
                        }
                    }
                }
            }
            else if (blockList[i].Split(null)[0] == "IfBlock")
            {
                for (int j = i + 1; j < lineCount; j++)
                {
                    if (blockList[j].Split(null)[0] == "ForBlock")
                        fScope++;
                    else if (blockList[j].Split(null)[0] == "EndBlock")
                        fScope--;
                    else if (blockList[j].Split(null)[0] == "IfBlock")
                        fScope = 0;
                    else if (blockList[j].Split(null)[0] == "IfEndBlock")
                    {
                        //Debug.Log("IN if = " + fCount);
                        if (fScope == 0)
                        {
                            scope = true;
                            i = j + 1;
                            break;
                        }
                        else
                        {
                            scope = false;
                            return;
                        }
                    }
                }
            }
        }
    }
    private void OrderingCheck()        //for, if의 순서 확인    //////
    {
        int fOrder = 0, feOrder = 0;
        ordering = true;

        for (int i = 0; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "ForBlock")
            {
                fOrder++;
            }
            else if (blockList[i].Split(null)[0] == "EndBlock")
            {
                fOrder--;
                if (fOrder < 0)
                {
                    ordering = false;
                    return;
                }
            }
        }

        for (int i = 0; i < lineCount; i++)
        {
            if (blockList[i].Split(null)[0] == "IfBlock")
            {
                feOrder++;
            }
            else if (blockList[i].Split(null)[0] == "IfEndBlock")
            {
                feOrder--;
                if (feOrder < 0)
                {
                    ordering = false;
                    return;
                }
            }
        }
    }
}
