using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockNotify : MonoBehaviour
{
    public Sprite[] sprite = new Sprite[10];
    public Sprite[] spriteMode = new Sprite[3];
    int count = 0;

    // Use this for initialization
    void Start()
    {
        //image = new Image[10];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickNotify()
    {
        // Debug.Log(this.name);
        GameObject blockMode = this.transform.parent.gameObject;
        // Debug.Log(block.name);
        GameObject block = blockMode.transform.parent.gameObject;
        GameObject cell = block.transform.parent.gameObject;
        GameObject Content = cell.transform.parent.gameObject;
        GameObject Viewport = Content.transform.parent.gameObject;

        GameObject inventory = Viewport.transform.parent.gameObject;
        //Debug.Log(inventory.name);

        if (inventory.name == "Task_Inventory")
        {
            StartCoroutine(NotifyClick(this.gameObject));

          

            //string spriteName = blockMode.GetComponent<Image>().sprite.name;
            //switch (spriteName)
            //{
            //    case "BasicMode":
            //        blockMode.GetComponent<Image>().sprite = spriteMode[1]; break;
            //    case "ArrayMode":
            //        blockMode.GetComponent<Image>().sprite = spriteMode[2]; break;
            //    case "PointMode":
            //        blockMode.GetComponent<Image>().sprite = spriteMode[0]; break;
            //}

        }

        // 하이라이트가 넘버블록을 클릭하여도 켜진다.
        block.GetComponent<BlockHighLightNotify>().onClick();

    }

    public void getBlockMode(int num)
    {
        GameObject blockMode = this.transform.parent.gameObject;
        blockMode.GetComponent<Image>().sprite = spriteMode[num];
    }

    public void getNumber(int num)
    {
        this.gameObject.GetComponent<Image>().sprite = sprite[num];

        //숫자 블록의 값을 찾아낼 수 있는 코드 
        //Debug.Log(this.gameObject.GetComponent<Image>().sprite.name);
    }

    private IEnumerator NotifyClick(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getTaskBlock", g);   // NumberManager
        gameObject.SendMessageUpwards("getNumberBlock", g);
    }

    //private IEnumerator Notify
}
