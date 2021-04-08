using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void OnclickNotify()
    {
        StartCoroutine(NotifyClick(this.gameObject));
    }

    private IEnumerator NotifyClick(GameObject g)
    {
        while (g == null)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("SelectBlockMode", g);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
