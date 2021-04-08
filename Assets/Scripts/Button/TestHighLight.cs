using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHighLight : MonoBehaviour {

    int count = 0;

	// Use this for initialization
	void Start () {
		
	}

    public void test()
    {
        GameObject Content = GameObject.Find("Content");
        GameObject Cell = Content.transform.GetChild(count).gameObject;
        GameObject block = Cell.transform.GetChild(0).gameObject;

        if (block.name != "RemarkBlock")
        {
            block.GetComponent<BlockHighLightNotify>().onHighLightClick();
        }

        count++;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
