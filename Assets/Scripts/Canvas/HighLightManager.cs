using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightManager : MonoBehaviour {

    GameObject gameBlock;   // 하이라이트인 블록
    Color highlight, not_higlight;     // 1. 하이라이트일때 2. 하이라이트가 아니일때

    bool currentHighLight = false;  // 현재 하이라이트가 켜져있는지 확인

	// Use this for initialization
	void Start () {
        gameBlock = null;
        highlight = new Color(146 / 255f, 146 / 255f, 146 / 255f, 146 / 255f);
        not_higlight = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0 / 255f);
    }

    public bool getCurrentHighLight()
    {
        return currentHighLight;
    }

    // 블록이 삭제되면 하이라이트를 false로 바꿔야 한다.
    public void setCurrentHighLight(bool b)
    {
        currentHighLight = b;
    }

    // this.GetComponent<Image>().color = new Color(0/255f, 100/255f, 10/255f, 255/255f);
    // 하이라이트 표시할 블록을 받음 
    public void getHighLightBlock(GameObject g)
    {
        bool isTrue = isHighLight(gameBlock);
        setNotHighLightBlock(gameBlock);    // 하이라이트를 지우고 시작한다. 

        if (gameBlock == g)
        {
            if (isTrue)
            {
                setNotHighLightBlock(gameBlock);
            }
            else
            {
                setHighLightBlock(gameBlock);
            }
        }
        else
        {
            gameBlock = g;

            setHighLightBlock(gameBlock);
        }
    }
    public bool isHighLight(GameObject g)
    {
        if (g != null)
        {
            GameObject Cell = g.transform.parent.gameObject;

            if (Cell.GetComponent<Image>().color == highlight)
            {
                return true;
            }
            else if (Cell.GetComponent<Image>().color == not_higlight)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
        else // g가 널일경우 당연이 하이라이트는 안켜져있다.
        {
            return false;
        }
    }

    public void setNotHighLightBlock(GameObject g) // highlight 제거
    {
        if (g != null)
        {
            GameObject Cell = g.transform.parent.gameObject;
            Cell.GetComponent<Image>().color = not_higlight;
            currentHighLight = false;
        }
    }

    public void setHighLightBlock(GameObject g)     // highlight 표시
    {
        GameObject Cell = g.transform.parent.gameObject;
        Cell.GetComponent<Image>().color = highlight;
        currentHighLight = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentHighLight == true)   // 하이라이트가 켜져있을때
        {
            if (Input.GetMouseButtonDown(0))    // 마우스를 클릭하면 
            {
                if (gameBlock != null)  // 블록이 파괴되면 문제가 생김 
                {
                    Vector3 mouse_p = Input.mousePosition;
                    Vector3 BlockPosition = gameBlock.transform.position;

                    // Vector3 ModeListPosition = ModeList.transform.position;
                    Vector3 s1 = new Vector3();
                    Vector3 s2 = new Vector3();
                    Vector3 s3 = new Vector3();
                    Vector3 s4 = new Vector3();

                    s1.x = BlockPosition.x - 100.0f; s1.y = BlockPosition.y + 40.0f;
                    s2.x = BlockPosition.x + 100.0f; s2.y = BlockPosition.y + 40.0f;
                    s3.x = BlockPosition.x - 100.0f; s3.y = BlockPosition.y - 40.0f;
                    s4.x = BlockPosition.x + 100.0f; s4.y = BlockPosition.y - 40.0f;
                    
                    // basket의 싸이즈
                    GameObject Basket = GameObject.Find("Basket");
                    Vector2 b = Basket.transform.position;
                    float b_width = Basket.GetComponent<RectTransform>().rect.width / 2;
                    float b_Hight = Basket.GetComponent<RectTransform>().rect.height / 2;

                    // Stop 버튼 
                    GameObject Stop = GameObject.Find("Stop");
                    Vector2 st = Stop.transform.position;
                    float s_width = Stop.GetComponent<RectTransform>().rect.width / 2;
                    float s_hight = Stop.GetComponent<RectTransform>().rect.height / 2;


                    if ((mouse_p.x > b.x - b_width) && (mouse_p.x < b.x + b_width) && (mouse_p.y < b.y + b_Hight) && (mouse_p.y > b.y - b_Hight))
                    {
                        // basket 위치 
                    }
                    else if ((mouse_p.x > s1.x) && (mouse_p.x < s2.x) && (mouse_p.y < s1.y) && (mouse_p.y > s3.y))
                    {
                        // 블록 위치 내부
                    }
                    else if ((mouse_p.x > st.x - s_width) && (mouse_p.x < st.x + s_width) && (mouse_p.y < st.y + s_hight) && (mouse_p.y > st.y - s_hight))
                    {
                        // stop 버튼 내부
                    }
                    else
                    {
                        // 블록 위치 밖으로 빠지면 하이라이트가 꺼진다.
                        setNotHighLightBlock(gameBlock);
                    }
                }
            }
        }
	}
}
