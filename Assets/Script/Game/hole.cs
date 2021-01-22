using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hole : MonoBehaviour
{
    public Sprite[] field_image=new Sprite[3];

    GameObject snowball;

    public bool snowhole = false;//雪で穴が埋まっているか？

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillHole(GameObject obj)
    {
        string name = obj.name;

        if (name == "snowball(Clone)")
        {
            snowball = obj;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = field_image[2];
            this.gameObject.GetComponent<FieldManager>().type = "plane";
            snowhole = true;

        }
        else if (name == "stone(Clone)")
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = field_image[1];
            this.gameObject.GetComponent<FieldManager>().type = "plane";

        }
    }

    public void CheckDayStatus(int status)
    {
        if (status == 1 && snowhole)
        {
            snowhole = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = field_image[0];
            this.gameObject.GetComponent<FieldManager>().type = "hole";
            snowball.GetComponent<stone>().existflag = true;


        }

    }


}
