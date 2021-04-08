using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class object_DrawManager : MonoBehaviour {

    GameObject DrawOnManager;

    int count = 0;  // 주석 블록 생성 갯수
    GameObject remarkObject;    // 주석 블록
    public Texture texture;  // 주석 이미지를 복사할 원본 이미지

    // 이미지 초기화 
    Texture2D t2D;
    //Vector2 mousePos = new Vector2();
    //RectTransform rect;
    //int width = 0;
    //int height = 0;

    // Use this for initialization
    void Start()
    {
        DrawOnManager = GameObject.Find("DrawWindow_Object");
        DrawOnManager.transform.position = new Vector2(960, 540 - 53);
        DrawOnManager.SetActive(false); // 드로우 창을 

        //print(texture.width + " " + texture.height);
        //width = texture.width;
        //height = texture.height;

        Texture2D srcT2D = texture as Texture2D;
        t2D = new Texture2D(srcT2D.width, srcT2D.height);
        t2D.filterMode = FilterMode.Point;
        t2D.SetPixels32(srcT2D.GetPixels32());
        t2D.Apply();
    }


    public GameObject getDrawOnCanvas()
    {
        return DrawOnManager;
    }

    public void LoadRemarkImage(GameObject g)
    {
        remarkObject = g; // 주석 블록을 받았으면 이미지를 생성하여 
        GameObject rawImage = remarkObject.transform.GetChild(1).gameObject;
       // Debug.Log("ddd");
        string getNum = remarkObject.name;
        int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.

        //var bytes = t2D.EncodeToPNG();

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        //String fileName = "remark_" + count.ToString() + ".png";
        String SpriteName = "object_remark_" + number;
        //File.WriteAllBytes(dirPath + SpriteName + ".png", bytes);
       // count++;

        // 여기까지 파일을 생성

        //print(count);
        //GameObject image = GameObject.Find("RawImage2").gameObject;
        //print(t2D.name);

        // 파일 불러 오기
        Texture2D texture1 = new Texture2D(500, 500);
        byte[] bytes1 = File.ReadAllBytes(dirPath + SpriteName + ".png");
        if ((bytes1.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes1);
        }

        Rect rect = new Rect(0, 0, texture1.width, texture1.height);

        rawImage.transform.GetComponent<RawImage>().texture = texture1;
        rawImage.transform.GetComponent<RawImage>().texture.name = SpriteName;

        
        //rawImage.transform.position = new Vector2(rawImage.transform.position.x + 31.9f, rawImage.transform.position.y);
        //rawImage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 33);
        //rawImage.transform.GetComponent<RawImage>().texture.height = 33;
    }


    public void CloseWindow()
    {
        DrawOnManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
