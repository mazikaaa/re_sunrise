using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject player;
    int pos_x,pos_y;
    int playerpos_x, playerpos_y;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerpos_x = player.GetComponent<PlayerManager>().mass_x;
        playerpos_y = player.GetComponent<PlayerManager>().mass_y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
