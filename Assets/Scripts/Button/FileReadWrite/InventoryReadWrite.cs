using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InventoryReadWrite : MonoBehaviour
{

    GameObject Block_Inventory;
    GameObject Task_Inventory;
    int bCount; // blcok_Inventory Count
    int tCount; // Task_Inventory Count
    int count = 0; // counting of Task_Inventory block 

    // Use this for initialization
    void Start()
    {
        // 프로그램 시작할시 Block inventory의 블록들을 모두 감춘다.
        Block_Inventory = GameObject.Find("Block_Inventory");
        Task_Inventory = GameObject.Find("Task_Inventory");

        bCount = Block_Inventory.transform.childCount;
        GameObject bCell;
        for (int i = 0; i < bCount; i++)
        {
            bCell = Block_Inventory.transform.GetChild(i).gameObject;
            bCell.SetActive(false);
        }
        test();
    }

    public void test()
    {
        LoadBlockInventory("InBlock");
        LoadBlockInventory("OutBlock");
        LoadBlockInventory("ForBlock");
        LoadBlockInventory("IfBlock");
        LoadBlockInventory("ContainerInBlock");
        LoadBlockInventory("ContainerOutBlock");
        LoadBlockInventory("AddBlock");
        LoadBlockInventory("SubBlock");
        LoadBlockInventory("BreakBlock");
        LoadBlockInventory("IncrementBlock");
        LoadBlockInventory("DecrementBlock");
        LoadBlockInventory("RemarkBlock");

        //getBlockCellCount();
    }

    public void getBlockCellCount()
    {
        int a_count = 0;

        for (int i = 0; i < Block_Inventory.transform.childCount; i++)
        {
            GameObject Cell = Block_Inventory.transform.GetChild(i).gameObject;
            GameObject block = Cell.transform.GetChild(0).gameObject;

            if (Cell.active == true)
            {
                if (block.name == "ForBlock" || block.name == "IfBlock")
                {
                    a_count++;
                }

                a_count++;
            }
        }

        Debug.Log("Block_Inventory : " + a_count + "개");
    }

    public void LoadTest()
    {
        //LoadTaskInventory("ForBlock", "ArrayMode", "OneBlock");
        //LoadTaskInventory("InBlock");
        // LoadTaskInventory("InBlock");
        // LoadTaskInventory("ForBlock", "ArrayMode", "block1") // ArrayMode에 따라 이미지가 바뀌여야함
        // LoadTaskInventory("ForBlock", "ArrayMode", "block1", "item1") // ArrayMode에 따라 이미지가 바뀌여야함
        // LoadTaskInventory("RemarkBlock", "remark_3")
        LoadTaskInventory("ForBlock", "BasicMode", "null");
        LoadTaskInventory("ForBlock", "BasicMode", "block2");
        LoadTaskInventory("AddBlock", "ArrayMode", "block0");
        LoadTaskInventory("SubBlock", "PointMode", "block2");
        //LoadTaskInventory("BreakBlock");
        LoadTaskInventory("RemarkBlock", "remark_1");
        LoadTaskInventory("IfBlock", "PointMode", "block4", "item1");
        LoadTaskInventory("IfBlock", "PointMode", "block2", "item1");
        //LoadTaskInventory("IfEndBlock");
        count = 0;
    }

    // Task_Inventory의 block을 Load한다.
    // Block_Inventory가 Load되었다고 가정하에 한다.
    public void LoadTaskInventory(string block)
    {
        int num = Block_Inventory.transform.childCount;

        for (int i = 0; i < num; i++)
        {
            GameObject cell = Block_Inventory.transform.GetChild(i).gameObject;
            GameObject item = cell.transform.GetChild(0).gameObject;
            if (block.Equals(item.name))
            {
                // 블록 load
                // Debug.Log(count);
                GameObject Viewport = Task_Inventory.transform.GetChild(0).gameObject;
                GameObject Content = Viewport.transform.GetChild(0).gameObject;
                GameObject t_cell = Content.transform.GetChild(count).gameObject;
                GameObject t_item = Instantiate(item) as GameObject;
                t_item.transform.SetParent(t_cell.transform, false);
                t_item.name = item.name;
                NotifyLoadBlock();
                count++;
            }
        }
    }

    // remarkBlock
    public void LoadTaskInventory(string block, string objectName)
    {
        if (block == "RemarkBlock")
        {// remark 블록을 생성할시 Counting
            int num1 = int.Parse(objectName.Substring(7));
            GameObject drawManager = GameObject.Find("Canvas");
            drawManager.GetComponent<DrawManager>().CountingRemark(num1);   // 저장한 주석보다 숫자가 높아지게 하여서 주석이미지가 겹치지 않게한다.


            int num = Block_Inventory.transform.childCount;

            for (int i = 0; i < num; i++)
            {
                GameObject cell = Block_Inventory.transform.GetChild(i).gameObject;
                GameObject item = cell.transform.GetChild(0).gameObject;
                if (block.Equals(item.name))
                {
                    // 블록 load
                    GameObject Viewport = Task_Inventory.transform.GetChild(0).gameObject;
                    GameObject Content = Viewport.transform.GetChild(0).gameObject;
                    GameObject t_cell = Content.transform.GetChild(count).gameObject;
                    GameObject t_item = Instantiate(item) as GameObject;
                    t_item.transform.SetParent(t_cell.transform, false);
                    t_item.name = item.name;

                    setRemarkBlock(t_item, objectName);

                    NotifyLoadBlock();
                    count++;
                }
            }
        }
    }

    public void setRemarkBlock(GameObject remark, string remarkNumberName)
    {
        Texture2D texture1 = new Texture2D(59, 59);
        string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/Inventory/RemarkPNG/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        byte[] bytes = File.ReadAllBytes(dirPath + remarkNumberName + ".png");
        if ((bytes.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes);
        }

        Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
        String spriteName = remarkNumberName;
        remark.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
        remark.GetComponent<Image>().sprite.name = spriteName;
    }

    public void LoadTaskInventory(string block, string blockMode, string numberBlock)   // numberBlock은 null일수도 있다.
    {
        int num = Block_Inventory.transform.childCount;

        for (int i = 0; i < num; i++)
        {
            GameObject cell = Block_Inventory.transform.GetChild(i).gameObject;
            GameObject item = cell.transform.GetChild(0).gameObject;
            if (block.Equals(item.name))
            {
                // 블록 load
                GameObject Viewport = Task_Inventory.transform.GetChild(0).gameObject;
                GameObject Content = Viewport.transform.GetChild(0).gameObject;
                GameObject t_cell = Content.transform.GetChild(count).gameObject;
                GameObject t_item = Instantiate(item) as GameObject;
                t_item.transform.SetParent(t_cell.transform, false);
                t_item.name = item.name;

               // blockMode 설정
                GameObject Mode = t_item.transform.GetChild(0).gameObject;
                

                // 숫자 블록 load
                GameObject numBlock = Mode.transform.GetChild(0).gameObject;

                setBlockMode(numBlock, blockMode);
                setSprite(Mode, numBlock, numberBlock);

                NotifyLoadBlock();
                count++;
            }
        }

    }

    public void LoadTaskInventory(string block, string blockMode, string numberBlock, string operatorBlock)
    {
        int num = Block_Inventory.transform.childCount;

        for (int i = 0; i < num; i++)
        {
            GameObject cell = Block_Inventory.transform.GetChild(i).gameObject;
            GameObject item = cell.transform.GetChild(0).gameObject;
            if (block.Equals(item.name))
            {
                // 블록 load
                GameObject Viewport = Task_Inventory.transform.GetChild(0).gameObject;
                GameObject Content = Viewport.transform.GetChild(0).gameObject;
                GameObject t_cell = Content.transform.GetChild(count).gameObject;
                GameObject t_item = Instantiate(item) as GameObject;
                t_item.transform.SetParent(t_cell.transform, false);
                t_item.name = item.name;

                // BlockMode load
                GameObject Mode = t_item.transform.GetChild(0).gameObject;
               

                // 숫자 블록 load
                GameObject numBlock = Mode.transform.GetChild(0).gameObject;

                setBlockMode(numBlock, blockMode);
                setSprite(Mode, numBlock, numberBlock);

                // 산술 연산자 블록 load
                GameObject operBlock = t_item.transform.GetChild(1).gameObject;
                setOperatorSprite(operBlock, operatorBlock);

                NotifyLoadBlock();
                count++;
            }
        }
    }

    // Container Load
    public void LoadContainer()
    {
        GameObject basket = GameObject.Find("Basket");
        int basketChild = basket.transform.childCount;

        for (int i = 0; i < basketChild; i++)
        {
            GameObject arrayN = basket.transform.GetChild(i).gameObject;
            GameObject RawImage = arrayN.transform.GetChild(1).gameObject;
            string blockN = arrayN.transform.GetChild(1).GetComponent<RawImage>().texture.name;

            // 클리어 하면 숫자 이미지 아니면 그림 이미지
            if (checkClear(blockN))
            {
                Texture2D texture1 = new Texture2D(59, 59);
                string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                byte[] bytes = File.ReadAllBytes(dirPath + blockN + ".png");
                if ((bytes.Length > 0))
                {
                    //print("일단 성공");
                    texture1.LoadImage(bytes);
                }
                RawImage.GetComponent<RawImage>().texture = texture1;
                RawImage.GetComponent<RawImage>().texture.name = blockN;
            }
            else
            {
                int numnum = int.Parse(blockN.Substring(5));

                Texture2D texture1 = new Texture2D(59, 59);
                string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                byte[] bytes = File.ReadAllBytes(dirPath + "object_remark_"+ numnum + ".png");
                if ((bytes.Length > 0))
                {
                    //print("일단 성공");
                    texture1.LoadImage(bytes);
                }

                RawImage.GetComponent<RawImage>().texture = texture1;
                RawImage.GetComponent<RawImage>().texture.name = blockN;
            }

        }
    }

    public void NotifyLoadBlock()
    {
        // Debug.Log(this.name);
        StartCoroutine(NotifyCellManger(this.gameObject));
    }

    private IEnumerator NotifyCellManger(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        Task_Inventory.SendMessageUpwards("createCellManager", g);
    }

    public void setOperatorSprite(GameObject OperatorBlock, string o)
    {
        if (o == "item1")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(0);
        }
        else if (o == "item2")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(1);
        }
        else if (o == "item3")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(2);
        }
        else if (o == "item4")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(3);
        }
        else if (o == "item5")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(4);
        }
        else if (o == "item6")
        {
            OperatorBlock.GetComponent<IfBlockClickNotify>().getNumber(5);
        }
    }

    // 숫자 이미지를 numberBlock에 넣는다
    public void LoadBlockN(GameObject numberBlock, string numberName)
    {
        //int num = int.Parse(numberName.Substring(5)); // blockN의 N의 값을 얻는다.

        Texture2D texture1 = new Texture2D(59, 59);
        string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        byte[] bytes = File.ReadAllBytes(dirPath + numberName + ".png");
        if ((bytes.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes);
        }

        Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
        String spriteName = numberName;
        numberBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
        numberBlock.GetComponent<Image>().sprite.name = spriteName;
    }

    // object_remark_N을 불러온다.
    public void LoadObjectRemarkN(GameObject numberBlock, string numberName)
    {
        int num = int.Parse(numberName.Substring(5)); // blockN의 N의 값을 얻는다.


        Texture2D texture1 = new Texture2D(59, 59);
        string dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        byte[] bytes = File.ReadAllBytes(dirPath + "object_remark_"+ num + ".png");
        if ((bytes.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes);
        }

        Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
        String spriteName = numberName;
        numberBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
        numberBlock.GetComponent<Image>().sprite.name = spriteName;
    }

    // blockN 이미지가 클리어 한지 확인
    public bool checkClear(string imageName)
    {
        string getNum = imageName;
        int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        String SpriteName = "object_remark_" + number;

        // 파일 불러 오기
        Texture2D texture1 = new Texture2D(59, 59);
        byte[] bytes1 = File.ReadAllBytes(dirPath + SpriteName + ".png");
        if ((bytes1.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes1);
        }
        bool different = true;

        var pixelData = texture1.GetPixels();

        var total = pixelData.Length;

        for (var i = 0; i < total; i++)
        {
            if (pixelData[i] == Color.white)
            {

            }
            else
            {
                different = false;
            }
        }

        return different;
    }

    public void setSprite(GameObject Mode, GameObject NumberBlock, string NumName)
    {
        string mode = Mode.GetComponent<Image>().sprite.name;

        if (NumName != "null")    // 숫자 블록이 null이면 넘어간다.
        {     
            int num = int.Parse(NumName.Substring(5)); // blockN의 N의 값을 얻는다.

            if (mode == "BasicMode")
            {
                LoadBlockN(NumberBlock, NumName);
            }
            else if (mode == "ArrayMode" || mode == "PointMode")
            {
                // 이미지가 클리어한지 확인 후 
                // 클리어하면 이미지 로드, 아니면 object_remark_ 로드 
                if (checkClear(NumName))
                {
                    LoadBlockN(NumberBlock, NumName);
                }
                else
                {
                    LoadObjectRemarkN(NumberBlock, NumName);
                }

            }
        }
    }

    public void setBlockMode(GameObject inventoryBlock, string s)
    {
        if (s == "BasicMode")
        {
            inventoryBlock.GetComponent<BlockNotify>().getBlockMode(0);
        }
        else if (s == "ArrayMode")
        {
            inventoryBlock.GetComponent<BlockNotify>().getBlockMode(1);
        }
        else if (s == "PointMode")
        {
            inventoryBlock.GetComponent<BlockNotify>().getBlockMode(2);
        }
    }

    // Task_Inventory의 block을 save해주는 함수 
    public void SaveTaskInventory()
    {
        string blockMode = "", numberBlock = "", operatorBlock = "";

        GameObject cell, block, bMode, numBlock, operBlock;

        GameObject Viewport = Task_Inventory.transform.GetChild(0).gameObject;
        GameObject Content = Viewport.transform.GetChild(0).gameObject;
        //GameObject t_cell = Content.transform.GetChild(count).gameObject;
        tCount = Content.transform.childCount;

        for (int i = 0; i < tCount - 12; i++)
        {
            blockMode = "";
            numberBlock = "";
            operatorBlock = "";

            //task invnetory의 cell, block, numblock을 각각 확인한다.
            cell = Content.transform.GetChild(i).gameObject;
            block = cell.transform.GetChild(0).gameObject;

            if (block.transform.childCount == 0)
            { 
                if (block.name == "RemarkBlock")
                {
                    numberBlock = block.GetComponent<Image>().sprite.name;
                }
            }

            // number block을 가지고 있을시
            if (block.transform.childCount == 1)
            {
                bMode = block.transform.GetChild(0).gameObject;
                numBlock = bMode.transform.GetChild(0).gameObject;

                blockMode = bMode.GetComponent<Image>().sprite.name;

                if (numBlock.GetComponent<Image>().sprite != null)
                    numberBlock = numBlock.GetComponent<Image>().sprite.name;
            }

            if (block.transform.childCount == 2)
            {
                bMode = block.transform.GetChild(0).gameObject;
                numBlock = bMode.transform.GetChild(0).gameObject;

                operBlock = block.transform.GetChild(1).gameObject;
                blockMode = bMode.GetComponent<Image>().sprite.name;

                if (numBlock.GetComponent<Image>().sprite != null)
                    numberBlock = numBlock.GetComponent<Image>().sprite.name;

                if (operBlock.GetComponent<Image>().sprite != null)
                    operatorBlock = operBlock.GetComponent<Image>().sprite.name;
            }

            // numberBlock이 null일수도 있다.
            Debug.Log("저장할 정보: " + block.name + " " + blockMode + " " + numberBlock + " " + operatorBlock);
        }

    }

    // Block_Inventory의 block을 enable해주는 함수 
    public void LoadBlockInventory(string block)
    {
        GameObject cell;
        GameObject item;

        for (int i = 0; i < bCount; i++)
        {
            cell = Block_Inventory.transform.GetChild(i).gameObject;
            item = cell.transform.GetChild(0).gameObject;

            if (block.Equals(item.name))
            {
                cell.SetActive(true);

                //Debug.Log("child num ="+ item.name + " " + item.transform.GetChildCount());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
