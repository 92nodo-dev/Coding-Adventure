using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawColorBtn : MonoBehaviour {

    public void onClick()
    {
        GameObject DrawOn_object = GameObject.Find("DrawOn");

        switch (this.name)
        {
            case "WhiteButton":
                DrawOn_object.GetComponent<DrawEditor>().drawColor = Color.white;

                break;
            case "BlackButton":
                DrawOn_object.GetComponent<DrawEditor>().drawColor = Color.black;
                break;
            case "RedButton":
                DrawOn_object.GetComponent<DrawEditor>().drawColor = Color.red;
                break;
            case "BlueButton":
                DrawOn_object.GetComponent<DrawEditor>().drawColor = Color.blue;
                break;

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
