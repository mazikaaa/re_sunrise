using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundManager : MonoBehaviour
{
    [SerializeField] TextAsset map_text;
    [SerializeField] TextAsset obj_text;

    private string[] mapline;
    private string[] objline;

    public GameObject[] mapgimick = new GameObject[9];
    public GameObject[] mapobject = new GameObject[3];
    public AudioClip warp_SE,not_SE;

    GameObject front,back,warpobject;
    Text field_text;
    public bool fieldflag = true;//プレイヤーがいるならtrue

    GameObject gameManager;
    GameManager game;
    AudioSource audioSource;

    public GameObject[] fields = new GameObject[130];
    public List <GameObject> objects = new List<GameObject>();

    private int[,] mapdata;
    private int[,] objdata;

    int i, j;

    private void Awake()
    {
        front = transform.Find("Front").gameObject;
        back = transform.Find("Back").gameObject;

        ReadText();

        for (i = 0; i < 10; i++)
        {
            for (j = 0; j < 13; j++)
            {

                    GameObject field = Instantiate(mapgimick[this.mapdata[i,j]], new Vector3(-7.50f + 0.80f * j, 3.50f + -0.80f * i, 0.0f), Quaternion.identity);
                    field.GetComponent<FieldManager>().mass_x = j;
                    field.GetComponent<FieldManager>().mass_y = i;
                    field.transform.parent = front.transform;
                
            }
        }


        for (i = 0; i < 10; i++)
        {
            for (j = 0; j < 13; j++)
            {
                if (objdata[i, j] != 0)
                {
                    GameObject obj = Instantiate(mapobject[objdata[i, j]], new Vector3(-7.50f + 0.80f * j, 3.50f + -0.80f * i, 0.0f), Quaternion.identity);
                    obj.GetComponent<stone>().mass_x = j;
                    obj.GetComponent<stone>().mass_y = i;
                    obj.GetComponent<stone>().fieldflag = true;
                    obj.transform.parent = this.gameObject.transform;
                    objects.Add(obj);
                }
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject Field = GameObject.Find("Field_Text");
        field_text = Field.GetComponent<Text>();

        gameManager = GameObject.Find("GameManager");
        game = gameManager.GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < front.transform.childCount; i++)
        {
           fields[i] =front.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i <objects.Count; i++)
        {
            objects[i].GetComponent<stone>().FetchFeild(true);
            objects[i].GetComponent<stone>().fieldflag = true;
            objects[i].GetComponent<stone>().PutStone();
        }
        //CheckField();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeField()
    {
        field_text.text = "裏";
        back.SetActive(true);
        front.SetActive(false);
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }

        fieldflag = false;
        this.gameObject.GetComponent<BackGroundManager>().fieldflag = true;
        this.gameObject.GetComponent<BackGroundManager>().ObjectActive();
        
        //this.gameObject.GetComponent<GroundManager>().enabled = false;
    }

    public void CheckField()
    {
        int status=game.status;

        if (!fieldflag)
        {
            status += 2;
            status = status % 4;
        }

        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].name == "snow_mount(Clone)")
            {
                fields[i].GetComponent<snow>().CheckDayStatus(status);
            }
            else if (fields[i].name == "hole(Clone)")
            {
                if (fields[i].GetComponent<hole>().snowhole)
                {
                    fields[i].GetComponent<hole>().CheckDayStatus(status);
                }
            }
        }

        int snowcount = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].name == "snowball(Clone)"&&snowcount<1)
            {
                objects[i].GetComponent<snowball>().CheckDayStatus(status);
                snowcount++;
            }
        }

    }

    public void ObjectActive()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(true);
        }
    }
    
    public void WarpObject(GameObject obj,int x,int y)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i].GetComponent<stone>().mass_x==x && objects[i].GetComponent<stone>().mass_y == y)
            {
                List<GameObject> backobjects = this.gameObject.GetComponent<BackGroundManager>().objects;
                //裏側の同じ位置に岩が置いてあった場合
                for(int j = 0; j < backobjects.Count; j++)
                {
                    if(x==backobjects[j].GetComponent<stone>().mass_x && y== backobjects[i].GetComponent<stone>().mass_y)
                    {
                        audioSource.PlayOneShot(not_SE);
                        return;
                    }
                }

                if (objects[i].name == "snowball(Clone)")
                {
                    objects[i].GetComponent<snowball>().fieldflag=false;
                }

                objects[i].GetComponent<stone>().FetchFeild(false);
                objects[i].GetComponent<stone>().PutStone();
                fields[x + y * 13].GetComponent<FieldManager>().stoneflag = false;
                warpobject = objects[i];
                objects.RemoveAt(i);
                this.gameObject.GetComponent<BackGroundManager>().GetObject(warpobject,true);
                obj.SetActive(false);
                audioSource.PlayOneShot(warp_SE);
            }
        }
    }

    public bool RemoveObject(GameObject obj, int x, int y)
    {
        for (int i = 0; i < objects.Count; i++)
        {

            if (objects[i].GetComponent<stone>().mass_x == x && objects[i].GetComponent<stone>().mass_y == y)
            {
                objects[i].GetComponent<stone>().FetchFeild(false);

                if (objects[i].name == "snowball(Clone)")
                {
                    if (objects[i].GetComponent<stone>().mass_x == objects[i].GetComponent<snowball>().default_x && objects[i].GetComponent<stone>().mass_y == objects[i].GetComponent<snowball>().default_y)
                        objects[i].GetComponent<stone>().PutStone();
                }
                objects.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void GetObject(GameObject obj,bool warp)
    {
        objects.Add(obj);
        if(warp)
        CheckField();
    }



    private void ReadText()
    {
        int i, j,line,raw;
        string textlines;

        //マップデータ用
        textlines = map_text.text;
        mapline = textlines.Split('\n');
        line = mapline.Length;
        raw = mapline[0].Split(',').Length;

        mapdata =new int[line, raw];
        for (i = 0; i < line; i++)
        {
            string[] text = mapline[i].Split(',');
            for (j = 0; j < raw; j++)
            {
                mapdata[i, j] = int.Parse(text[j]);
            }
        }

        //オブジェクト用
        textlines = obj_text.text;
        mapline = textlines.Split('\n');
        line = mapline.Length;
        raw = mapline[0].Split(',').Length;

        objdata = new int[line, raw];
        for (i = 0; i < line; i++)
        {
            string[] text = mapline[i].Split(',');
            for (j = 0; j < raw; j++)
            {
                objdata[i, j] = int.Parse(text[j]);
            }
        }
    }
}
