using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IfBlockClickNotify : MonoBehaviour
{
    public Sprite[] sprite = new Sprite[6];
    // Use this for initialization
    void Start()
    {

    }

    public void OnclickNotify()
    {
        GameObject block = this.transform.parent.gameObject;
        // Debug.Log(block.name);
        GameObject cell = block.transform.parent.gameObject;
        GameObject Content = cell.transform.parent.gameObject;
        GameObject Viewport = Content.transform.parent.gameObject;


        GameObject inventory = Viewport.transform.parent.gameObject;
        //Debug.Log(inventory.name);

        if (inventory.name == "Task_Inventory")
            StartCoroutine(NotifyClick(this.gameObject));
    }

    private IEnumerator NotifyClick(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("getIfBlock", g);
    }

    public void getNumber(int num)
    {
        this.gameObject.GetComponent<Image>().sprite = sprite[num];

        //숫자 블록의 값을 찾아낼 수 있는 코드 
        //Debug.Log(this.gameObject.GetComponent<Image>().sprite.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
