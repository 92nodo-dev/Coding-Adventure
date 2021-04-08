using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellPadding : MonoBehaviour
{

    //GameObject cell;

    GridLayoutGroup glg;
    int[] checkArray;
    int[] changeArray;

    // Use this for initialization
    void Start()
    {
        checkArray = new int[100];
        changeArray = new int[100];

        for (int i = 0; i < 100; i++)
        {
            checkArray[i] = 0;
            changeArray[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        checkThePadding();
    }

    public void checkThePadding()
    {
        int childNum = getChildNumber();

        // 배열 초기화
        checkArray = new int[100];

        for (int i = 0; i < 100; i++)
        {
            checkArray[i] = 0;
        }

        // for문과 end문의 위치를 파악한다.
        for (int i = 0; i < childNum; i++)
        {
            GameObject Viewport = this.transform.GetChild(0).gameObject;
            GameObject Content = Viewport.transform.GetChild(0).gameObject;
            DragAndDropCell checkCell = Content.transform.GetChild(i).GetComponent<DragAndDropCell>();

            // null이 아닌 cell의 자식 객체를 찾는다.
            if (checkCell.GetComponentInChildren<DragAndDropItem>() != null)
            {
                if (checkCell.GetComponentInChildren<DragAndDropItem>().name == "ForBlock"
                    || checkCell.GetComponentInChildren<DragAndDropItem>().name == "IfBlock")
                {
                    checkArray[i]++;
                }

                if (checkCell.GetComponentInChildren<DragAndDropItem>().name == "EndBlock"
                    || checkCell.GetComponentInChildren<DragAndDropItem>().name == "IfEndBlock")
                {
                    checkArray[i]--;
                }
            }

            //Debug.Log(checkCell.GetComponentInChildren<DragAndDropItem>().name);
        }

        // checkArray를 통해 changeArray를 만들어 cell의 padding 값을 구한다.
        for (int i = 0; i < childNum; i++)
        {
            if (i - 1 >= 0)
            {
                if (checkArray[i] == 1)
                {
                    if ((checkArray[i - 1] == 1))
                    {
                        changeArray[i] = changeArray[i - 1] + 1;        // 앞에 for문이 있을경우 for문 보다 padding 길이가 길어야한다.
                    }

                    if ((checkArray[i - 1] == 0) || (checkArray[i - 1] == -1))
                    {
                        changeArray[i] = changeArray[i - 1];        // 앞에 check = 0,-1 이 있을경우 다음 역시 0
                    }
                }
                else if (checkArray[i] == -1)
                {
                    if (checkArray[i - 1] == 1)
                    {
                        changeArray[i] = changeArray[i - 1];        // 앞에 for문이 있을경우 for문 보다 padding 길이가 길어야한다.
                    }

                    if ((checkArray[i - 1] == 0) || (checkArray[i - 1] == -1))
                    {
                        changeArray[i] = changeArray[i - 1] - 1;        // 앞에 check = 0,-1 이 있을경우 다음 역시 0
                    }
                }
                else if (checkArray[i] == 0)
                {
                    if (checkArray[i - 1] == 1)
                    {
                        changeArray[i] = changeArray[i - 1] + 1;        // 앞에 for문이 있을경우 for문 보다 padding 길이가 길어야한다.
                    }

                    if ((checkArray[i - 1] == 0) || (checkArray[i - 1] == -1))
                    {
                        changeArray[i] = changeArray[i - 1];        // 앞에 check = 0,-1 이 있을경우 다음 역시 0
                    }
                }
            }
        }

        // padding 값을 설정한다. 
        for (int i = 0; i < childNum; i++)
        {
            GameObject Viewport = this.transform.GetChild(0).gameObject;
            GameObject Content = Viewport.transform.GetChild(0).gameObject;
            DragAndDropCell checkCell = Content.transform.GetChild(i).GetComponent<DragAndDropCell>();

            

           // DragAndDropCell checkCell = this.transform.GetChild(i).GetComponent<DragAndDropCell>();
            glg = checkCell.GetComponent<GridLayoutGroup>();

            glg.padding.left = 50 * changeArray[i];

            // 주석은 칸옮김이 없음
            if (checkCell.GetComponentInChildren<DragAndDropItem>() != null)
            {
                if (checkCell.GetComponentInChildren<DragAndDropItem>().name == "RemarkBlock")
                {
                    glg.padding.left = 0;
                }
            }

            if (glg.padding.left < 0)
            {
                glg.padding.left = 0;
            }
        }

    }

    // 자식의 갯수를 반환한다.
    public int getChildNumber()
    {
        GameObject Viewport = this.transform.GetChild(0).gameObject;
        GameObject Content = Viewport.transform.GetChild(0).gameObject;

        return Content.transform.childCount;
    }
}
