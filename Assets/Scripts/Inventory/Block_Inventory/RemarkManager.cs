using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemarkManager : MonoBehaviour {

    GridLayoutGroup glg;
    GameObject cell;
    GameObject Content;

    GameObject DrawOnManger;    // 드로우 window
    GameObject DrawOn;  // 그림판
    public void onClick()
    {
        cell = this.transform.parent.gameObject;
        Content = cell.transform.parent.gameObject;

        if (Content.name == "Content")
        {
            DrawOnManger = GameObject.Find("Canvas");
            DrawOnManger = DrawOnManger.transform.GetComponent<DrawManager>().getDrawOnCanvas();
            DrawOnManger.SetActive(true);

            DrawOn = GameObject.Find("DrawOn");
            DrawOn.GetComponent<DrawEditor>().SetGameObject(this.gameObject);

            //Sprite i = this.GetComponent<Image>().sprite;
            //Debug.Log(i.name);
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //if (inventoryCheck())
        //{
        //    Vector2 s = new Vector2(400, 80);
        //    glg = cell.transform.GetComponent<GridLayoutGroup>();
        //    glg.cellSize = s;
        //}
	}

    // taskinventory에 있을경우 glg 값을 조정한다.
    public bool inventoryCheck()
    {
        cell = this.transform.parent.gameObject;
        Content = cell.transform.parent.gameObject;

        if (Content.name == "Content")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
