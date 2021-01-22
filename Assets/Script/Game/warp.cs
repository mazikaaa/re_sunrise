using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour
{
    public bool warpflag=false;

    GameObject warpobj,groundmanager;
    public GameObject front, back;

    int mass_x,mass_y;

    // Start is called before the first frame update
    void Start()
    {
        Getfield();
    }

    // Update is called once per frame
    void Update()
    {
        if (warpflag && Input.GetKeyDown(KeyCode.Return))
        {
            if (front.activeSelf)
            {
                groundmanager.GetComponent<GroundManager>().WarpObject(warpobj,mass_x,mass_y);
            }
            else
            {
                Debug.Log("warp");
                groundmanager.GetComponent<BackGroundManager>().WarpObject(warpobj, mass_x, mass_y);
            }
            warpflag = false;
        }
    }

    public void CanWarp(int x,int y,GameObject stone)
    {
        warpflag = true;
        mass_x = x;
        mass_y = y;
        warpobj = stone;    
    }

    public void Getfield()
    {
        groundmanager = GameObject.Find("GroundManager");
        front = groundmanager.transform.Find("Front").gameObject;
        back = groundmanager.transform.Find("Back").gameObject;
    }
}
