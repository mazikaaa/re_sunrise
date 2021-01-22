using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    int audioflag =0;

    AudioSource audiosource;
    public AudioClip button_SE;
    public GameObject audio;

    int stageNo=0;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audioflag = PlayerPrefs.GetInt("Audio",1);
        if (audioflag == 0)
        {
            Instantiate(audio);
            PlayerPrefs.SetInt("Audio", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoGame(int i)
    {
        audiosource.PlayOneShot(button_SE);
        stageNo = i;
        Invoke("Game", 0.3f);
    }

    public void Game()
    {
        SceneManager.LoadScene("Game" + stageNo);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Audio", 0);
    }
}
