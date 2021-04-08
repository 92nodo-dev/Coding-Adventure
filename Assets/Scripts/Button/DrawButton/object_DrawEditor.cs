using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
public class object_DrawEditor : MonoBehaviour {


    public Color drawColor = Color.black;

    Texture2D t2D;
    Vector2 mousePos = new Vector2();
    RectTransform rect;
    int width = 0;
    int height = 0;

    Vector2 previous_drag_position;
    Color[] clean_colours_array;
    Color transparent;
    Color32[] cur_colors;
    bool mouse_was_previously_held_down = false;
    bool no_drawing_on_current_drag = false;

    GameObject remark;
    RawImage remark_RawImage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool mouse_held_down = Input.GetMouseButton(0);
        if (mouse_held_down)
        {
            float x = 960.0f;
            float y = 540.0f;

            //범위 내에 있어야지만 그려진다.
            if (Input.mousePosition.x < (x + 240) && Input.mousePosition.x > (x - 235))
            {
                if (Input.mousePosition.y < (y + 240) && Input.mousePosition.y > (y - 230))
                {
                    //SetPixelControl((int)Input.mousePosition.x + 790, (int)Input.mousePosition.y + 700, drawColor);
                    Vector2 pos1 = new Vector2((int)Input.mousePosition.x + 790, (int)Input.mousePosition.y + 700);
                    ChangeColourAtPoint(pos1, drawColor);
                }
                else
                {
                    // We're not over our destination texture
                    previous_drag_position = Vector2.zero;
                    if (!mouse_was_previously_held_down)
                    {
                        // This is a new drag where the user is left clicking off the canvas
                        // Ensure no drawing happens until a new drag is started
                        no_drawing_on_current_drag = true;
                    }
                }
            }
            else
            {
                // We're not over our destination texture
                previous_drag_position = Vector2.zero;
                if (!mouse_was_previously_held_down)
                {
                    // This is a new drag where the user is left clicking off the canvas
                    // Ensure no drawing happens until a new drag is started
                    no_drawing_on_current_drag = true;
                }
            }
        }
        else if (!mouse_held_down)
        {
            previous_drag_position = Vector2.zero;
            no_drawing_on_current_drag = false;
        }
        mouse_was_previously_held_down = mouse_held_down;
    }

    public GameObject remarkN()
    {
        return remark;
    }


    //public void SetPixelControl(int x, int y, Color c)
    //{
    //    t2D.SetPixel(x, y, c);

    //    t2D.Apply();
    //}

    public void ChangeColourAtPoint(Vector2 world_point, Color color)
    {
        Vector2 pixel_pos = new Vector2(Mathf.RoundToInt(world_point.x), Mathf.RoundToInt(world_point.y));

        if (previous_drag_position == Vector2.zero)
        {
            SetPixelControl((int)world_point.x, (int)world_point.y, color);
        }
        else
        {
            //print("dddddd");
            ColourBetween(previous_drag_position, pixel_pos);
            //t2D.Apply();
        }

        previous_drag_position = pixel_pos;

        t2D.Apply();
    }

    public void ColourBetween(Vector2 start_point, Vector2 end_point)
    {
        // Get the distance from start to finish
        float distance = Vector2.Distance(start_point, end_point);
        Vector2 direction = (start_point - end_point).normalized;

        Vector2 cur_position = start_point;

        // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps)
        {
            cur_position = Vector2.Lerp(start_point, end_point, lerp);
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    t2D.SetPixel((int)cur_position.x + i, (int)cur_position.y + j, drawColor);
                }
            }
            
        }
    }

    public void SetPixelControl(int x, int y, Color c)
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -10; j < 10; j++)
            {
                t2D.SetPixel(x + i, y + j, c);
            }
        }

        //for (int i = -5; i < 5; i++)
        //{
        //    for (int j = -15; j < -10; j++)
        //    {
        //        t2D.SetPixel(x + i, y + j, c);
        //    }
        //}

        //for (int i = -5; i < 5; i++)
        //{
        //    for (int j = 10; j < 15; j++)
        //    {
        //        t2D.SetPixel(x + i, y + j, c);
        //    }
        //}

        //for (int i = -10; i < 10; i++)
        //{
        //    for (int j = -10; j < 10; j++)
        //    {
        //        t2D.SetPixel(x + i, y + j, c);
        //    }
        //}

        //for (int i = -15; i < -10; i++)
        //{
        //    for (int j = -5; j < 5; j++)
        //    {
        //        t2D.SetPixel(x + i, y + j, c);
        //    }
        //}

        //for (int i = 10; i < 15; i++)
        //{
        //    for (int j = -5; j < 5; j++)
        //    {
        //        t2D.SetPixel(x + i, y + j, c);
        //    }
        //}

        t2D.Apply();
    }

    // 숫자이미지로 다시 되돌릴때 사용된다. object_DrawBtn에서 시작
    public void returnImage()
    {
        string getNum = remark.name;
        int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        String SpriteName = "block" + number;

        // 파일 불러 오기
        Texture2D texture1 = new Texture2D(59, 59);
        byte[] bytes1 = File.ReadAllBytes(dirPath + SpriteName + ".png");
        if ((bytes1.Length > 0))
        {
            //print("일단 성공");
            texture1.LoadImage(bytes1);
        }

        Rect rect = new Rect(0, 0, texture1.width, texture1.height);
        remark_RawImage.transform.GetComponent<RawImage>().texture = texture1;
        remark_RawImage.transform.GetComponent<RawImage>().texture.name = SpriteName;
    }

    public void SetGameObject(GameObject g)
    {
        remark = g;

        remark_RawImage = remark.transform.GetChild(1).GetComponent<RawImage>();

        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = remark_RawImage.texture;
        rect = rawImage.GetComponent<RectTransform>();

        width = (int)rect.rect.width;
        height = (int)rect.rect.height;

        var srcT2D = rawImage.texture as Texture2D;

        t2D = new Texture2D(srcT2D.width, srcT2D.height);
        t2D.filterMode = FilterMode.Point;
        t2D.SetPixels32(srcT2D.GetPixels32());
        t2D.Apply();

        rawImage.texture = t2D;

        var pixelData = t2D.GetPixels();

        //print("Total pixels " + pixelData.Length);

        var colorIndex = new List<Color>();

        var total = pixelData.Length;

        for (var i = 0; i < total; i++)
        {

            var color = pixelData[i];

            if (colorIndex.IndexOf(color) == -1)
            {
                colorIndex.Add(color);
            }

        }
    }

    public void Save()
    {
        String rawName = remark_RawImage.GetComponent<RawImage>().texture.name;
        remark_RawImage.GetComponent<RawImage>().texture = t2D;
        remark_RawImage.GetComponent<RawImage>().texture.name = rawName;

        // 폴더에 저장
        var bytes = t2D.EncodeToPNG();

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/ArrayRemarkList/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllBytes(dirPath + rawName + ".png", bytes);

        // 저장 후 블록코딩도구에 적용시키기
        GameObject content = GameObject.Find("Content");
        int ContentChildNum = content.transform.childCount;    // Content의 자식 갯수
        print(ContentChildNum);
        for (int i = 0; i < ContentChildNum; i++)
        {
            GameObject Cell = content.transform.GetChild(i).gameObject;
            if (Cell.transform.childCount == 1)
            {
                GameObject block = Cell.transform.GetChild(0).gameObject;
                if (block.transform.childCount > 0)
                {
                    GameObject Mode = block.transform.GetChild(0).gameObject;
                    GameObject numBlock = Mode.transform.GetChild(0).gameObject;
                    string modeN = Mode.GetComponent<Image>().sprite.name;
                    string numBlockName = "no";
                    if (numBlock.GetComponent<Image>().sprite != null)
                       numBlockName = numBlock.GetComponent<Image>().sprite.name;

                    if (numBlockName != "no")
                    {

                        string getNum = remark.name;
                        int number = int.Parse(getNum.Substring(5));    // ArrayN에서 N의 값을 알아낸다.

                        int numBlockNumber = int.Parse(numBlockName.Substring(5));

                        if (modeN == "ArrayMode" || modeN == "PointMode")
                        {
                            // Array에 이미지가  clear시 이미지로 바꿔줘야한다.
                            if (checkClear() == false)
                            {
                                if (number == numBlockNumber)
                                {
                                    Texture2D texture1 = new Texture2D(59, 59);
                                    texture1 = remark_RawImage.GetComponent<RawImage>().texture as Texture2D;

                                    Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);

                                    //String spriteName = RawImage.GetComponent<RawImage>().texture.name;
                                    numBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                                    numBlock.GetComponent<Image>().sprite.name = "block" + number;
                                }
                            }
                            else
                            {
                                if (number == numBlockNumber)
                                {
                                    var dirPath1 = Application.dataPath + "/../Assets/Resources/Sprites/ArrayNumberList/";

                                    if (!Directory.Exists(dirPath1))
                                    {
                                        Directory.CreateDirectory(dirPath1);
                                    }

                                    String SpriteName1 = "block" + number;

                                    // 파일 불러 오기
                                    Texture2D texture1 = new Texture2D(59, 59);
                                    byte[] bytes1 = File.ReadAllBytes(dirPath1 + SpriteName1 + ".png");
                                    if ((bytes1.Length > 0))
                                    {
                                        //print("일단 성공");
                                        texture1.LoadImage(bytes1);
                                    }

                                    Rect rect1 = new Rect(0, 0, texture1.width, texture1.height);
                                    numBlock.GetComponent<Image>().sprite = Sprite.Create(texture1, rect1, new Vector2(0.5f, 0.5f));
                                    numBlock.GetComponent<Image>().sprite.name = "block" + number;
                                }
                            }
                        }
                    }

                }
            }
        }


    }

    public void Clear()
    {

        var pixelData = t2D.GetPixels();

        var total = pixelData.Length;

        for (var i = 0; i < total; i++)
        {

            pixelData[i] = Color.white;

        }

        t2D.SetPixels(pixelData);
        t2D.Apply();

    }

    public bool checkClear()
    {
        string getNum = remark.name;
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
            //pixelData[i] = Color.white;

        }

        return different;
    }
}
