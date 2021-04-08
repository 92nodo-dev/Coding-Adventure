using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHighLightNotify : MonoBehaviour {

    Vector2 ContentPosition;
	// Use this for initialization
	void Start () {
        GameObject Content = GameObject.Find("Content");
        ContentPosition = Content.transform.position;
	}

    public void onHighLightClick()
    {
        GameObject Cell = this.transform.parent.gameObject;
        GameObject Content = Cell.transform.parent.gameObject;
        GameObject Viewport = Content.transform.parent.gameObject;
        GameObject Task_Inventory = Viewport.transform.parent.gameObject;

        // 블록을 클릭했을때 넘버블록이 있다는걸 알려주기 (하이라이트와 연관)
        GameObject blockMode = null, numberBlock = null;
        if (this.transform.childCount >= 1)
        {
            blockMode = this.transform.GetChild(0).gameObject;
            numberBlock = blockMode.transform.GetChild(0).gameObject;
        }


        if (Task_Inventory.name == "Task_Inventory")
        {
            StartCoroutine(ClickNotify(this.gameObject));

            if (numberBlock != null)
            {
                StartCoroutine(NotifyNumberManager(numberBlock));
            }
            else
            {   // numberblock이 존재하지 않을때도 보내야함
                //numberBlock = Cell;
                //numberBlock.name = "noBlock";
                StartCoroutine(NotifyNumberManager(false));
            }

        }
        // y : 315로 고정하여 

        // 처음 시작을 고정값으로 고정시킨다, 
        // 9번째까지 내리고 
        // 그다음부터 Content값을 90씩 증가시킨다. 
        int BlockCount = Content.transform.childCount - 12;
        int thisCount = 0;
        for (int i = 0; i < BlockCount; i++)
        {
            GameObject Cell1 = Content.transform.GetChild(i).gameObject;
            GameObject Block = Cell1.transform.GetChild(0).gameObject;

            if (this.gameObject == Block)
            {
                thisCount = i;  // 자신이 몇번째인지 알게된다.
                print(thisCount);
            }
        }

        if (thisCount == 0)
        {
            Content.GetComponent<RectTransform>().position = ContentPosition;
        }
        else if (thisCount > 8)
        {
            // 무조건 고정위치를 잡아야한다... 
            // 9번쨰면 +90
            // 10번째면 + 180
            // (n-8)*90

            //ContentPosition.y = ContentPosition.y + 90;
            //Content.GetComponent<RectTransform>().position = ContentPosition;
            //print(Content.transform.position);

           // Vector2 savePosition = Content.GetComponent<RectTransform>().position;
            //savePosition.y += 90;
            Vector2 test = new Vector2(0, ((thisCount - 8) * 90));
            Content.GetComponent<RectTransform>().position = ContentPosition+test;
        }
        else
        {
            Content.GetComponent<RectTransform>().position = ContentPosition;
        }

        

        // print(Content.transform.position); 1460,1080
        //float PosY = Cell.GetComponent<RectTransform>().position.y;
        //print(Cell.transform.position);
        // for블록일 경우 다시 올라가야한다. 

        // Cell의 번지수를 보고 9번째까지는 가만히 있는다 
        // 버튼을 누르면 처음에 맨 위에서 부터 시작한다. 




    }

    public void onClick()
    {
        GameObject Cell = this.transform.parent.gameObject;
        GameObject Content = Cell.transform.parent.gameObject;
        GameObject Viewport = Content.transform.parent.gameObject;
        GameObject Task_Inventory = Viewport.transform.parent.gameObject;

        // 블록을 클릭했을때 넘버블록이 있다는걸 알려주기 (하이라이트와 연관)
        GameObject blockMode = null, numberBlock = null;
        if (this.transform.childCount >= 1)
        {
            blockMode = this.transform.GetChild(0).gameObject;
            numberBlock = blockMode.transform.GetChild(0).gameObject;
        }


        if (Task_Inventory.name == "Task_Inventory")
        {
            StartCoroutine(ClickNotify(this.gameObject));

            if (numberBlock != null)
            {
                StartCoroutine(NotifyNumberManager(numberBlock));
            }
            else
            {   // numberblock이 존재하지 않을때도 보내야함
                //numberBlock = Cell;
                //numberBlock.name = "noBlock";
                StartCoroutine(NotifyNumberManager(false));
            }

        }
    }

    private IEnumerator ClickNotify(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getHighLightBlock", g);  // HighLightManager
    }

    private IEnumerator NotifyNumberManager(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getTaskBlock", g);  // HighLightManager
    }

    private IEnumerator NotifyNumberManager(bool g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getTaskBlock", g);  // HighLightManager
    }

    // Update is called once per frame
    void Update () {
		
	}
}
