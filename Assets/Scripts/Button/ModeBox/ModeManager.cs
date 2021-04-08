using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour {
    GameObject ModeList;
    GameObject mode;    // 현재 모드
    GameObject numberBlock; // numberBlock 

    bool modeListCondition = false;
    bool modeCondition = false;
    bool numberBlockCondition = false;
    bool isTaskInventory = false;

    // Use this for initialization
    void Start () {
        // ModeList가 targetitem임
        ModeList = this.transform.GetChild(0).gameObject;
        ModeList.SetActive(modeListCondition);
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 s = Input.mousePosition;

        //    Debug.Log(s);
        //}

            // mode 와 numberBlock을 클릭하면 이미지가 바뀐다.
        if (modeCondition == true && numberBlockCondition == true)
        {
            string spriteName = mode.GetComponent<Image>().sprite.name;
            switch (spriteName)
            {
                case "BasicMode":
                    numberBlock.GetComponent<BlockNotify>().getBlockMode(0); break;
                case "ArrayMode":
                    numberBlock.GetComponent<BlockNotify>().getBlockMode(1); break;
                case "PointMode":
                    numberBlock.GetComponent<BlockNotify>().getBlockMode(2); break;
            }

            modeCondition = false;
            numberBlockCondition = false;

            ModeList.transform.position = Input.mousePosition;
            ModeList.SetActive(false);
            modeListCondition = false;

            // 모드 변경시 이미지도 변경된다.
            GameObject nbm = GameObject.Find("Canvas");
            nbm.GetComponent<NumberManager>().reCall_Image(spriteName, numberBlock);
            
        }


        if (Input.GetMouseButtonDown(0))
        {
            bool currentPosition = true;

            if (modeListCondition == true)
            {
                Vector3 mouse_p = Input.mousePosition;

                // items의 위치
                Vector3 ModeListPosition = ModeList.transform.position;
                Vector3 s1 = new Vector3();
                Vector3 s2 = new Vector3();
                Vector3 s3 = new Vector3();
                Vector3 s4 = new Vector3();

                s1.x = ModeListPosition.x + 27.5f; s1.y = ModeListPosition.y + 62.5f;
                s2.x = ModeListPosition.x + 27.5f + 130f; s2.y = ModeListPosition.y + 62.5f;
                s3.x = ModeListPosition.x + 27.5f; s3.y = ModeListPosition.y - 62.5f;
                s4.x = ModeListPosition.x + 27.5f + 130f; s4.y = ModeListPosition.y - 62.5f;

                if ((mouse_p.x > s1.x) && (mouse_p.x < s2.x) && (mouse_p.y < s1.y) && (mouse_p.y > s3.y))
                {

                }
                else
                {
                    ModeList.SetActive(false);
                    modeListCondition = false;
                }
                // Debug.Log(items.transform.position);
            }
        }

        if (ModeList.active == true)
        {
            ModeList.transform.position = numberBlock.transform.position;
            
        }
	}

    public void SelectBlockMode(GameObject g)
    {
        mode = g;
        modeCondition = true;
        //Debug.Log(g.name);
    }

    public void getNumberBlock(GameObject g)
    {
        numberBlock = g;
        numberBlockCondition = true;

        // taskInventory에 있다는게 검증됨
        isTaskInventory = true;
        //Debug.Log(g.name);
    }

    // TaskInventory에 있어야 되게 만들어야함 
    public void NumberButtonClick()
    {
        if (isTaskInventory == true)
        {
            ModeList.transform.position = Input.mousePosition;


            ModeList.SetActive(true);
            modeListCondition = true;

            isTaskInventory = false;
        }
    }

    public void getButtonPosition(Vector3 btnPosition)
    {
        ModeList.transform.position = btnPosition;
        ModeList.SetActive(true);
        modeListCondition = true;
    }

}
