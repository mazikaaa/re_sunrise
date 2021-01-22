using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour
{

    int mass_x, mass_y;

    public int default_x, default_y;

    public bool meltflag = false;//溶けているかどうか

    public bool fieldflag;//雪玉がどちらの面にあるか
    public bool default_field;//最初どちらの面にいるか

    // Start is called before the first frame update
    private void Awake()
    {
       
    }

    void Start()
    {
        default_x = this.gameObject.GetComponent<stone>().mass_x;
        default_y = this.gameObject.GetComponent<stone>().mass_y;
        default_field = this.gameObject.GetComponent<stone>().fieldflag;

        fieldflag= this.gameObject.GetComponent<stone>().fieldflag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckDayStatus(int status)
    {

        GameObject ground = GameObject.Find("GroundManager");
        bool field = ground.GetComponent<GroundManager>().fieldflag;

        if (status == 1&&!meltflag)
        {
            mass_x = this.gameObject.GetComponent<stone>().mass_x;
            mass_y = this.gameObject.GetComponent<stone>().mass_y;
            meltflag =true;

            if (mass_x != -1)
            {
                if (fieldflag != default_field)
                {
                    if (fieldflag)
                        ground.GetComponent<GroundManager>().fields[mass_x + mass_y * 13].GetComponent<FieldManager>().stoneflag = false;
                    else
                        ground.GetComponent<BackGroundManager>().fields[mass_x + mass_y * 13].GetComponent<FieldManager>().stoneflag = false;
                }
            }
            this.gameObject.SetActive(false);
        }
        else if (meltflag)
        {
            meltflag = false;

            if (default_field == field)
                this.gameObject.SetActive(true);
            else
                this.gameObject.SetActive(false);

            if (default_field != fieldflag)
            {
                mass_x = this.gameObject.GetComponent<stone>().mass_x;
                mass_y = this.gameObject.GetComponent<stone>().mass_y;

                if (default_field == false)
                {
                    ground.GetComponent<BackGroundManager>().GetObject(this.gameObject);
                    ground.GetComponent<GroundManager>().RemoveObject(this.gameObject, mass_x, mass_y);
                }
                else
                {
                    ground.GetComponent<GroundManager>().GetObject(this.gameObject);
                    ground.GetComponent<BackGroundManager>().RemoveObject(this.gameObject, mass_x, mass_y);
                }
            
            }

            this.gameObject.GetComponent<stone>().mass_x=default_x;
            this.gameObject.GetComponent<stone>().mass_y=default_y;
            this.gameObject.GetComponent<stone>().fieldflag = default_field;
            this.gameObject.GetComponent<stone>().PutStone();
            this.transform.position = new Vector3(-7.50f + 0.80f * default_x, 3.50f + -0.80f *default_y, 0.0f);
        }
        
    }

    private void OnEnable()
    {
        if (meltflag)
            this.gameObject.SetActive(false);
    }

}
