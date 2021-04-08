using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReset : MonoBehaviour
{

    GameObject Task_Inventory;
    int cellCount;

    // Use this for initialization
    void Start()
    {


        //Debug.Log(num);
    }

    // 블록 리셋 버튼 Task_inventory에 cell 개수를 확인하여 총 개수 -3 을 하여 item을 삭제한다.
    public void deleteBlock()
    {
        Task_Inventory = GameObject.Find("Task_Inventory");
        GameObject viewport = Task_Inventory.transform.GetChild(0).gameObject;
        GameObject content = viewport.transform.GetChild(0).gameObject;
        cellCount = content.transform.childCount;

        //Debug.Log(cellCount);
        for (int i = 0; i < cellCount - 3; i++)
        {
            DragAndDropCell c_cell = content.transform.GetChild(i).GetComponent<DragAndDropCell>();
            DragAndDropItem c_item = c_cell.GetItem();
            if (c_item != null)
            {
                Destroy(c_item.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
