using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offset : MonoBehaviour {

    public bool negative;

	// Use this for initialization
	void Start () {
        Adjust();
	}

    public void Adjust()
    {
        transform.localPosition = Vector3.zero; //First: reset
        int direction = negative ? -1 : 1;
        Debug.Log(transform.parent.localScale);
        transform.Translate(direction * 0.5f * transform.parent.localScale.x * transform.right);
    }
}