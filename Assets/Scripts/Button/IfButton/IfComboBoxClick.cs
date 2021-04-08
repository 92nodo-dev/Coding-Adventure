using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IfComboBoxClick : MonoBehaviour
{

    GameObject items;   // 콤보박스
    bool ComboListCondition = false;
    GameObject item;    
    bool itemCondition = false;
    GameObject ifBlock; // ifblock의 콤보버튼
    bool ifBlockCondition = false;

    bool isMouseClicked = false;

     bool isTaskInventory = false;

    // Use this for initialization
    void Start()
    {
        // items를 처음엔 안보이게 한다. 
        items = this.transform.GetChild(0).gameObject;
        items.SetActive(ComboListCondition);
    }

    // Update is called once per frame
    void Update()
    {

        // item버튼을 클릭, ifBlock을 클릭하면 이미지가 바뀐다.
        if (itemCondition == true && ifBlockCondition == true)
        {
            //Debug.Log(item.transform.position);
            ifBlock.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
            itemCondition = false;
            ifBlockCondition = false;

            items.transform.position = Input.mousePosition;
            items.SetActive(false);
            ComboListCondition = false;
        }


        if (Input.GetMouseButtonDown(0))
        {
            bool currentPosition = true;
            // 콤보박스가 열려있을때 
            if (ComboListCondition == true)
            {
                Vector3 mouse_p = Input.mousePosition;

                // items의 위치
                Vector3 itemsPosition = items.transform.position;
                Vector3 s1 = new Vector3();
                Vector3 s2 = new Vector3();
                Vector3 s3 = new Vector3();
                Vector3 s4 = new Vector3();

                s1.x = itemsPosition.x - 125f; s1.y = itemsPosition.y - 18f;
                s2.x = itemsPosition.x + 125f; s2.y = itemsPosition.y - 18f;
                s3.x = itemsPosition.x - 125f; s3.y = itemsPosition.y - 158f;
                s4.x = itemsPosition.x + 125f; s4.y = itemsPosition.y - 158f;

                if ((mouse_p.x > s1.x) && (mouse_p.x < s2.x) && (mouse_p.y < s1.y) && (mouse_p.y > s3.y))
                {

                }
                else
                {
                    items.SetActive(false);
                    ComboListCondition = false;
                }
                // Debug.Log(items.transform.position);
            }
        }

        if (items.active == true)
        {
            items.transform.position = ifBlock.transform.position;
        }

    }


    public void getOperatorBlock(GameObject g)
    {
        item = g;
        itemCondition = true;
    }

    public void getIfBlock2(GameObject g)
    {
        ifBlock = g;
        ifBlockCondition = true;
        //Debug.Log(ifBlock.name);
        isTaskInventory = true;
    }

    public void ComboButtonClick()
    {

        //if (ComboListCondition == true)
        //{
        //    items.transform.position = Input.mousePosition;
        //    items.SetActive(false);
        //    ComboListCondition = false;
        //}
        //else if 
        // 
        // 블록이 TaskInventory에 있어야함 
        if (isTaskInventory == true)
        {
            items.transform.position = Input.mousePosition;
            items.SetActive(true);
            ComboListCondition = true;

            isTaskInventory = false;
        }
        //if (ComboListCondition == false)
        //{

        //}
    }
}
