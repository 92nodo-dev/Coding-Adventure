using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Character : MonoBehaviour {
    public GameDataManager mydata;
    public movingController myMove;

    public GameObject charBasket;
    public GameObject mybasket;
    public GameObject child;
    public bool isCarrying = false;
    public bool HAVE = false;
    public float tempX, tempY;
    void Start()
    {
        charBasket = GameObject.Find("miniBasket") as GameObject;
        mybasket = GameObject.Find("Basket") as GameObject;
    }

    // temp 방향으로 이동
    public void moveToObject(GameObject temp) 
    {
        tempX = mydata.getCharXPosition();
        tempY = mydata.getCharYPosition();
        if ((temp.transform.position.y - mydata.getCharYPosition()) > 10)
            mydata.moveCharYPosition(mydata.getSpeed(), 1);
        else if ((mydata.getCharYPosition() - temp.transform.position.y) > 10)
            mydata.moveCharYPosition(mydata.getSpeed(), -1);
        if ((temp.transform.position.x - mydata.getCharXPosition()) > 10)
            mydata.moveCharXPosition(mydata.getSpeed(), 1);
        else if ((mydata.getCharXPosition() - temp.transform.position.x) > 10)
            mydata.moveCharXPosition(mydata.getSpeed(), -1);

        if ((tempX == mydata.getCharXPosition()) && (tempY == mydata.getCharYPosition()) && (myMove.getCheckNum() > 500) && (myMove.getCheckNum() < 1000)) myMove.setCheckNum(9990);
        else if ((tempX == mydata.getCharXPosition()) && (tempY == mydata.getCharYPosition()) && (myMove.getCheckNum() > 1000) && (myMove.getCheckNum() < 1500)) myMove.setCheckNum(9990);

    }
    public void moveToArrObject(GameObject temp)
    {
        if ((temp.transform.position.y - mydata.getCharYPosition()) > 10)
            mydata.moveCharYPosition(mydata.getSpeed(), 1);
        else if ((mydata.getCharYPosition() - temp.transform.position.y) > 10)
            mydata.moveCharYPosition(mydata.getSpeed(), -1);
        if ((temp.transform.position.x - mydata.getCharXPosition()) > 10)
            mydata.moveCharXPosition(mydata.getSpeed(), 1);
        else if ((mydata.getCharXPosition() - temp.transform.position.x) > 10)
            mydata.moveCharXPosition(mydata.getSpeed(), -1);

        if ((tempX == mydata.getCharXPosition()) && (tempY == mydata.getCharYPosition()) && (myMove.getCheckNum() > 2000) && (myMove.getCheckNum() < 2500)) myMove.setCheckNum(9980);
        else if ((tempX == mydata.getCharXPosition()) && (tempY == mydata.getCharYPosition()) && (myMove.getCheckNum() > 2500) && (myMove.getCheckNum() < 3000)) myMove.setCheckNum(9980);
        tempX = mydata.getCharXPosition();
        tempY = mydata.getCharYPosition();
    }
    // 캐릭터가 temp를 집어듬
    void pickUp(GameObject temp)
    {
        if (HAVE == true)
        {
            child = charBasket.transform.GetChild(0).gameObject;
            Destroy(child);
        }
        temp.transform.SetParent(charBasket.transform);
        isCarrying = true;
        myMove.setCheckNum(10000);
        HAVE = true;
    }
    void ArrpickUp(GameObject temp)
    {
        Text mytext = temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        Text chartext = charBasket.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>();
        chartext.text = mytext.text;
        chartext.name = mytext.text;
        isCarrying = true;
        HAVE = true;
        myMove.setCheckNum(10000);
    }
    //캐릭터 원위치
    void moveBack(GameObject temp)
    {
        if (mydata.getBasicCharXPosition() > mydata.getCharXPosition())         mydata.moveCharXPosition(mydata.getSpeed(), 1);
        else if (mydata.getBasicCharXPosition() < mydata.getCharXPosition())   mydata.moveCharXPosition(mydata.getSpeed(), -1);
        if (mydata.getBasicCharYPosition() > mydata.getCharYPosition())         mydata.moveCharYPosition(mydata.getSpeed(), 1);
        else if (mydata.getBasicCharYPosition() < mydata.getCharYPosition())    mydata.moveCharYPosition(mydata.getSpeed(), -1);
        if ((tempX == mydata.getCharXPosition()) && (tempY == mydata.getCharYPosition()) && (myMove.getCheckNum() > 1000) && (myMove.getCheckNum() < 1500)) myMove.setCheckNum(9990);
        tempX = mydata.getCharXPosition();
        tempY = mydata.getCharYPosition();
    }

    // array에 가지고 있는 변수를 복사
    public void put(GameObject temp, GameObject temp2)
    {
        GameObject myObj = Instantiate(temp) as GameObject;
        mydata.PlusOutBoxCnt();
        myObj.name = temp.name;
        myObj.transform.SetParent(temp2.transform);
        isCarrying = false;
        myMove.setCheckNum(10000);
    }

    // temp쪽으로 가서 집어드는 거까지
    public void InButton(GameObject temp)
    {
        moveToObject(temp);
        int check = 0;
        if ((mydata.getCharXPosition() > temp.transform.position.x) && (mydata.getCharYPosition() > temp.transform.position.y))
            check = 1;
        if ((mydata.getCharXPosition() > temp.transform.position.x) && (mydata.getCharYPosition() < temp.transform.position.y))
            check = 2;
        if ((mydata.getCharXPosition() < temp.transform.position.x) && (mydata.getCharYPosition() > temp.transform.position.y))
            check = 3;
        if ((mydata.getCharXPosition() < temp.transform.position.x) && (mydata.getCharYPosition() < temp.transform.position.y))
            check = 4;
        switch (check)
        {
            case 1:
                if ((mydata.getCharXPosition() - temp.transform.position.x < 15) && (mydata.getCharYPosition() - temp.transform.position.y < 15))
                    pickUp(temp);
                break;
            case 2:
                if ((mydata.getCharXPosition() - temp.transform.position.x < 15) && (temp.transform.position.y - mydata.getCharYPosition() < 15))
                    pickUp(temp);
                break;
            case 3:
                if ((temp.transform.position.x - mydata.getCharXPosition() < 15) && (mydata.getCharYPosition() - temp.transform.position.y < 15))
                    pickUp(temp);
                break;
            case 4:
                if ((temp.transform.position.x - mydata.getCharXPosition() < 15) && (temp.transform.position.y - mydata.getCharYPosition() < 15))
                    pickUp(temp);
                break;
        }

    }

    //들고있는 객체를 OutBox로 옮김. : 원래 put까지 처리해야하지만, 자꾸 에러가 떠서 put을 Debuging 내에서 checkNum == 9990인 경우로 처리해버림.
    public void OutButton(GameObject temp)
    {
        moveToObject(temp);
    }

    //array에 집어넣는 함수 : 원래 put까지 처리해야하지만, 자꾸 에러가 떠서 put을 Debuging 내에서 checkNum == 9990인 경우로 처리해버림.
    public void ArrayInButton(GameObject temp)
    {
        moveToObject(temp);
    }

    //InButton과 같은 동작
    public void ArrayOutButton(GameObject temp)
    {
        int check = 0;
        if ((mydata.getCharXPosition() > temp.transform.position.x) && (mydata.getCharYPosition() > temp.transform.position.y))
            check = 1;
        if ((mydata.getCharXPosition() > temp.transform.position.x) && (mydata.getCharYPosition() < temp.transform.position.y))
            check = 2;
        if ((mydata.getCharXPosition() < temp.transform.position.x) && (mydata.getCharYPosition() > temp.transform.position.y))
            check = 3;
        if ((mydata.getCharXPosition() < temp.transform.position.x) && (mydata.getCharYPosition() < temp.transform.position.y))
            check = 4;
        moveToObject(temp);
        switch (check)
        {
            case 1:
                if ((mydata.getCharXPosition() - temp.transform.position.x < 15) && (mydata.getCharYPosition() - temp.transform.position.y < 15))
                    ArrpickUp(temp);
                break;
            case 2:
                if ((mydata.getCharXPosition() - temp.transform.position.x < 15) && (temp.transform.position.y - mydata.getCharYPosition() < 15))
                    ArrpickUp(temp);
                break;
            case 3:
                if ((temp.transform.position.x - mydata.getCharXPosition() < 15) && (mydata.getCharYPosition() - temp.transform.position.y < 15))
                    ArrpickUp(temp);
                break;
            case 4:
                if ((temp.transform.position.x - mydata.getCharXPosition() < 15) && (temp.transform.position.y - mydata.getCharYPosition() < 15))
                    ArrpickUp(temp);
                break;
        }
    }
    public void ArrAS(GameObject temp)
    {
        moveToArrObject(temp);
    }
}
