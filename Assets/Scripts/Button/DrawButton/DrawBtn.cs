using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBtn : MonoBehaviour {

    GameObject Canvas;

    //GameObject b;
	// Use this for initialization
	void Start () {
        Canvas = GameObject.Find("Canvas");


	}

    public void OnHome()
    {
        Canvas.GetComponent<DrawManager>().CloseWindow();
    }

    public void OnSave()
    {

    }

    public void OnErase()
    {

    }

    // Update is called once per frame
    void Update () {
        //b = GameObject.Find("DrawWindow");
        //print(b.transform.position);
	}
}
