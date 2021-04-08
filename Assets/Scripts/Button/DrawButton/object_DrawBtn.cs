using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class object_DrawBtn : MonoBehaviour {

    GameObject Canvas;

    //GameObject b;
    // Use this for initialization
    void Start()
    {
        Canvas = GameObject.Find("Canvas");


    }

    public void OnHome()
    {
        // 창이 모두 흰색이면 다시 숫자 이미지로 돌아간다.
        GameObject g = GameObject.Find("DrawOn_object");
        bool isClear = g.GetComponent<object_DrawEditor>().checkClear();    // true일시 이미지로 바꾼다.
        if (isClear)
        {
            print("s");
            g.GetComponent<object_DrawEditor>().returnImage();
        }
        else
        {
            GameObject remark = g.GetComponent<object_DrawEditor>().remarkN();
            RawImage rawImage = remark.transform.GetChild(1).GetComponent<RawImage>();
            int num = int.Parse(remark.name.Substring(5));

            var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
  
            String SpriteName = "object_remark_" + num;


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

        }

        Canvas.GetComponent<object_DrawManager>().CloseWindow();
    }

    public void OnSave()
    {

    }

    public void OnErase()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 좌표가 밖에 있을때 좌표를 하나로 잡는다.


        //b = GameObject.Find("DrawWindow");
        //print(b.transform.position);
    }
}
