using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject clear, over,background,player;
    public AudioClip clear_SE, over_SE;

    [SerializeField]int time=0;
    public int status = 3;//日の状態を知る  0:明け方　1:昼　2:夕方　3:夜 

    public GroundManager field_front;
    public BackGroundManager field_back;

    int changetime = 10;
    string day = "朝";
    AudioSource audiosource;

    public Image Day;
    public Text time_text;
    public Sprite[] day_image = new Sprite[4];
    public Sprite[] back_image = new Sprite[3];
 
    // Start is called before the first frame update
    void Start()
    {
        ChangeDayImage(status);
        ChangeDayText(status);
        time_text.text = day + "まで" + (changetime - time).ToString();
        audiosource = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainTime()
    {
        time++;

        if (time >= changetime)
        {
            ChangeStatus();
            time = 0;
        }
        time_text.text = day + "まで" + (changetime - time).ToString();
    }

    private void ChangeStatus()
    {
        status++;
        status = status % 4;
        ChangeDayImage(status);
        ChangeDayText(status);

        field_front.CheckField();
        field_back.CheckField();

        time_text.text = day + "まで" + (changetime - time).ToString();
    }

    public void DoorChangeStatus()
    {
        status += 2;
        status = status % 4;
        ChangeDayImage(status);
        ChangeDayText(status);

        field_front.CheckField();
        field_back.CheckField();

        time_text.text = day + "まで" + (changetime - time).ToString();
    }

    private void ChangeDayImage(int num)
    {
        Day.sprite = day_image[num];
    }

    private void ChangeDayText(int num)
    {
        switch (num) {
            case 0:
                day = "昼";
                background.GetComponent<SpriteRenderer>().sprite = back_image[0];
                break;
            case 1:
                day = "夕方";
                background.GetComponent<SpriteRenderer>().sprite = back_image[0];
                break;
            case 2:
                day = "夜";
                background.GetComponent<SpriteRenderer>().sprite = back_image[1];
                break;
            case 3:
                day = "朝";
                background.GetComponent<SpriteRenderer>().sprite = back_image[2];
                break;
        }
    }

    public void GameOver()
    {
        audiosource.PlayOneShot(over_SE);
        over.SetActive(true);
        Invoke("AudioStop", 1.0f);
    }

    public void GameClear()
    {
        audiosource.PlayOneShot(clear_SE);
        clear.SetActive(true);
        Invoke("AudioStop", 1.0f);
    }

    public void Replay(int i)
    {
        SceneManager.LoadScene("Game" + i);
    }

    public void AudioStop()
    {
        audiosource.Stop();
    }

    public void Ranking(int i)
    {
        int HP = player.GetComponent<PlayerManager>().HP;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(HP,i-1);
    }

    public void GoSelect()
    {
        PlayerPrefs.SetInt("Audio", 0);
        SceneManager.LoadScene("selectscene");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Audio", 0);
    }

}
