using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{

    public GameObject space;
    public GameObject Inbox, myCharacter, Outbox;
    public GameObject basket;
    public GameObject[] inBoxNum;
    public GameObject[] TotalBox;

    private int basicSpeed = 0;
    private int TotalBoxCount = 0;
    private int OutBoxCount = 0;

    private float charXPosition, charYPosition;
    private float basicCharXPosition, basicCharYPosition;

    private string line;

    public TextControl myTextController;
    public Character mychar;
    public movingController myMove;

    public void GameData()
    { 
        basket = GameObject.Find("Basket");
        space = GameObject.Find("NumStorage") as GameObject;
        inBoxNum = new GameObject[10];
        TotalBox = new GameObject[30];
        Outbox = GameObject.Find("OutBox") as GameObject;
        myCharacter = GameObject.Find("Character") as GameObject;
        basicCharXPosition = myCharacter.transform.position.x;
        basicCharYPosition = myCharacter.transform.position.y;
    }
    public void Start()
    {
        GameData();
        Inbox = GameObject.Find("InBox") as GameObject;
        charXPosition = myCharacter.transform.position.x;
        charYPosition = myCharacter.transform.position.y;
    }
    public float getBasicCharXPosition()
    {
        return charXPosition;
    }
    public float getBasicCharYPosition()
    {
        return charYPosition;
    }
    public float getCharXPosition()
    {
        return myCharacter.transform.position.x;
    }
    public float getCharYPosition()
    {
        return myCharacter.transform.position.y;
    }
    public void moveCharXPosition(int x, int y)
    {
        myCharacter.transform.Translate(3 * x * y, 0, 0);
    }
    public void moveCharYPosition(int x, int y)
    {
        myCharacter.transform.Translate(0, 3 * x * y, 0);
    }
    public void setSpeed(int v)
    {
        basicSpeed = v;
    }
    public int getSpeed()
    {
        return basicSpeed;
    }
    public void makeInBox(int num)
    {
        TotalBox[TotalBoxCount] = Instantiate(GameObject.Find("Box_Mother")) as GameObject;
        TotalBox[TotalBoxCount].name = "Box_" + TotalBoxCount;
        myTextController.setBoxText(TotalBoxCount, num);
        TotalBox[TotalBoxCount].transform.parent = GameObject.Find("InBox").transform;
        TotalBoxCount++;
    }
    public void makeBox(int num)
    {
        TotalBox[TotalBoxCount] = Instantiate(GameObject.Find("Box_Mother")) as GameObject;
        TotalBox[TotalBoxCount].name = "Box_" + TotalBoxCount;
        myTextController.setBoxText(TotalBoxCount, num);
        TotalBox[TotalBoxCount].transform.parent = GameObject.Find("InBox").transform;
        TotalBoxCount++;
    }

    public void refresh()
    {
        for (int i = 0; i < TotalBoxCount; i++)
            Destroy(TotalBox[i]) ;

        for(int i = 0; i < OutBoxCount; i++)
        {
            Destroy(Outbox.transform.GetChild(i).gameObject);
        }
        Text tmp;
        TotalBoxCount = 0;
        OutBoxCount = 0;
        for(int i = 0; i < 10; i++)
        {
            tmp = basket.transform.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<Text>();
            tmp.text = "";
        }
        myCharacter.transform.position = new Vector2(basicCharXPosition, basicCharYPosition);
        mychar.HAVE = false;
        setSpeed(0);
        myMove.refresh();
    }
    public GameObject getFirstInBox()
    {
        return Inbox.transform.GetChild(0).gameObject;
    }

    public void PlusBoxCnt()
    {
        TotalBoxCount++;
    }
    public void MinusBoxCnt()
    {
        TotalBoxCount--;
    }
    public void PlusOutBoxCnt()
    {
        OutBoxCount++;
    }
    public void MinusOutBoxCnt()
    {
        OutBoxCount--;
    }
    public void setWholeLine(string wholeline)
    {
        line = wholeline;
    }
    public string getWholeLine()
    {
        return line;
    }
}
