using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAndCombo : MonoBehaviour
{
    GameObject ifBlock;
    GameObject numberBlock;
    // Use this for initialization
    void Start()
    {

    }

    // numberBlock으로 mode변경을 위한 정보를 보낸다.
    public void getNumberBlock(GameObject g) 
    {
        numberBlock = g;
        StartCoroutine(NotifyNumberBlock(numberBlock));
    }

    private IEnumerator NotifyNumberBlock(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }
        GameObject mode = GameObject.Find("ModeBox");
        mode.gameObject.SendMessageUpwards("getNumberBlock", g);
    }

    public void getIfBlock(GameObject g)
    {
        ifBlock = g;
        StartCoroutine(NotifyClick(ifBlock));
    }

    private IEnumerator NotifyClick(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }
        GameObject Combo = GameObject.Find("ComboBox");
        Combo.gameObject.SendMessageUpwards("getIfBlock2", g);
    }

    //public Ray ray;
    //public RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        Debug.Log(hitInfo.transform.gameObject.name); //gameObject name
        //    }
        //}

    }
}
