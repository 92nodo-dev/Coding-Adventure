using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class NumberManager : MonoBehaviour
{

    GameObject inventoryBlock;      // 인벤토리 블록
    GameObject gameBlock;           // 오브젝트 블록
    bool isInventory = false;
    bool isGameBlock = false;

    // Use this for initialization
    void Start()
    {
        //string s = "Array1";

        //int i = s.Length;
        //Debug.Log(i);
        //string ss = s.Substring(5);
        //Debug.Log(ss);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameBlock == true && isInventory == true)
        {
            // gameBlock의 RawImage를 받아서 InventoryBlock의 이미지에 넣는다.
            GameObject Mode = inventoryBlock.transform.parent.gameObject;
            if (Mode.name == "BlockMode")
            {
                // BasicMode일때는 숫자가 보인다.
                if (Mode.GetComponent<Image>().sprite.name == "BasicMode")
                {
                    string getNum = gameBlock.name;
                    int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.
                    //inventoryBlock.GetComponent<BlockNotify>().getNumber(number);

                    Texture2D texture1 = new Texture2D(59, 59);
                    string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    byte[] bytes = File.ReadAllBytes(dirPath + "block" + number + ".png");
                    if ((bytes.Length > 0))
                    {
                        //print("일단 성공");
                        texture1.LoadImage(bytes);
                    }

                    Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
                    String spriteName = "block" + number;
                    inventoryBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                    inventoryBlock.GetComponent<Image>().sprite.name = spriteName;

                }
                else if (Mode.GetComponent<Image>().sprite.name == "ArrayMode"
                    || Mode.GetComponent<Image>().sprite.name == "PointMode")
                {// BasicMode가 아닐때는 숫자 or 이미지가 전달된다.

                    string getNum = gameBlock.name;
                    int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.
                    GameObject RawImage = gameBlock.transform.GetChild(1).gameObject;

                    Texture2D texture1 = new Texture2D(59, 59);
                    texture1 = RawImage.GetComponent<RawImage>().texture as Texture2D;

                    Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);

                    //String spriteName = RawImage.GetComponent<RawImage>().texture.name;
                    inventoryBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                    inventoryBlock.GetComponent<Image>().sprite.name = "block" + number;

                }

                

                isGameBlock = false;
                isInventory = true;
            }
        }
    }

    // 블록 모드를 바꾸었을때 이미지를 다시 불러온다 
    public void reCall_Image(string mode, GameObject numb)
    {
        string imageName = "no";
        if(numb.GetComponent<Image>().sprite != null)
            imageName = numb.GetComponent<Image>().sprite.name;
       // print(imageName);

        if (imageName != "no")  // number블록이 비어있을수도 있다.
        {
            if (mode == "BasicMode")
            {
                Texture2D texture1 = new Texture2D(59, 59);
                string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                byte[] bytes = File.ReadAllBytes(dirPath + imageName + ".png");
                if ((bytes.Length > 0))
                {
                    //print("일단 성공");
                    texture1.LoadImage(bytes);
                }

                Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
                String spriteName = imageName;
                numb.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                numb.GetComponent<Image>().sprite.name = spriteName;
            }
            else
            {
                //string getNum = numb.name;
                int number = int.Parse(imageName.Substring(5));    // ArrayN에서 N의 값을 알아낸다.

                // RawImage는 number의 숫자에 따라 array블록을 찾아서 ...
                GameObject Basket = GameObject.Find("Basket");
                GameObject ArrayN = Basket.transform.GetChild(number).gameObject;
                GameObject RawImage = ArrayN.transform.GetChild(1).gameObject;

                Texture2D texture1 = new Texture2D(59, 59);
                texture1 = RawImage.GetComponent<RawImage>().texture as Texture2D;

                Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);

                //String spriteName = RawImage.GetComponent<RawImage>().texture.name;
                numb.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                numb.GetComponent<Image>().sprite.name = "block" + number;

            }
        }
    }

    public void getTaskBlock(bool g)
    {
        isInventory = g;
    }

    public void getTaskBlock(GameObject g)
    {
        inventoryBlock = g; // numberBlock

        isInventory = true;
    }

    public void getGameBlock(GameObject g)
    {
        gameBlock = g;

        // 현재 하이라이트가 켜져있는지 확인한다.
        GameObject highlight = GameObject.Find("Canvas");
        bool ch = highlight.GetComponent<HighLightManager>().getCurrentHighLight();

        // TaskInventory가 먼저 클릭되지 않는다면 게임블록은 클릭되지 않는다.
        // 블록의 하이라이트는 블록을 삭제시 false로 바뀐다,
        // 
        if (isInventory == true && ch == true)
        {
            isGameBlock = true;
        }
            //Debug.Log(g.name);
    }
}
