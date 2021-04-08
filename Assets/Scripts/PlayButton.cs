using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayButton : MonoBehaviour {

    public Text result;
    public GameObject Block_Inventory;

    public GameDataManager mydata;
    public Debuging oneGame;
    public movingController myMove;

    public void OnClick()
    {
        mydata.setSpeed(2);
        result.text = "";
        string line = "";
        GameObject root = GameObject.Find("Content");

        int inventoryCount = 0;     //Inventory에 있는 Block의 수            //임시-> Task Inventory에 직접 접근하여 얻을 수 있음
        for (int i = 0; i < Block_Inventory.transform.childCount; i++)
        {
            GameObject Cell = Block_Inventory.transform.GetChild(i).gameObject;
            GameObject block = Cell.transform.GetChild(0).gameObject;

            if (Cell.active)
            {
                if (block.name == "ForBlock" || block.name == "IfBlock")
                {
                    inventoryCount++;
                }

                inventoryCount++;
            }
        }
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        int cellCount = blocks.Length - inventoryCount;
        Debug.Log(cellCount + "  " + blocks.Length + "  " + inventoryCount);

        for (int i = 0; i < cellCount; i++)
        {
            Transform oneBlock = root.transform.GetChild(i);
            Transform inside = oneBlock.transform.GetChild(0);

            if (inside.name == "RemarkBlock")   //주석이 나올 때 무시하기
            {
                continue;
            }

            if (inside.transform.childCount == 0)    //숫자가 없는 블록일 때
            {
                line += inside.name + "\n";
            }
            else                                //숫자가 있는 블록일 때
            {
                Transform mode = inside.transform.GetChild(0);
                GameObject gMode = mode.gameObject;
                Transform number = mode.transform.GetChild(0);
                GameObject gNumber = number.gameObject;

                line += inside.name + " " + gMode.GetComponent<Image>().sprite.name;

                try
                {
                    line += " " + gNumber.GetComponent<Image>().sprite.name;   //숫자가 null X
                }
                catch (Exception e)
                {
                    line += " " + gNumber.GetComponent<Image>().sprite;        //숫자가 null O
                }

                if (inside.name == "IfBlock")
                {
                    Transform option = inside.transform.GetChild(1);
                    line += " " + option.gameObject.GetComponent<Image>().sprite.name;  //If문에서의 조건을 확인(ComboIcon)
                }
                line += "\n";
            }
        }
        result.text += line;

        oneGame = new Debuging(line);
        if (oneGame.getPairingCheck() && oneGame.getScopeCheck() && oneGame.getOrderingCheck())        //블럭들이 제대로 배치되었는지 확인 (pairing error check)
        {
            oneGame.run();
            Debug.Log("IN : " + oneGame.getInBox());
            Debug.Log("Array : " + oneGame.getArrayBox());
            Debug.Log("OUT : " + oneGame.getOutBox());
            Debug.Log("myValue : " + oneGame.getMyValue());
            Debug.Log("wholeLine : " + oneGame.wholeLine);

            mydata.setWholeLine(oneGame.wholeLine);
            myMove.Play();
        }
        else
        {
            Debug.Log("Incorrect Block Placement");
            if (!oneGame.getPairingCheck())
                Debug.Log("--Pairing Error--");
            else if (!oneGame.getScopeCheck())
                Debug.Log("--Scope Error--");
            else if (!oneGame.getOrderingCheck())
                Debug.Log("--Ordering Error--");
        }


    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
