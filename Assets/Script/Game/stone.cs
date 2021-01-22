using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 pos;
    float x, y;

    //public string name;

    GameObject front, back, groundmanager,player;
    GameManager game;

    public GameObject[] field = new GameObject[130];

    public bool fieldflag;//岩があるのがtrueなら表面,falseなら裏面

    public int mass_x = 0, mass_y = 0;

    public bool existflag = true;//岩が存在するか(穴に落ちてないか)

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        groundmanager = GameObject.Find("GroundManager");

        front = groundmanager.transform.Find("Front").gameObject;
        back = groundmanager.transform.Find("Back").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Move(int dir)
    {
        //0:上　1;右　2:下   3:左
        switch (dir)
        {
            case 0:
                if (CheckField(mass_x, mass_y - 1) == false)
                    return false;

                LeaveStone();
                pos = this.gameObject.transform.position;
                x = 0.0f;
                y = 0.8f;
                mass_y--;

                if (existflag)
                    PutStone();
                else
                {
                    mass_x = -1;
                    mass_y = -1;
                    this.gameObject.SetActive(false);
                }

                rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));
                return true;
            case 1:
                if (CheckField(mass_x + 1, mass_y) == false)
                    return　false;

                LeaveStone();
                pos = this.gameObject.transform.position;
                x = 0.8f;
                y = 0.0f;
                mass_x++;

                if (existflag)
                    PutStone();
                else
                {
                    mass_x = -1;
                    mass_y = -1;
                    this.gameObject.SetActive(false);
                }

                rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));
                return true;
            case 2:
                if (CheckField(mass_x, mass_y +1) == false)
                    return false;

                LeaveStone();
                pos = this.gameObject.transform.position;
                x = 0.0f;
                y = -0.8f;
                mass_y++;

                if (existflag)
                    PutStone();
                else
                {
                    mass_x = -1;
                    mass_y = -1;
                    this.gameObject.SetActive(false);
                }


                rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));
                return true;
            case 3:
                if (CheckField(mass_x-1, mass_y) == false)
                    return false;

                LeaveStone();
                pos = this.gameObject.transform.position;
                x =- 0.8f;
                y = 0.0f;
                mass_x--;

                if (existflag)
                    PutStone();
                else
                {
                    mass_x = -1;
                    mass_y = -1;
                    this.gameObject.SetActive(false);
                }


                rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));
                return true;
            default:
                return false;
        }
    
    }

    private bool CheckField(int x, int y)
    {
        if (x < 0 || x >= 13 | y < 0 || y >= 10)
            return false;

        string type = field[x + y * 13].GetComponent<FieldManager>().type;
        bool stone = field[x + y * 13].GetComponent<FieldManager>().stoneflag;

        if (stone)
        {
            return false;
        }

        if (type == "block")
        {
            return false;
        }
        else if(type=="hole")
        {
            field[x + y * 13].GetComponent<hole>().FillHole(this.gameObject);
            existflag = false;
            return true;
        }
        else if (type=="warp")
        {
            field[x + y * 13].GetComponent<warp>().CanWarp(x,y,this.gameObject);
            return true;
        }
        else
        {
            return true;
        }
    }

    public void FetchFeild(bool flag)
    {
        groundmanager = GameObject.Find("GroundManager");
        front = groundmanager.transform.Find("Front").gameObject;
        back = groundmanager.transform.Find("Back").gameObject;

        //trueなら表面,falseなら裏面
        if (flag)
        {
            fieldflag = true;
            for (int i = 0; i < front.transform.childCount; i++)
            {
                field[i] = front.transform.GetChild(i).gameObject;
            }
        }
        else
        {
            for (int i = 0; i < back.transform.childCount; i++)
            {
                field[i] = back.transform.GetChild(i).gameObject;
            }
        }
    }

    public void PutStone()
    {
        field[mass_x + mass_y * 13].GetComponent<FieldManager>().stoneflag = true;
    }

    public void LeaveStone()
    {
        field[mass_x +mass_y * 13].GetComponent<FieldManager>().stoneflag = false;
    }

    private void OnEnable()
    {
        if (!existflag)
            this.gameObject.SetActive(false);
    }
}
