using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlockNotify : MonoBehaviour
{
    GameObject Canvas;
    GameObject DrawOnManger_object;    // 드로우 WIndow
    GameObject DrawOn_object;  // 그림판

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickNotify()
    {
        // 현재 하이라이트가 켜져있는지 확인한다.
        GameObject highlight = GameObject.Find("Canvas");
        bool ch = highlight.GetComponent<HighLightManager>().getCurrentHighLight();

        GameObject dw = null, dwo = null;
        dw = GameObject.Find("DrawWindow");
        dwo = GameObject.Find("DrawWindow_Object");

        if (dw == null && dwo == null)  // 창이 안켜져 있을때만 버튼을 누를수 있다.
        {

            if (ch)
            {
                StartCoroutine(NotifyClick(this.gameObject));
            }
            else // 하이라이트가 켜져있지 않으면 object_remark가 뜬다.
            {
                Canvas = GameObject.Find("Canvas");
                DrawOnManger_object = Canvas.transform.GetComponent<object_DrawManager>().getDrawOnCanvas();
                //DrawOnManger_object.SetActive(true);
                DrawOnManger_object.SetActive(true);
                DrawOn_object = GameObject.Find("DrawOn_object");
                // DrawOn_object.GetComponent<object_DrawEditor>().SetGameObject(this.gameObject);

                //Sprite i = this.GetComponent<Image>().sprite;
                //Debug.Log(i.name);



                Canvas.GetComponent<object_DrawManager>().LoadRemarkImage(this.gameObject);

                //print(DrawOn_object.name);
                DrawOn_object.GetComponent<object_DrawEditor>().SetGameObject(this.gameObject);
            }

        }
    }

    private IEnumerator NotifyClick(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getGameBlock", g);   // NumberManager
    }
}
