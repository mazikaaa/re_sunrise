using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 pos;
    GameObject front, back,groundmanager,gamemanager;
    GameManager game;
    List<GameObject> objects = new List<GameObject>();
    AudioSource audiosource;

    public Text HP_text;
    public GameObject[] field=new GameObject[130];
    public Sprite[] forms = new Sprite[4];
    public AudioClip warp_SE,not_SE;
    float x, y;
    bool goalflag = false,overflag=false;//ゴールしたかどうか,ゲームオーバーかどうか

    public bool fieldflag = true;//trueなら表面,falseなら裏面

    public int HP = 100;
    public int mass_x=0, mass_y=0;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        front = GameObject.Find("Front");
        this.gameObject.transform.position = new Vector3(-7.50f + 0.80f * mass_x, 3.50f + -0.80f * mass_y, 0.0f);

        for (int i = 0; i < front.transform.childCount; i++)
        {
            field[i] = front.transform.GetChild(i).gameObject;
        }

        groundmanager = GameObject.Find("GroundManager");
        objects = groundmanager.GetComponent<GroundManager>().objects;

        gamemanager = GameObject.Find("GameManager");
        game = gamemanager.GetComponent<GameManager>();

        GameObject HPobj = GameObject.Find("HP_Text");
        HP_text = HPobj.GetComponent<Text>();
        HP_text.text = HP.ToString();

        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Action(mass_x,mass_y);
    }

    private void Move()
    {
        int status = game.status;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CheckField(mass_x, mass_y -1,0) == false)
                return;

            this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[0];
            pos = this.gameObject.transform.position;
            x = 0.0f;
            y = 0.8f;
            mass_y--;
            rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));

            if (status == 3)
            {
                HP -= 1;
            }
            else if (status == 0 || status == 2)
            {
                HP -= 2;
            }
            else
            {
                HP -= 5;
            }
            HP_text.text = HP.ToString();
            gamemanager.GetComponent<GameManager>().GainTime();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CheckField(mass_x, mass_y + 1,2) == false)
                return;

            this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[2];
            pos = this.gameObject.transform.position;
            x = 0.0f;
            y = -0.8f;
            mass_y++;
            rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));

            if (status == 3)
            {
                HP -= 1;
            }
            else if (status == 2 || status == 0)
            {
                HP -= 2;
            }
            else
            {
                HP -= 5;
            }
            HP_text.text = HP.ToString();
            gamemanager.GetComponent<GameManager>().GainTime();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CheckField(mass_x+1, mass_y,1) == false)
                return;

            this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[1];
            pos = this.gameObject.transform.position;
            x = 0.8f;
            y = 0.0f;
            mass_x++;
            rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));

            if (status == 3)
            {
                HP -= 1;
            }
            else if (status == 2 || status == 0)
            {
                HP -= 2;
            }
            else
            {
                HP -= 5;
            }
            HP_text.text = HP.ToString();
            gamemanager.GetComponent<GameManager>().GainTime();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CheckField(mass_x-1, mass_y,3 ) == false)
                return;

            this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[3];
            pos = this.gameObject.transform.position;
            x = -0.8f;
            y = 0.0f;
            mass_x--;
            rigid.MovePosition(new Vector3(pos.x + x, pos.y + y, pos.z));

            if (status == 3)
            {
                HP -= 1;
            }
            else if (status == 0 || status == 2)
            {
                HP -= 2;
            }
            else
            {
                HP -= 5;
            }
            HP_text.text = HP.ToString();
            gamemanager.GetComponent<GameManager>().GainTime();
        }

        if (HP <= 0&&!overflag)
        {
            game.GameOver();
            overflag = true;
        }
    }

    //dirは進行方向(0:上　1:右　2:下　3:左)
    private bool CheckField(int x, int y,int dir)
    {
        if (x < 0 || x >= 13 | y < 0 || y >= 10)
            return false;

        string type = field[x + y * 13].GetComponent<FieldManager>().type;
    　　bool stone= field[x + y * 13].GetComponent<FieldManager>().stoneflag;

        if (stone)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                int mass_x=objects[i].GetComponent<stone>().mass_x;
                int mass_y=objects[i].GetComponent<stone>().mass_y;

                if (mass_x == x && mass_y == y)
                {
                    if (!(objects[i].GetComponent<stone>().Move(dir)))
                        return false;
                    else
                        return true;
                }
            }
        }

        if (type == "block"||type=="hole")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Action(int x,int y)
    {
        string type = field[x + y * 13].GetComponent<FieldManager>().type;

        switch (type)
        {
            case"door":
                DoorAction();
                break;
            case "goal":
                GoalAction();
                break;
            default:
                break;
        }
    }

    private void DoorAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fieldflag)
            {  
                if (!DoorCheck(fieldflag))
                {
                    audiosource.PlayOneShot(not_SE);
                    return;
                }

                groundmanager.GetComponent<GroundManager>().ChangeField();
                back = GameObject.Find("Back");


                for (int i = 0; i < back.transform.childCount; i++)
                {
                    field[i] = back.transform.GetChild(i).gameObject;
                }

                objects = groundmanager.GetComponent<BackGroundManager>().objects;

                fieldflag = false;
                gamemanager.GetComponent<GameManager>().DoorChangeStatus();
            }
            else
            {
                if (!DoorCheck(fieldflag))
                {
                    audiosource.PlayOneShot(not_SE);    
                    return;
                }

                groundmanager.GetComponent<BackGroundManager>().ChangeField();
                front = GameObject.Find("Front");

                for (int i = 0; i < front.transform.childCount; i++)
                {
                    field[i] = front.transform.GetChild(i).gameObject;
                }

                objects = groundmanager.GetComponent<GroundManager>().objects;

                fieldflag = true;
                gamemanager.GetComponent<GameManager>().DoorChangeStatus();
            }
            audiosource.PlayOneShot(warp_SE);
        }
    }

    private bool DoorCheck(bool flag)
    {
        GameObject backfield;
        if (flag)
        {
            backfield = groundmanager.GetComponent<BackGroundManager>().fields[mass_x + mass_y * 13];
            if (backfield.GetComponent<FieldManager>().stoneflag)
                return false;
        }
        else
        {
            backfield = groundmanager.GetComponent<GroundManager>().fields[mass_x + mass_y * 13];
            if (backfield.GetComponent<FieldManager>().stoneflag)
                return false;
        }
        return true;
    }

    private void GoalAction()
    {
        if (!goalflag)
        {
            game.GameClear();
            goalflag = true;
        }
    }

}
