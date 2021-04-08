using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DrawEditor : MonoBehaviour {

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

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        bool mouse_held_down = Input.GetMouseButton(0);
        if (mouse_held_down)
        {
            float x = 960.0f;
            float y = 540.0f;

            // 범위 내에 있어야지만 그려진다.
            if (Input.mousePosition.x < (x + 590) && Input.mousePosition.x > (x - 585))
            {
                if (Input.mousePosition.y < (y + 110) && Input.mousePosition.y > (y - 105))
                {
                    //print("Mouse " + Input.mousePosition.x + "," + Input.mousePosition.y);

                    //SetPixelControl((int)Input.mousePosition.x + 845, (int)Input.mousePosition.y + 530, new Color(drawColor.r, drawColor.g, drawColor.b));
                    Vector2 pos1 = new Vector2((int)Input.mousePosition.x + 845, (int)Input.mousePosition.y + 530);
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


    public void SetGameObject(GameObject g)
    {
        remark = g;

        //Image spriteImage = remark.GetComponent<Image>();
        //rect = spriteImage.GetComponent<RectTransform>();

        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = remark.GetComponent<Image>().sprite.texture;
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
        Rect rect1 = new Rect(0, 0, t2D.width, t2D.height);

        String spriteName = remark.GetComponent<Image>().sprite.name;
        remark.GetComponent<Image>().sprite = Sprite.Create(t2D, rect1, new Vector2(0.5f, 0.5f));
        remark.GetComponent<Image>().sprite.name = spriteName;

        // 폴더에 저장
        var bytes = t2D.EncodeToPNG();

        var dirPath = Application.dataPath + "/../Assets/Resources/Sprites/Inventory/RemarkPNG/";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllBytes(dirPath + spriteName + ".png", bytes);

       // GameObject image = GameObject.Find("RawImage2").gameObject;
        //print(t2D.name);
        //image.transform.GetComponent<RawImage>().texture = t2D;
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
}
