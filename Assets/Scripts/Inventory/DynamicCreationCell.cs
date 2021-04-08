using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DynamicCreationCell : MonoBehaviour
{

    public DragAndDropCell sourceCell;  // 복사할 cell
    static public DragAndDropCell cell;
    int num = 0;

    int cellNumber = 0;             // 자식 object의 개수

    // Use this for initialization
    void Start()
    {
        //sourceCell.transform.SetParent(this.transform, false);


        //DragAndDropCell c_cell = this.transform.GetChild(3).GetComponent<DragAndDropCell>();
        //Debug.Log(c_cell.name);
        //Debug.Log(this.transform.GetChild(2).name);
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(0)&& num < 2)
        //{
        //    createDynamicCell();


        //    num++;
        //}
        //Debug.Log("");
        getNumberOfCell();
        deleteCellManager();
        createCellManager();

    }

    // 셀 갯수는 3개 이상이고 cell의 자식 객체가 없으면 삭제한다. 
    public void deleteCellManager()
    {
        if (cellNumber > 12)
        {
            for (int i = 0; i < cellNumber - 12; i++)
            {
                GameObject viewport = this.transform.GetChild(0).gameObject;
                GameObject content = viewport.transform.GetChild(0).gameObject;
                DragAndDropCell c_cell = content.transform.GetChild(i).GetComponent<DragAndDropCell>();

                //DragAndDropCell c_cell = this.transform.GetChild(i).GetComponent<DragAndDropCell>();
                DragAndDropItem c_item = c_cell.GetItem();
                if (c_item == null)
                {
                    Destroy(c_cell.gameObject);
                }
            }
        }

    }

    //(셀의 개수-2 ~셀의 개수)  범위 내에 items가 들어온다면 동적생성
    public void createCellManager()
    {
        for (int i = cellNumber - 12; i < cellNumber; i++)
        {

            GameObject viewport = this.transform.GetChild(0).gameObject;
            GameObject content = viewport.transform.GetChild(0).gameObject;
            DragAndDropCell c_cell = content.transform.GetChild(i).GetComponent<DragAndDropCell>();

            DragAndDropItem c_item = c_cell.GetItem();

            if (c_item != null)
            {
                createDynamicCell();
                break;
            }
        }

        getNumberOfCell();
    }

    public void createDynamicCell()
    {
        cell = new DragAndDropCell();
        cell = Instantiate(sourceCell) as DragAndDropCell;

        GameObject viewport = this.transform.GetChild(0).gameObject;
        GameObject content = viewport.transform.GetChild(0).gameObject;

        cell.transform.SetParent(content.transform, false);
        cell.name = "DynamicCell";
    }

    public void createDynamicCell(bool isTrue)
    {
        if (isTrue == true)
        {
            cell = new DragAndDropCell();
            cell = Instantiate(sourceCell) as DragAndDropCell;

            GameObject viewport = this.transform.GetChild(0).gameObject;
            GameObject content = viewport.transform.GetChild(0).gameObject;

            cell.transform.SetParent(content.transform, false);
            cell.name = "DynamicCell";
        }
    }

    public void getNumberOfCell()
    {
        GameObject viewport = this.transform.GetChild(0).gameObject;
        GameObject content = viewport.transform.GetChild(0).gameObject;


        cellNumber = content.transform.childCount;        //  child object의 개수
    }

    public DragAndDropItem GetItem()
    {
        return GetComponentInChildren<DragAndDropItem>();
    }

    public DragAndDropCell nextCell(DragAndDropCell cell)
    {
        int index = cell.transform.GetSiblingIndex();
        DragAndDropCell nCell = null;

        nCell = cell.transform.parent.GetChild(index + 1).GetComponent<DragAndDropCell>();

        return nCell;
    }
}
