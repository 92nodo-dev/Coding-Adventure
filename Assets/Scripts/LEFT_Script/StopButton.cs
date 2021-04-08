using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour {

    public GameDataManager mydata;
    
    public void OnClick()
    {
        mydata.refresh();
    }
}
