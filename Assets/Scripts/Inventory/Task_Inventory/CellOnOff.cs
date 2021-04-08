using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOnOff : MonoBehaviour {

    int childNum = 0;

	// Use this for initialization
	void Start () {
        childNum = this.transform.childCount;
	}

    // Update is called once per frame
    void Update() {
        DragAndDropCell cell;
        childNum = this.transform.childCount;

        // cell 위에서 밑에서 3번째 까지 
        for (int i = 0; i < (childNum - 2); i++)
        {
            cell = this.transform.GetChild(i).GetComponent<DragAndDropCell>();
            //cell.getMsg(i.ToString());
            cell.getCellOnOff(true);
        }

        // cell 밑에서 2개
        for (int i = (childNum - 2); i < childNum; i++)
        {
            cell = this.transform.GetChild(i).GetComponent<DragAndDropCell>();

            //cell.getMsg(i.ToString());
            cell.getCellOnOff(false);
        }
	}
}
