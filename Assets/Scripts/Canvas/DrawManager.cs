using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DrawManager : MonoBehaviour {

    GameObject DrawOnManager;

    int count = 0;  // 주석 블록 생성 갯수
    GameObject remarkObject;    // 주석 블록
    public Texture texture;  // 주석 이미지를 복사할 원본 이미지

    // 이미지 초기화 
    Texture2D t2D;
    //Vector2 mousePos = new Vector2();
    //RectTransform rect;
    int width = 0;
    int height = 0;

    // Use this for initialization
    void Start () {
        DrawOnManager = GameObject.Find("DrawWindow");
        DrawOnManager.transform.position = new Vector2(960, 540-49);
        DrawOnManager.SetActive(false); // 드로우 창을 

        //print(texture.width + " " + texture.height);
        width = texture.width;
        height = texture.height;

        Texture2D srcT2D = texture as Texture2D;
        t2D = new Texture2D(srcT2D.width, srcT2D.height);
        t2D.filterMode = FilterMode.Point;
        t2D.SetPixels32(srcT2D.GetPixels32());
        t2D.Apply();
    }

    public void CountingRemark(int num)
    {
        count = num + 1;
    }

    public GameObject getDrawOnCanvas()
    {
        return DrawOnManager;
    }

    public void CreateRemarkImage(GameObject g)
    {
        remarkObject = g; // 주석 블록을 받았으면 이미지를 생성하여 

        var bytes = t2D.EncodeToPNG();

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/Inventory/RemarkPNG/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        //String fileName = "remark_" + count.ToString() + ".png";
        String SpriteName = "remark_" + count.ToString();
        File.WriteAllBytes(dirPath + SpriteName + ".png", bytes);
        count++;

        // 여기까지 파일을 생성

        //print(count);
        //GameObject image = GameObject.Find("RawImage2").gameObject;
        //print(t2D.name);

        // 파일 불러 오기
        Texture2D texture1 = new Texture2D(1200, 240);
        byte[] bytes1 = File.ReadAllBytes(dirPath + SpriteName + ".png");
        if ((bytes1.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes1);
        }

        Rect rect = new Rect(0, 0, texture1.width, texture1.height);
        remarkObject.transform.GetComponent<Image>().sprite = Sprite.Create(texture1, rect, new Vector2(0.5f, 0.5f));
        remarkObject.transform.GetComponent<Image>().sprite.name = SpriteName;
    }

    public void CloseWindow()
    {
        DrawOnManager.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
