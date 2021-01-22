using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snow : MonoBehaviour
{
    public bool snowflag = true;
    public Sprite[] field_image=new Sprite[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckDayStatus(int status)
    {

        if (status == 1)
        {
            snowflag = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = field_image[0];
            this.gameObject.GetComponent<FieldManager>().type = "plane";
        }
        else
        {
            snowflag = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = field_image[1];
            this.gameObject.GetComponent<FieldManager>().type = "block";
        }
    }
}
