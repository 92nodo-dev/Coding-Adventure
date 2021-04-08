using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUpButton : MonoBehaviour {

    public GameDataManager mydata;

    public void OnClick()
    {
        mydata.setSpeed((mydata.getSpeed()+1) % 8);
    }
}
